using System;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.Utilities.BaseContextManagement;
using XPW.Utilities.Filtering;
using XPW.Utilities.UtilityModels;

namespace XPW.Admin.Controllers {
     [RoutePrefix("roles")]
     public class RolesController : BaseServiceController<Role, XPWAdminContext> {
          [Route("save")]
          [HttpPost]
          [RequestFiltering]
          public async Task<GenericResponseModel<Role>> Save(Role apiModel) {
               return await Task.Run(() => {
                    try {
                         apiModel = Service.SaveReturn(apiModel);
                    } catch (Exception ex) {
                         ErrorMessage = ex.Message;
                         ErrorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<Role>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? apiModel : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = "800.2",
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("update/{id}")]
          [HttpPut]
          [RequestFiltering]
          public async Task<GenericResponseModel<Role>> Update([FromUri]int id, [FromBody]Role apiModel) {
               return await Task.Run(() => {
                    try {
                         var name = new XPWAdminContext().GetType().Name;
                         apiModel.DateUpdated = DateTime.Now;
                         apiModel = Service.UpdateReturn(apiModel);
                    } catch (Exception ex) {
                         ErrorMessage = ex.Message;
                         ErrorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<Role>() {
                         Code = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject = string.IsNullOrEmpty(ErrorMessage) ? apiModel : null,
                         ErrorMessage = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details = ErrorDetails,
                              ErrNumber = "800.2",
                              Message = ErrorMessage
                         }
                    };
               });
          }
     }
}
