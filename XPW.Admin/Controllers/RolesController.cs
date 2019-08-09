using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.Admin.App_Models.Request;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.Utilities.BaseContextManagement;
using XPW.Utilities.DatabaseValidation;
using XPW.Utilities.Filtering;
using XPW.Utilities.HeaderValidations;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Admin.Controllers {
     [Serializable]
     [RoutePrefix("roles")]
     [Authorization]
     [DatabaseConnectionValidation("XPWAdmin")]
     public class RolesController : BaseServiceController<Role, XPWAdminContext> {
          [Route("save")]
          [HttpPost]
          [RequestFiltering]
          public async Task<GenericResponseModel<Role>> Save([FromBody]RoleModel apiModel) {
               return await Task.Run(async () => {
                    Role role = new Role();
                    ErrorCode = "800.4";
                    try {
                         var roles = Service.GetAll().ToList();
                         if (roles.Where(a => a.Name.ToLower() == apiModel.Name.ToLower()).FirstOrDefault() != null) {
                              ErrorCode = "800.41";
                              throw new Exception(apiModel.Name + " was already exist!");
                         }
                         role = await Service.SaveReturnAsync(new Role { Name = apiModel.Name, Order = apiModel.Order });
                    } catch (Exception ex) {
                         string message = ex.Message + (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage        = message;
                         MethodBase m        = MethodBase.GetCurrentMethod();
                         StackTrace trace    = new StackTrace(ex, true);
                         string sourceFile   = trace.GetFrame(0).GetFileName();
                         ErrorLogs.Write(new ErrorLogsModel {
                              Application    = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller     = GetType().Name,
                              CurrentAction  = m.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode      = ErrorCode,
                              Message        = message,
                              SourceFile     = sourceFile,
                              LineNumber     = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace     = ex.ToString(),
                              Method         = m.Name.Split('>')[0].TrimStart('<')
                         }, ex);
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
               return await Task.Run(async () => {
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
                         role                = await Service.UpdateReturnAsync(role);
                    } catch (Exception ex) {
                         string message = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage        = message;
                         MethodBase m        = MethodBase.GetCurrentMethod();
                         StackTrace trace    = new StackTrace(ex, true);
                         string sourceFile   = trace.GetFrame(0).GetFileName();
                         ErrorLogs.Write(new ErrorLogsModel {
                              Application    = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller     = GetType().Name,
                              CurrentAction  = m.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode      = ErrorCode,
                              Message        = message,
                              SourceFile     = sourceFile,
                              LineNumber     = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace     = ex.ToString(),
                              Method         = m.Name.Split('>')[0].TrimStart('<')
                         }, ex);
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
