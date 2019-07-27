using System.Web.Http;
using XPW.Admin.Configurations.Base;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;

namespace XPW.Admin.Controllers {
     [RoutePrefix("test")]
     public class TestController : ServiceController<Role, XPWAdminContext> {
          [Route("test-action")]
          [HttpGet]
          public string Test() {
               return "test";
          }
     }
}
