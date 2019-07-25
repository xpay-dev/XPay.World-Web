using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using XPW.Admin.Configurations.Base;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.CommonData.XPWAdmin.ervices;
using XPW.CommonData.XPWExternal.Entities;
using XPW.CommonData.XPWExternal.Services;

namespace XPW.Admin.Controllers {
     [RoutePrefix("test")]
     public class TestController : BaseController {
          [Route("{token}/token")]
          [HttpGet]
          public string Token() {
               return RequestContext.RouteData.Values["token"].ToString();
          }
          [Route("get-all")]
          [HttpGet]
          public Tuple<List<Account>, List<PasswordHistory>> GetAll() {
               return new Tuple<List<Account>, List<PasswordHistory>>(new AccountService().GetAll().ToList(), new PasswordHistoryService().GetAll().ToList());
          }
     }
}
