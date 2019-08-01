using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Admin.App_Models.Request;
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
          public async Task<GenericResponseModel<Role>> Save([FromBody]RoleModel apiModel) {
               return await Task.Run(() => {
                    Role role = new Role();
                    ErrorCode = "800.4";
                    try {
                         var roles = Service.GetAll().ToList();
                         if (roles.Where(a => a.Name.ToLower() == apiModel.Name.ToLower()).FirstOrDefault() != null) {
                              ErrorCode = "800.41";
                              throw new Exception(apiModel.Name + " was already exist!");
                         }
                         role = Service.SaveReturn(new Role { Name = apiModel.Name, Order = apiModel.Order });
                    } catch (Exception ex) {
                         ErrorMessage = ex.Message;
                         ErrorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<Role>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? role : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("update/{id}")]
          [HttpPut]
          [RequestFiltering]
          public async Task<GenericResponseModel<Role>> Update([FromUri]int id, [FromBody]RoleModel apiModel) {
               return await Task.Run(() => {
                    Role role = new Role();
                    ErrorCode = "800.5";
                    try {
                         #pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                         if (id == null) {
                         #pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                              ErrorCode = "800.51";
                              throw new Exception("Invalid data reference.");
                         }
                         if (id == 0) {
                              ErrorCode = "800.51";
                              throw new Exception("Invalid data reference.");
                         }
                         if (id != apiModel.Id) {
                              ErrorCode = "800.52";
                              throw new Exception("Invalid data reference. Data didn't match.");
                         }
                         var oldModel = Service.Get(id);
                         if (oldModel == null) {
                              ErrorCode = "800.53";
                              throw new Exception("Invalid data reference. No data found.");
                         }
                         var roles = Service.GetAll().Where(a => a.Id != apiModel.Id).ToList();
                         role      = roles.Where(a => a.Name.ToLower() == apiModel.Name.ToLower()).FirstOrDefault();
                         if (role != null) {
                              ErrorCode = "800.54";
                              throw new Exception(apiModel.Name + " was already exist!");
                         }
                         role.Name           = apiModel.Name;
                         role.Order          = apiModel.Order;
                         role.DateUpdated    = DateTime.Now;
                         role                = Service.UpdateReturn(role);
                    } catch (Exception ex) {
                         ErrorMessage = ex.Message;
                         ErrorDetails.Add(ex.Message);
                    }
                    return new GenericResponseModel<Role>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? role : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
     }
}
