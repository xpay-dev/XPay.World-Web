using System.Web.Http;
using XPW.Utilities.IPWhiteListing;

namespace XPW.TestWeb.Controllers {
     [RoutePrefix("{token}/test")]
     public class TestController : ApiController {

          [Route("ip")]
          [HttpGet]
          [IPAuthorization]
          public string IP() {
              return "access";
          }

          [Route("token")]
          [HttpGet]
          public object Token() {
               return RequestContext.RouteData.Values["token"].ToString();
          }
     }
}
