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
     [RoutePrefix("roles")]
     [Authorization]
     [DatabaseConnectionValidation("XPWAdmin")]
     public class RolesController : BaseServiceController<Role, XPWAdminContext> {
          [Route("save")]
          [HttpPost]
          [RequestFiltering]
          public async Task<GenericResponseModel<RoleModel>> Save([FromBody]RoleModel viewModel) {
               return await Task.Run(async () => {
                    Role role = new Role();
                    ErrorCode = "800.4";
                    try {
                         var roles = Service.GetAll().ToList();
                         if (roles != null) {
                              if (roles.Count > 0) {
                                   if (roles.Where(a => a.Name.ToLower() == viewModel.Name.ToLower()).FirstOrDefault() != null) {
                                        ErrorCode = "800.41";
                                        throw new Exception(viewModel.Name + " was already exist!");
                                   }
                              }
                         }
                         role = new Role { Name = viewModel.Name, Order = viewModel.Order };
                         role = await Service.SaveReturnAsync(role);
                         viewModel.Id             = role.Id;
                         viewModel.DateCreated    = role.DateCreated;
                    } catch (Exception ex) {
                         string message = ex.Message + (ex.InnerException != null && !string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage        = message;
                         MethodBase m        = MethodBase.GetCurrentMethod();
                         StackTrace trace    = new StackTrace(ex, true);
                         string sourceFile   = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
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
                    return new GenericResponseModel<RoleModel>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? viewModel : null,
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
          public async Task<GenericResponseModel<RoleModel>> Update([FromUri]int id, [FromBody]RoleModel viewModel) {
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
                         if (!viewModel.Id.HasValue) {
                              ErrorCode = "800.52";
                              throw new Exception("Invalid data reference. Data didn't match.");
                         }
                         if (id != viewModel.Id) {
                              ErrorCode = "800.53";
                              throw new Exception("Invalid data reference. Data didn't match.");
                         }
                         var oldModel = await Service.Get(id);
                         if (oldModel == null) {
                              ErrorCode = "800.54";
                              throw new Exception("Invalid data reference. No data found.");
                         }
                         var roles = Service.GetAll().Where(a => a.Id != viewModel.Id.Value).ToList();
                         if (roles != null) {
                              if (roles.Count > 0) {
                                   role = roles.Where(a => a.Name.ToLower() == viewModel.Name.ToLower()).FirstOrDefault();
                                   if (role != null) {
                                        ErrorCode = "800.55";
                                        throw new Exception(viewModel.Name + " was already exist!");
                                   }
                              }
                         }
                         oldModel.Name            = viewModel.Name;
                         oldModel.Order           = viewModel.Order;
                         oldModel.DateUpdated     = DateTime.Now;
                         role                     = await Service.UpdateReturnAsync(oldModel);
                         viewModel.DateCreated    = role.DateCreated;
                         viewModel.DateUpdated    = role.DateUpdated;
                         viewModel.Id             = id;
                    } catch (Exception ex) {
                         string message = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage        = message;
                         MethodBase m        = MethodBase.GetCurrentMethod();
                         StackTrace trace    = new StackTrace(ex, true);
                         string sourceFile   = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
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
                    return new GenericResponseModel<RoleModel>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? viewModel : null,
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
