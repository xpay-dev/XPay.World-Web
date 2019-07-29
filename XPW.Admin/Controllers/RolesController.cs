using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Web.Http;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.Utilities.BaseContextManagement;
using XPW.Utilities.Filtering;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Admin.Controllers {
     [RoutePrefix("roles")]
     public class RolesController : BaseServiceController<Role, XPWAdminContext> {
          private static readonly bool SaveLog = Convert.ToBoolean(ConfigurationManager.AppSettings["SaveRevision"].ToString());
          [Route("save")]
          [HttpPost]
          [RequestFiltering]
          public async Task<GenericResponseModel<Role>> Save([FromBody]Role apiModel) {
               return await Task.Run(() => {
                    try {
                         apiModel = Service.SaveReturn(apiModel);
                         var fileName = new XPWAdminContext().GetType().Name + "-" + new Role().GetType().Name + "-" + apiModel.Id.ToString() + ".json";
                         if (SaveLog) {
                              List<RevisionLog<Role>> revisions = new List<RevisionLog<Role>>();
                              revisions.Add(new RevisionLog<Role> {
                                   Context        = new XPWAdminContext().GetType().Name,
                                   Entity         = new Role().GetType().Name,
                                   Revisions      = apiModel,
                                   RevisionType   = Utilities.Enums.RevisionType.Create
                              });
                              RevisionLogs<Role>.Write(revisions, fileName);                      
                         }
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
               var fileName = new XPWAdminContext().GetType().Name + "-" + new Role().GetType().Name + "-" + apiModel.Id.ToString() + ".json";
               return await Task.Run(async () => {
                    try {
#pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                         if (id == null) {
#pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                              throw new Exception("Invalid data reference");
                         }
                         if (id == 0) {
                              throw new Exception("Invalid data reference");
                         }
                         apiModel.DateUpdated = DateTime.Now;
                         apiModel = Service.UpdateReturn(apiModel);
                         if (SaveLog) {
                              List<RevisionLog<Role>> revisions = RevisionLogs<Role>.Read(fileName);
                              revisions.Add(new RevisionLog<Role> {
                                   Context        = new XPWAdminContext().GetType().Name,
                                   Entity         = new Role().GetType().Name,
                                   Revisions      = apiModel,
                                   RevisionType   = Utilities.Enums.RevisionType.Update
                              });
                              RevisionLogs<Role>.Write(revisions, fileName);                      
                         }
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
     }
}
