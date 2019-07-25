using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using XPW.Admin.Configurations.Base;
using XPW.Admin.Configurations.Entities;
using XPW.Admin.Configurations.Services;
using XPW.CommonData.Entities;
using XPW.CommonData.Services;

namespace XPW.Admin.Controllers {
     [RoutePrefix("test")]
     public class TestController : BaseController {
          [Route("get-all")]
          [HttpGet]
          public Tuple<List<Account>, List<PasswordHistory>> GetAll() {
               var accounts = new AccountService().GetAll().ToList();
               var histories = new PasswordHistoryService().GetAll().ToList();
               return new Tuple<List<Account>, List<PasswordHistory>>(accounts, histories);
          }
          [Route("{token}/token")]
          [HttpGet]
          public string Token() {
               return RequestContext.RouteData.Values["token"].ToString();
          }
     }
}
