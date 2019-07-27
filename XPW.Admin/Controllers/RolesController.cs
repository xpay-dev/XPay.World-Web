using System.Web.Http;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.Utilities.BaseContextManagement;

namespace XPW.Admin.Controllers {
     [RoutePrefix("roles")]
     public class RolesController : BaseServiceController<Role, XPWAdminContext> {
          [Route("test")]
          [HttpGet]
          public string Test() {
               return "test";
          }
     }
}
