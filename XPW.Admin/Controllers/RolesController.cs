using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Admin.Configurations.Base;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.CommonData.XPWAdmin.Services;
using XPW.Utilities.UtilityModels;

namespace XPW.Admin.Controllers {
     [RoutePrefix("roles")]
     public class RolesController : BaseController {
          internal static readonly RoleService roleService = new RoleService();
          internal string errorMessage = string.Empty;
          internal List<string> errorDetails = new List<string>();
          [Route("get-all")]
          [HttpGet]
          public async Task<GenericResponseListModel<Role>> GetAll() {
               var roles = new List<Role>();
               try {
                    roles = roleService.GetAll().ToList();
               } catch (Exception ex) {
                    errorMessage = ex.Message;
                    errorDetails.Add(ex.Message);
               }
               return new GenericResponseListModel<Role>() {
                    Code                = Utilities.Enums.CodeStatus.Success,
                    CodeStatus          = Utilities.Enums.CodeStatus.Success.ToString(),
                    ReferenceObject     = string.IsNullOrEmpty(errorMessage) ? roles : null,
                    ErrorMessage        = string.IsNullOrEmpty(errorMessage) ? null : new ErrorMessage {
                                             Details   = errorDetails,
                                             ErrNumber = "800",
                                             Message   = errorMessage
                    }  
               };
          }
     }
}
