using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace SIoC.core.Impl
{
    public class SIoCResRootImpl : IIoCResolutionRoot, IIoCBindingRoot
    {
        internal HashSet<Type> _singletonTypes;
        internal HashSet<Type> _constantTypes;
        internal HashSet<Type> _transitentTypes;

        internal ConcurrentDictionary<Type, object> _singletonInstance;
        internal ConcurrentDictionary<Type, object> _constantInstance;

        internal ConcurrentDictionary<Type, TypeCache> _singletonImpl;
        internal ConcurrentDictionary<Type, TypeCache> _constantImpl;
        internal ConcurrentDictionary<Type, TypeCache> _transientImpl;
        internal ConcurrentDictionary<Type, Func<object>> _methodImpl;

        internal ConcurrentDictionary<Type, BindingType> _bindingType;
        ConcurrentDictionary<Type, object> _array;

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
            _singletonTypes = new HashSet<Type>();
            _constantTypes = new HashSet<Type>();
            _transitentTypes = new HashSet<Type>();
            _singletonInstance = new ConcurrentDictionary<Type, object>();
            _constantInstance = new ConcurrentDictionary<Type, object>();
            _singletonImpl = new ConcurrentDictionary<Type, TypeCache>();
            _constantImpl = new ConcurrentDictionary<Type, TypeCache>();
            _transientImpl = new ConcurrentDictionary<Type, TypeCache>();
            _methodImpl = new ConcurrentDictionary<Type, Func<object>>();
            _bindingType = new ConcurrentDictionary<Type, BindingType>();
            _array = new ConcurrentDictionary<Type, object>();
        }

        void AddBindingType(Type t, BindingType bt)
        {
            if (!_bindingType.TryAdd(t, bt))
                throw new Exception(string.Format("There is already a definition for type {0}", t.FullName));
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
            if (!_singletonTypes.Add(tInt))
                throw new Exception(string.Format("There is already a definition for type {0}", tInt.FullName));
            var ctor = tImpl.GetConstructors().FirstOrDefault();
            var cache = new TypeCache();
            cache.TImpl = tImpl;
            cache.CtorParms = ctor.GetParameters();
            _singletonImpl.TryAdd(tInt, cache);
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

            if (!_transitentTypes.Add(tInt))
                throw new Exception(string.Format("There is already a definition for type {0}", tInt.FullName));

            var ctor = tImpl.GetConstructors().FirstOrDefault();
            var cache = new TypeCache();
            cache.TImpl = tImpl;
            cache.CtorParms = ctor.GetParameters();
            _transientImpl.TryAdd(tInt, cache);
        }

        public void BindToMethod<TInterface>(Func<TInterface> func)
        {
            Func<object> method = func as Func<object>;
            BindToMethod(typeof(TInterface), method);
        }

        public void BindToMethod(Type tint, Func<object> obj)
        {
            AddBindingType(tint, BindingType.Method);

            if (!_methodImpl.TryAdd(tint, obj))
                throw new Exception(string.Format("There is already a definition for type {0}", tint.FullName));
        }

        public void BindToConstant<TInterface>(TInterface obj)
        {
            BindToConstant(typeof(TInterface), obj);
        }

        public void BindToConstant(Type tint, object obj)
        {
            AddBindingType(tint, BindingType.Constant);
            if (!_constantTypes.Add(tint))
                throw new Exception(string.Format("There is already a definition for type {0}", tint.FullName));

            _constantInstance.TryAdd(tint, obj);
        }

        public bool HasBindingFor<T>()
        {
            return HasBindingFor(typeof(T));
        }

        public bool HasBindingFor(Type tint)
        {
            return _bindingType.ContainsKey(tint);
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

        object ResolveType(Type t)
        {
            BindingType bt;
            _bindingType.TryGetValue(t, out bt);
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

        object ResolveList(Type t)
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
                var irs = ((IEnumerable)rs);
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

        object ResolveMethod(Type t)
        {
            Func<object> tmp;
            _methodImpl.TryGetValue(t, out tmp);
            var obj = tmp();
            if (obj.GetType().IsGenericType && (obj.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) || obj.GetType().GetGenericTypeDefinition() == typeof(IList<>)))
                return ResolveList(obj.GetType());
            return tmp;
        }

        object ResolveConstant(Type t)
        {
            object rs;
            _constantInstance.TryGetValue(t, out rs);
            if (rs.GetType().IsGenericType && (rs.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) || rs.GetType().GetGenericTypeDefinition() == typeof(IList<>)))
                return ResolveList(rs.GetType());
            return rs;
        }

        object ResolveSingleton(Type t)
        {
            object rs;
            if (_singletonInstance.TryGetValue(t, out rs))
            {
                if (rs.GetType().IsGenericType && (rs.GetType().GetGenericTypeDefinition() == typeof(IEnumerable<>) || rs.GetType().GetGenericTypeDefinition() == typeof(IList<>)))
                    return ResolveList(rs.GetType());
                return rs;
            }

            TypeCache tmp;
            _singletonImpl.TryGetValue(t, out tmp);
            List<object> parms = new List<object>();
            foreach (var p in tmp.CtorParms)
            {
                parms.Add(ResolveType(p.ParameterType));
            }

            rs = Activator.CreateInstance(tmp.TImpl, parms.ToArray());
            if (!_singletonInstance.TryAdd(t, rs))
                _singletonInstance.TryGetValue(t, out rs);
            return rs;
        }

        object ResolveTransient(Type t)
        {
            TypeCache tmp;
            _transientImpl.TryGetValue(t, out tmp);
            List<object> parms = new List<object>();
            foreach (var p in tmp.CtorParms)
                parms.Add(ResolveType(p.ParameterType));

            return Activator.CreateInstance(tmp.TImpl, parms.ToArray());
        }
        #endregion
    }
}
