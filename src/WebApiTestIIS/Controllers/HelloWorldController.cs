namespace WebApiTestIIS.Controllers
{
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;

    [RoutePrefix("api/helloworld")]
    public class HelloWorldController : ApiController
    {
        readonly IHelloWorld _hello;

        public HelloWorldController(IHelloWorld hello)
        {
            _hello = hello;
        }

        [Route("saysomething"), HttpGet]
        public IHttpActionResult SaySomething()
        {
            return Content(System.Net.HttpStatusCode.OK, _hello.SayHello());
        }
    }
}