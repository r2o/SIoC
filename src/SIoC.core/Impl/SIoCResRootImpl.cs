namespace SIoC.core.Impl
{
    using System;
    using System.Collections;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    public class SIoCResRootImpl : IIoCResolutionRoot, IIoCBindingRoot
    {
        private HashSet<Type> singletonTypes;

        private HashSet<Type> constantTypes;

        private HashSet<Type> transitentTypes;

        private ConcurrentDictionary<Type, object> singletonInstance;

        private ConcurrentDictionary<Type, object> constantInstance;

        private ConcurrentDictionary<Type, TypeCache> singletonImpl;

        private ConcurrentDictionary<Type, TypeCache> constantImpl;

        private ConcurrentDictionary<Type, TypeCache> transientImpl;

        private ConcurrentDictionary<Type, Func<object>> methodImpl;

        private ConcurrentDictionary<Type, BindingType> bindingType;

        private ConcurrentDictionary<Type, object> array;

        internal enum BindingType
        {
            None,
            Singleton,
            Constant,
            Method,
            Transient
        }

        public SIoCResRootImpl()
        {
            singletonTypes = new HashSet<Type>();
            constantTypes = new HashSet<Type>();
            transitentTypes = new HashSet<Type>();
            singletonInstance = new ConcurrentDictionary<Type, object>();
            constantInstance = new ConcurrentDictionary<Type, object>();
            singletonImpl = new ConcurrentDictionary<Type, TypeCache>();
            constantImpl = new ConcurrentDictionary<Type, TypeCache>();
            transientImpl = new ConcurrentDictionary<Type, TypeCache>();
            methodImpl = new ConcurrentDictionary<Type, Func<object>>();
            bindingType = new ConcurrentDictionary<Type, BindingType>();
            array = new ConcurrentDictionary<Type, object>();
        }
        
        public void BindInSingleton<TInterface, TImplementation>() where TImplementation : TInterface
        {
            BindInSingleton(typeof(TInterface), typeof(TImplementation));
        }

        public void BindInSingleton<TInterface>(Type timpl)
        {
            BindInSingleton(typeof(TInterface), timpl);
        }

        public void BindInSingleton(Type tInt, Type tImpl)
        {
            AddBindingType(tInt, BindingType.Singleton);
            if (!singletonTypes.Add(tInt))
            {
                throw new Exception(string.Format("There is already a definition for type {0}", tInt.FullName));
            }

            var ctor = tImpl.GetConstructors().FirstOrDefault();
            var cache = new TypeCache();
            cache.TImpl = tImpl;
            cache.CtorParms = ctor.GetParameters();
            singletonImpl.TryAdd(tInt, cache);
        }

        public void BindInTransient<TInterface, TImplementation>() where TImplementation : TInterface
        {
            BindInTransient(typeof(TInterface), typeof(TImplementation));
        }

        public void BindInTransient<TInterface>(Type timpl)
        {
            BindInTransient(typeof(TInterface), timpl);
        }

        public void BindInTransient(Type tInt, Type tImpl)
        {
            AddBindingType(tInt, BindingType.Transient);

            if (!transitentTypes.Add(tInt))
            {
                throw new Exception(string.Format("There is already a definition for type {0}", tInt.FullName));
            }

            var ctor = tImpl.GetConstructors().FirstOrDefault();
            var cache = new TypeCache();
            cache.TImpl = tImpl;
            cache.CtorParms = ctor.GetParameters();
            transientImpl.TryAdd(tInt, cache);
        }

        public void BindToMethod<TInterface>(Func<TInterface> func)
        {
            Func<object> method = func as Func<object>;
            BindToMethod(typeof(TInterface), method);
        }

        public void BindToMethod(Type tint, Func<object> obj)
        {
            AddBindingType(tint, BindingType.Method);

            if (!methodImpl.TryAdd(tint, obj))
            { 
                throw new Exception(string.Format("There is already a definition for type {0}", tint.FullName));
            }
        }

        public void BindToConstant<TInterface>(TInterface obj)
        {
            BindToConstant(typeof(TInterface), obj);
        }

        public void BindToConstant(Type tint, object obj)
        {
            AddBindingType(tint, BindingType.Constant);
            if (!constantTypes.Add(tint))
            {
                throw new Exception(string.Format("There is already a definition for type {0}", tint.FullName));
            }

            constantInstance.TryAdd(tint, obj);
        }

        public bool HasBindingFor<T>()
        {
            return HasBindingFor(typeof(T));
        }

        public bool HasBindingFor(Type tint)
        {
            return bindingType.ContainsKey(tint);
        }

        public T Get<T>()
        {
            return (T)Get(typeof(T));
        }

        public T Get<T>(Type tint)
        {
            return (T)Get(tint);
        }

        public object Get(Type tint)
        {
            return ResolveType(tint);
        }

        #region Resolution

        private object ResolveType(Type t)
        {
            BindingType bt;
            bindingType.TryGetValue(t, out bt);
            switch (bt)
            {
                case BindingType.Constant:
                    return ResolveConstant(t);
                case BindingType.Method:
                    return (ResolveMethod(t) as Func<object>)();
                case BindingType.Singleton:
                    return ResolveSingleton(t);
                case BindingType.Transient:
                    return ResolveTransient(t);
                default:
                    if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                    {
                        return ResolveList(t);
                    }

                    if (typeof(MulticastDelegate).IsAssignableFrom(t))
                    {
                        var tmp = t.GetGenericArguments()[0];
                        var tmpVal = ResolveType(tmp);
                        var block = Expression.Block(Expression.Constant(tmpVal));
                        var res = Expression.Lambda(block);
                        var cmp = res.Compile();
                        return cmp;
                    }

                    throw new Exception(string.Format("No bindings found for type {0}", t.FullName));
            }
        }

        private object ResolveList(Type t)
        {
            var realType = t.GetGenericArguments().First();
            var lst = typeof(List<>);
            Type r = lst.MakeGenericType(new Type[] { realType });
            var addm = r.GetMethod("Add");
            var list = Activator.CreateInstance(r);
            var rs = ResolveType(realType);
            object result;
            if (rs.GetType().IsGenericType && typeof(IEnumerable).IsAssignableFrom(rs.GetType().GetGenericTypeDefinition()))
            {
                var irs = (IEnumerable)rs;
                var ienu = irs.GetEnumerator();
                while (ienu.MoveNext())
                {
                    var tt = ResolveType((Type)ienu.Current);
                    addm.Invoke(list, new object[] { tt });
                }
            }
            else
            {
                result = rs;
                addm.Invoke(list, new object[] { result });
            }

            return list;
        }

        private object ResolveMethod(Type t)
        {
            Func<object> tmp;
            methodImpl.TryGetValue(t, out tmp);
            var obj = tmp();
            if (obj.GetType().IsGenericType && (obj.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) || obj.GetType().GetGenericTypeDefinition() == typeof(IList<>)))
            {
                return ResolveList(obj.GetType());
            }

            return tmp;
        }

        private object ResolveConstant(Type t)
        {
            object rs;
            constantInstance.TryGetValue(t, out rs);
            if (rs.GetType().IsGenericType && (rs.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) || rs.GetType().GetGenericTypeDefinition() == typeof(IList<>)))
            {
                return ResolveList(rs.GetType());
            }

            return rs;
        }

        private object ResolveSingleton(Type t)
        {
            object rs;
            if (singletonInstance.TryGetValue(t, out rs))
            {
                if (rs.GetType().IsGenericType && (rs.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) || rs.GetType().GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    return ResolveList(rs.GetType());
                }

                return rs;
            }

            TypeCache tmp;
            singletonImpl.TryGetValue(t, out tmp);
            List<object> parms = new List<object>();
            foreach (var p in tmp.CtorParms)
            {
                parms.Add(ResolveType(p.ParameterType));
            }

            rs = Activator.CreateInstance(tmp.TImpl, parms.ToArray());
            if (!singletonInstance.TryAdd(t, rs))
            {
                singletonInstance.TryGetValue(t, out rs);
            }

            return rs;
        }

        private object ResolveTransient(Type t)
        {
            TypeCache tmp;
            transientImpl.TryGetValue(t, out tmp);
            List<object> parms = new List<object>();
            foreach (var p in tmp.CtorParms)
            {
                parms.Add(ResolveType(p.ParameterType));
            }

            return Activator.CreateInstance(tmp.TImpl, parms.ToArray());
        }

        private void AddBindingType(Type t, BindingType bt)
        {
            if (!bindingType.TryAdd(t, bt))
            {
                throw new Exception(string.Format("There is already a definition for type {0}", t.FullName));
            }
        }
        
        #endregion
    }
}
