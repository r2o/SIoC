namespace WebApiTestIIS.Controllers
{
    using Models;
    using System.Web.Http;

    [RoutePrefix("api/helloworld")]
    public class HelloWorldController : ApiController
    {
        private readonly IHelloWorld hello;

        public HelloWorldController(IHelloWorld hello)
        {
            this.hello = hello;
        }

        [Route("saysomething"), HttpGet]
        public IHttpActionResult SaySomething()
        {
            return Content(System.Net.HttpStatusCode.OK, hello.SayHello());
        }
    }
}