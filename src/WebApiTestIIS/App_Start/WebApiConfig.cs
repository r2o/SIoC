namespace WebApiTestIIS
{
    using SIoC.core;
    using System.Web.Http;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            var rr = Container.Get<IIoCResolutionRoot>();
            var container = new SIoC.WebApi.SIoCDependenyResolver(rr);
            container.RegisterApiControllers();
            GlobalConfiguration.Configuration.DependencyResolver = container;
        }
    }
}
