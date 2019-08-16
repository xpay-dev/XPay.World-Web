using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using XPW.Admin.App_Models.Request;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.Utilities.AppConfigManagement;
using XPW.Utilities.BaseContextManagement;
using XPW.Utilities.DatabaseValidation;
using XPW.Utilities.Filtering;
using XPW.Utilities.Functions;
using XPW.Utilities.HeaderValidations;
using XPW.Utilities.Logs;
using XPW.Utilities.UtilityModels;

namespace XPW.Admin.Controllers {
     [RoutePrefix("accounts")]
     [Authorization]
     [DatabaseConnectionValidation("XPWAdmin")]
     public class AccountsController : BaseServiceController<Account, XPWAdminContext> {
          internal static readonly AppConfig appConfigManager = new AppConfig(HostingEnvironment.ApplicationPhysicalPath + "App_Settings", "appConfig.json");
          [Route("save")]
          [HttpPost]
          [RequestFiltering]
          public async Task<GenericResponseModel<AccountModel>> Save([FromBody]AccountModel viewModel) {
               return await Task.Run(async () => {
                    AccountInformation accountInfo     = new AccountInformation();
                    Account account                    = new Account();
                    bool passwordGenerate              = appConfigManager.AppSetting<bool>("PasswordAutoGenerator", true, new AppConfigSettingsModel { Value = "false", Group = "Admin" });
                    bool useDefaultPassword            = appConfigManager.AppSetting<bool>("UseDefaultPassword", true, new AppConfigSettingsModel { Value = "false", Group = "Admin" });
                    string defaultPassword             = appConfigManager.AppSetting<string>("DefaultPassword", true, new AppConfigSettingsModel { Value = "patCHES214#", Group = "Admin" });
                    ErrorCode = "800.4";
                    try {
                         var accounts = Service.GetAll().ToList();
                         if (accounts != null) {
                              if (accounts.Count > 0) {
                                   if (accounts.Where(a => a.Username.ToLower() == viewModel.Username.ToLower()).FirstOrDefault() != null) {
                                        ErrorCode = "800.41";
                                        throw new Exception(viewModel.Username + " was already exist!");
                                   }
                              }
                         }
                         if (!passwordGenerate) {
                              if (string.IsNullOrEmpty(viewModel.Password)) {
                                   ErrorCode = "800.42";
                                   throw new Exception("Password: cannot be null or empty");
                              }
                              if (viewModel.Password.Length < 8) {
                                   ErrorCode = "800.43";
                                   throw new Exception("Password: string length is invalid, must be 8 characters and up");
                              }
                              var textString = Regex.IsMatch(viewModel.Password.Trim(), @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[$_@=|{}+!#<>])[a-zA-Z0-9$_@=|{}+!#<>]{8,50}");
                              if (!textString) {
                                   ErrorCode = "800.44";
                                   throw new Exception("Password: must be have at least one (1) Upper case letter, one (1) lower case letter, a number and/or a special character");
                              }
                         }
                         accountInfo = new AccountInformation {
                              Address1       = viewModel.Address1,
                              Address2       = viewModel.Address2,
                              Birthday       = viewModel.Birthday,
                              City_Town      = viewModel.City_Town,
                              Country        = viewModel.Country,
                              FirstName      = viewModel.FirstName,
                              LastName       = viewModel.LastName,
                              MiddleName     = viewModel.MiddleName,
                              MobileNumber   = viewModel.MobileNumber,     
                              Province_State = viewModel.Province_State,
                              ZipCode        = viewModel.ZipCode
                         };
                         account = new Account {
                              AccountInformationId     = accountInfo.Id,
                              EmailAddress             = viewModel.EmailAddress,
                              RoleId                   = viewModel.RoleId,
                              Username                 = viewModel.Username,
                              Password                 = crypto.Encrypt((passwordGenerate ? (useDefaultPassword ? defaultPassword : Generator.StringGenerator(10)) : viewModel.Password)),
                              AccountInformation       = accountInfo,
                         };
                         account = await Service.SaveReturnAsync(account);
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
                    return new GenericResponseModel<AccountModel>() {
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
