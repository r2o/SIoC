namespace SIoC.NancyFx
{
    using Nancy;
    using Nancy.Bootstrapper;
    using Nancy.Conventions;
    using Nancy.Diagnostics;
    using SIoC.core;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;

    public class SIoCNancyBootstrapper : NancyBootstrapperWithRequestContainerBase<IIoCResolutionRoot>
    {
        static IIoCBindingRoot _br;

        static SIoCNancyBootstrapper()
        {
            _br = Container.Get<IIoCBindingRoot>();
        }

        protected override IIoCResolutionRoot CreateRequestContainer(NancyContext context)
        {
            return Container.Get<IIoCResolutionRoot>();
        }

        protected override IEnumerable<INancyModule> GetAllModules(IIoCResolutionRoot container)
        {
            var obj = container.Get(typeof(INancyModule));
            var enu = (IEnumerable)obj;
            List<INancyModule> result = new List<INancyModule>();
            var enumerator = enu.GetEnumerator();
            while (enumerator.MoveNext())
            {
                var tmp = ApplicationContainer.Get(((ModuleRegistration)enumerator.Current).ModuleType);
                result.Add((INancyModule)tmp);
            }
            return result;
        }

        protected override INancyModule GetModule(IIoCResolutionRoot container, Type moduleType)
        {
            if (!(_br.HasBindingFor(moduleType)))
                _br.BindInTransient(moduleType, moduleType);
            return (INancyModule)container.Get(moduleType);
        }

        protected override void RegisterRequestContainerModules(IIoCResolutionRoot container, IEnumerable<ModuleRegistration> moduleRegistrationTypes)
        {
            Func<object> func = () => moduleRegistrationTypes;
            if (!_br.HasBindingFor<INancyModule>())
            {
                _br.BindToMethod(typeof(INancyModule), func);
                foreach (var m in moduleRegistrationTypes)
                {
                    _br.BindInTransient(m.ModuleType, m.ModuleType);
                }
            }
        }

        protected override void ConfigureConventions(NancyConventions nancyConventions)
        {
            if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["StaticContent"]))
                nancyConventions.StaticContentsConventions.Add(StaticContentConventionBuilder.AddDirectory(ConfigurationManager.AppSettings["StaticContent"]));
            base.ConfigureConventions(nancyConventions);
        }

        protected override IIoCResolutionRoot GetApplicationContainer()
        {
            return Container.Get<IIoCResolutionRoot>();
        }

        protected override IEnumerable<IApplicationStartup> GetApplicationStartupTasks()
        {
            var tps = ApplicationContainer.Get(typeof(IApplicationStartup));
            if (tps is System.Collections.IEnumerable)
            {
                var lst = (IEnumerable)tps;
                return lst.OfType<Type>().Select(f => (IApplicationStartup)ApplicationContainer.Get(f));
            }

            return default(IEnumerable<IApplicationStartup>);
        }

        public class CustomRootPathProvider : IRootPathProvider
        {
            public string GetRootPath()
            {
                return Environment.CurrentDirectory;
            }
        }
        protected override IRootPathProvider RootPathProvider
        {
            get { return new CustomRootPathProvider(); }
        }

        protected override DiagnosticsConfiguration DiagnosticsConfiguration
        {
            get { return new DiagnosticsConfiguration { Password = @"A2\6mVtH/XRT\p,B" }; }
        }

        protected override IDiagnostics GetDiagnostics()
        {
            return ApplicationContainer.Get<IDiagnostics>();
        }

        protected override INancyEngine GetEngineInternal()
        {
            return this.ApplicationContainer.Get<INancyEngine>();
        }

        protected override IEnumerable<IRegistrations> GetRegistrationTasks()
        {
            var tps = ApplicationContainer.Get(typeof(IRegistrations));
            if (tps is System.Collections.IEnumerable)
            {
                var lst = (IEnumerable)tps;
                return lst.OfType<Type>().Select(f => (IRegistrations)ApplicationContainer.Get(f));
            }
            return default(IEnumerable<IRegistrations>);
        }

        protected override IEnumerable<IRequestStartup> RegisterAndGetRequestStartupTasks(IIoCResolutionRoot container, Type[] requestStartupTypes)
        {
            List<IRequestStartup> result = new List<IRequestStartup>();
            foreach (var t in requestStartupTypes)
            {
                if (_br.HasBindingFor(t))
                    _br.BindInTransient(t, t);
                result.Add((IRequestStartup)container.Get(t));
            }
            return result;
        }

        protected override void RegisterBootstrapperTypes(IIoCResolutionRoot applicationContainer)
        {
            _br.BindToConstant<INancyModuleCatalog>(this);
        }

        protected override void RegisterCollectionTypes(IIoCResolutionRoot container, IEnumerable<CollectionTypeRegistration> collectionTypeRegistrationsn)
        {
            foreach (var cr in collectionTypeRegistrationsn)
            {
                switch (cr.Lifetime)
                {
                    case Lifetime.Singleton:
                        _br.BindToConstant(cr.RegistrationType, cr.ImplementationTypes);
                        foreach (var i in cr.ImplementationTypes)
                            _br.BindInSingleton(i, i);
                        break;
                    case Lifetime.Transient:
                        Func<object> impls = () => cr.ImplementationTypes;
                        _br.BindToMethod(cr.RegistrationType, impls);
                        foreach (var i in cr.ImplementationTypes)
                            _br.BindToMethod(i, () => ApplicationContainer.Get(i));
                        break;
                }
            }
        }

        protected override void RegisterInstances(IIoCResolutionRoot container, IEnumerable<InstanceRegistration> instanceRegistrations)
        {
            foreach (var c in instanceRegistrations)
            {
                switch (c.Lifetime)
                {
                    case Lifetime.Transient:
                        Func<object> fc = () => c.Implementation;
                        _br.BindToMethod(c.RegistrationType, fc);
                        break;
                    case Lifetime.Singleton:
                        _br.BindToConstant(c.RegistrationType, c.Implementation);
                        break;
                    case Lifetime.PerRequest:
                        throw new InvalidOperationException("Unable to register a per request lifetime.");
                }
            }
        }

        protected override void RegisterTypes(IIoCResolutionRoot container, IEnumerable<TypeRegistration> typeRegistrations)
        {
            foreach (var t in typeRegistrations)
            {
                switch (t.Lifetime)
                {
                    case Lifetime.Transient:
                        _br.BindInTransient(t.RegistrationType, t.ImplementationType);
                        break;
                    case Lifetime.Singleton:
                        _br.BindInSingleton(t.RegistrationType, t.ImplementationType);
                        break;
                    case Lifetime.PerRequest:
                        throw new InvalidOperationException("Unable to register a per request lifetime.");
                }
            }
        }
    }
}
