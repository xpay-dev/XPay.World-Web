using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Admin.Configurations.Base;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.CommonData.XPWAdmin.ervices;
using XPW.Utilities.UtilityModels;

namespace XPW.Admin.Controllers {
     [RoutePrefix("accounts")]
     public class AccountsController : BaseController {
          internal static readonly AccountService AccountService = new AccountService();
          internal string errorMessage = string.Empty;
          internal List<string> errorDetails = new List<string>();
          [Route("get-all")]
          [HttpGet]
          public async Task<GenericResponseListModel<Account>> GetAll() {
               return await Task.Run(() => {
                    var roles = new List<Account>();
                    try {
                         roles = AccountService.GetAll().ToList();
                    } catch (Exception ex) {
                         errorMessage = ex.Message;
                         errorDetails.Add(ex.Message);
                    }
                    return new GenericResponseListModel<Account>() {
                         Code = string.IsNullOrEmpty(errorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus = string.IsNullOrEmpty(errorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject = string.IsNullOrEmpty(errorMessage) ? roles : null,
                         ErrorMessage = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                              Details = errorDetails,
                              ErrNumber = "800",
                              Message = errorMessage
                         }
                    };
               });
          }
     }
}
