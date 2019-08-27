using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Hosting;
using System.Web.Http;
using XPW.Admin.App_Models.Request;
using XPW.Admin.App_Models.Response;
using XPW.CommonData.XPWAdmin.DataContext;
using XPW.CommonData.XPWAdmin.Entities;
using XPW.CommonData.XPWAdmin.ervices;
using XPW.CommonData.XPWAdmin.Services;
using XPW.Utilities.AppConfigManagement;
using XPW.Utilities.BaseContext;
using XPW.Utilities.BaseContextManagement;
using XPW.Utilities.CryptoHashingManagement;
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
          internal static readonly AccountService accountService = new AccountService();
          internal static readonly RoleService roleService = new RoleService();
          internal static readonly AccountInformationService accountInformationService = new AccountInformationService();
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
                         string encryptedPassword = crypto.Encrypt((passwordGenerate ? (useDefaultPassword ? defaultPassword : Generator.StringGenerator(10)) : viewModel.Password));
                         account = new Account {
                              AccountInformationId     = accountInfo.Id,
                              EmailAddress             = viewModel.EmailAddress,
                              RoleId                   = viewModel.RoleId,
                              Username                 = viewModel.Username,
                              Password                 = encryptedPassword,
                              AccountInformation       = accountInfo,
                         };
                         account        = await Service.SaveReturnAsync(account);
                         account.Role   = await roleService.Get(account.RoleId);               
                         await accountService.GenerateUserJsonData(account);
                         string token   = Checker.NumberExtractor(account.Id.ToString()) + "-" + account.AccountInformationId.ToString();
                         string url     = appConfigManager.AppSetting<string>("AdminActivationURL", true, new AppConfigSettingsModel { Value = "https:\\\\localhost:9909\\Admin\\Token\\Activate?token=", Group = "Admin" });
                         url            += token;
                         bool isSend    = await accountService.AccountEmail(account, "XPay.World Registration", url);
                         viewModel.Id                   = account.Id;
                         viewModel.AccountInformationId = account.AccountInformationId;
                         viewModel.Password             = string.Empty;
                         viewModel.Token                = token;
                         viewModel.TokenExpiry          = account.DateCreated.AddMinutes(70);
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
          [Route("update/{id}")]
          [HttpPut]
          [RequestFiltering]
          public async Task<GenericResponseModel<AccountUserModel>> Update([FromUri]Guid id, [FromBody]AccountUserModel viewModel) {
               return await Task.Run(async () => {
                    Account account          = new Account();
                    bool passwordGenerate    = appConfigManager.AppSetting<bool>("PasswordAutoGenerator", true, new AppConfigSettingsModel { Value = "false", Group = "Admin" });
                    bool useDefaultPassword  = appConfigManager.AppSetting<bool>("UseDefaultPassword", true, new AppConfigSettingsModel { Value = "false", Group = "Admin" });
                    string defaultPassword   = appConfigManager.AppSetting<string>("DefaultPassword", true, new AppConfigSettingsModel { Value = "patCHES214#", Group = "Admin" });
                    ErrorCode = "800.5";
                    try {
                         #pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                         if (id == null) {
                         #pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                              ErrorCode = "800.51";
                              throw new Exception("Invalid data reference.");
                         }
                         if (id == Guid.Empty) {
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
                         var accounts = Service.GetAll().Where(a => a.Id != viewModel.Id.Value).ToList();
                         if (accounts != null) {
                              if (accounts.Count > 0) {
                                   account = accounts.Where(a => a.Username.ToLower() == viewModel.Username.ToLower()).FirstOrDefault();
                                   if (account != null) {
                                        ErrorCode = "800.55";
                                        throw new Exception(viewModel.Username + " was already exist!");
                                   }
                              }
                         }
                         string decryptedPassword = crypto.Decrypt(oldModel.Password);
                         string encryptedPassword = string.Empty;
                         if (decryptedPassword != viewModel.Password) {
                              encryptedPassword = crypto.Encrypt((passwordGenerate ? (useDefaultPassword ? defaultPassword : Generator.StringGenerator(10)) : viewModel.Password));
                         } else {
                              encryptedPassword = oldModel.Password;
                         }
                         oldModel.AccountInformationId = oldModel.AccountInformationId;
                         oldModel.EmailAddress         = viewModel.EmailAddress;
                         oldModel.RoleId               = viewModel.RoleId;
                         oldModel.Username             = viewModel.Username;
                         oldModel.Password             = encryptedPassword;
                         oldModel.DateUpdated          = DateTime.Now;
                         account                       = await Service.UpdateReturnAsync(oldModel);
                         account.AccountInformation    = await accountInformationService.Get(oldModel.AccountInformationId);
                         account.Role                  = await roleService.Get(account.RoleId);
                         await accountService.GenerateUserJsonData(account);
                         viewModel.DateCreated         = account.DateCreated;
                         viewModel.DateUpdated         = account.DateUpdated;
                         viewModel.Id                  = id;
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
                    return new GenericResponseModel<AccountUserModel>() {
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
          [Route("update-information/{id}")]
          [HttpPut]
          [RequestFiltering]
          public async Task<GenericResponseModel<AccountUserInformation>> UpdateInformation([FromUri]int id, [FromBody]AccountUserInformation viewModel) {
               return await Task.Run(async () => {
                    AccountInformation accountInformation   = new AccountInformation();
                    bool passwordGenerate                   = appConfigManager.AppSetting<bool>("PasswordAutoGenerator", true, new AppConfigSettingsModel { Value = "false", Group = "Admin" });
                    bool useDefaultPassword                 = appConfigManager.AppSetting<bool>("UseDefaultPassword", true, new AppConfigSettingsModel { Value = "false", Group = "Admin" });
                    string defaultPassword                  = appConfigManager.AppSetting<string>("DefaultPassword", true, new AppConfigSettingsModel { Value = "patCHES214#", Group = "Admin" });
                    ErrorCode = "800.6";
                    try {
                         #pragma warning disable CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                         if (id == null) {
                         #pragma warning restore CS0472 // The result of the expression is always the same since a value of this type is never equal to 'null'
                              ErrorCode = "800.61";
                              throw new Exception("Invalid data reference.");
                         }
                         if (id == 0) {
                              ErrorCode = "800.61";
                              throw new Exception("Invalid data reference.");
                         }
                         if (!viewModel.Id.HasValue) {
                              ErrorCode = "800.62";
                              throw new Exception("Invalid data reference. Data didn't match.");
                         }
                         if (id != viewModel.Id) {
                              ErrorCode = "800.63";
                              throw new Exception("Invalid data reference. Data didn't match.");
                         }
                         var oldModel = await accountInformationService.Get(id);
                         if (oldModel == null) {
                              ErrorCode = "800.64";
                              throw new Exception("Invalid data reference. No data found.");
                         }
                         oldModel.Address1             = viewModel.Address1;
                         oldModel.Address2             = viewModel.Address2;
                         oldModel.Birthday             = viewModel.Birthday;
                         oldModel.City_Town            = viewModel.City_Town;
                         oldModel.Country              = viewModel.Country;
                         oldModel.FirstName            = viewModel.FirstName;
                         oldModel.LastName             = viewModel.LastName;
                         oldModel.MiddleName           = viewModel.MiddleName;
                         oldModel.MobileNumber         = viewModel.MobileNumber;
                         oldModel.Province_State       = viewModel.Province_State;
                         oldModel.ZipCode              = viewModel.ZipCode;
                         oldModel.DateUpdated          = DateTime.Now;
                         accountInformation            = await accountInformationService.UpdateReturnAsync(oldModel);
                         var account                   = Service.GetAll().Where(a=> a.AccountInformationId == id).FirstOrDefault(); ;
                         account.Role                  = await roleService.Get(account.RoleId);
                         account.AccountInformation    = oldModel;
                         await accountService.GenerateUserJsonData(account);
                         viewModel.DateCreated   = accountInformation.DateCreated;
                         viewModel.DateUpdated   = accountInformation.DateUpdated;
                         viewModel.Id            = id;
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
                    return new GenericResponseModel<AccountUserInformation>() {
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
          [Route("get-information/{id}")]
          [HttpGet]
          public virtual async Task<GenericResponseModel<AccountInformation>> GetInformation([FromUri]string id) {
               return await Task.Run(async () => {
                    var data = new AccountInformation();
                    try {
                         ErrorCode = "800.7";
                         if (!Guid.TryParse(id, out Guid guidId) && !int.TryParse(id, out int intId)) {
                              id = crypto.Decrypt((id.Contains(" ") ? id.Replace(" ", "+") : id));
                         }
                         string entityName = new AccountInformation().GetType().Name;
                         spParams.Add(new StoredProcedureParam {
                              Param = "TableName",
                              Value = Pluralized.Pluralize(new AccountInformation().GetType().Name)
                         });
                         spParams.Add(new StoredProcedureParam {
                              Param = "Id",
                              Value = id.ToString()
                         });
                         var dataType = string.Empty;
                         var isGuid = Guid.TryParse(id, out guidId);
                         var isNumeric = int.TryParse(id, out intId);
                         if (isGuid) {
                              dataType = "uniqueidentifier".ToUpper();
                         } else if (isNumeric) {
                              dataType = "int".ToUpper();
                         } else {
                              ErrorCode = "800.71";
                              throw new Exception("Invalid data reference.");
                         }
                         spParams.Add(new StoredProcedureParam {
                              Param = "Type",
                              Value = dataType
                         });
                         data = await accountInformationService.StoredProcedure("spGetAll", spParams);
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                    }
                    return new GenericResponseModel<AccountInformation>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("delete-information/{id}")]
          [HttpDelete]
          public virtual async Task<GenericResponseModel<AccountInformation>> DeleteInformation([FromUri]string id) {
               return await Task.Run(async () => {
                    var data = new AccountInformation();
                    try {
                         ErrorCode = "800.8";
                         if (!Guid.TryParse(id, out Guid guidId) && !int.TryParse(id, out int intId)) {
                              var crypto = new HashUtilityManagement(key, iv);
                              id = crypto.Decrypt((id.Contains(" ") ? id.Replace(" ", "+") : id));
                         }
                         var isGuid = Guid.TryParse(id, out guidId);
                         var isNumeric = int.TryParse(id, out intId);
                         if (isGuid) {
                              data = await accountInformationService.Get(guidId);
                              if (data == null) {
                                   ErrorCode = "800.81";
                                   throw new Exception("Invalid data reference.");
                              }
                              await Service.DeleteAsync(guidId);
                         } else if (isNumeric) {
                              data = await accountInformationService.Get(intId);
                              if (data == null) {
                                   ErrorCode = "800.81";
                                   throw new Exception("Invalid data reference.");
                              }
                              await Service.DeleteAsync(intId);
                         } else {
                              ErrorCode = "800.82";
                              throw new Exception("Invalid data reference.");
                         }
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                    }
                    return new GenericResponseModel<AccountInformation>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? data : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("account-confirmation/{token}")]
          [HttpPut]
          public async Task<GenericResponseModel<AccountActivationModel>> AccountConfirmation([FromUri]string token) {
               return await Task.Run(async () => {
                    var accountActivation = new AccountActivationModel();
                    try {
                         ErrorCode = "800.91";
                         var response = await accountService.ValidateActivationPasscodeToken(token);
                         response.Item1.Password = string.Empty;
                         accountActivation = new AccountActivationModel { Username = response.Item1.Username, IsValidated = response.Item2, Message = response.Item3 };
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                         accountActivation = new AccountActivationModel { Username = string.Empty, IsValidated = false, Message = message };
                    }
                    return new GenericResponseModel<AccountActivationModel>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? accountActivation : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("forgot-password/{userAccess}")]
          [HttpPut]
          public async Task<GenericResponseModel<AccountForgotPasswordModel>> ForgotPassword([FromUri]string userAccess) {
               return await Task.Run(async () => {
                    var details = new AccountForgotPasswordModel();
                    try {
                         ErrorCode      = "800.92";
                         var account    = await accountService.UpdateForgottenAccount(userAccess);
                         string token   = account.RoleId.ToString() + "-" + account.DateUpdated.Value.ToString("yyddMM") + "_" + Checker.NumberExtractor(account.Id.ToString()) + "-" + account.AccountInformationId.ToString();
                         string url     = appConfigManager.AppSetting<string>("AdminforgotPasswordURL", true, new AppConfigSettingsModel { Value = "https:\\\\localhost:9909\\Admin\\Token\\ForgotPassword?userAccess=", Group = "Admin" });
                         url            += token;
                         bool isSend    = await accountService.AccountEmail(account, "XPay.World Forgot Password", url);
                         details        = new AccountForgotPasswordModel { Username = account.Username, IsSend = isSend, Token = token, Message = "Success" };
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                         details = new AccountForgotPasswordModel { Username = string.Empty, IsSend = false, Token = string.Empty, Message = message };
                    }
                    return new GenericResponseModel<AccountForgotPasswordModel>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? details : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("forgot-password-validation/{token}")]
          [HttpGet]
          public async Task<GenericResponseModel<AccountForgotPasswordValidationModel>> ForgotPasswordValidation([FromUri]string token) {
               return await Task.Run(async () => {
                    var validateModel = new AccountForgotPasswordValidationModel();
                    try {
                         ErrorCode = "800.91";
                         var response = await accountService.ForgotPasswordTokenValidator(token);
                         response.Item1.Password = string.Empty;
                         validateModel = new AccountForgotPasswordValidationModel { Username = response.Item1.Username, IsValidated = response.Item2, Message = response.Item3 };
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                         validateModel = new AccountForgotPasswordValidationModel { Username = string.Empty, IsValidated = false, Message = message };
                    }
                    return new GenericResponseModel<AccountForgotPasswordValidationModel>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? validateModel : null,
                         ErrorMessage        = string.IsNullOrEmpty(ErrorMessage) ? null : new ErrorMessage {
                              Details        = ErrorDetails,
                              ErrNumber      = ErrorCode,
                              Message        = ErrorMessage
                         }
                    };
               });
          }
          [Route("forgot-password-change/{token}")]
          [HttpPut]
          public async Task<GenericResponseModel<AccountForgotChangePasswordModel>> ForgotPasswordChange([FromUri]string token, [FromBody]ForgotPassswordModel viewModel) {
               return await Task.Run(async () => {
                    var details = new AccountForgotChangePasswordModel();
                    try {
                         ErrorCode = "800.93";
                         if (token != viewModel.Token) {
                              ErrorCode = "800.931";
                              throw new Exception("Invalid token.");
                         }
                         var response = await accountService.ForgotPasswordTokenValidator(token);
                         if (!response.Item2) {
                              ErrorCode = "800.932";
                              throw new Exception(response.Item3);
                         }
                         Account account = response.Item1;
                         if (account.Username != viewModel.Username) { 
                              ErrorCode = "800.933";
                              throw new Exception("Invalid account or password!");
                         }
                         if (crypto.Decrypt(account.Password) != viewModel.CurrentPassword) {
                              ErrorCode = "800.934";
                              throw new Exception("Invalid account or password!");
                         }
                         if (viewModel.NewPassword == viewModel.CurrentPassword) {
                              ErrorCode = "800.935";
                              throw new Exception("New password is the same as the previous password.");
                         }
                         account.Password = crypto.Encrypt(viewModel.NewPassword);
                         account = await Service.UpdateReturnAsync(account);
                         details = new AccountForgotChangePasswordModel { IsSend = false, Message = "Updated", IsChange = true, Username = viewModel.Username };
                         bool isSend = await accountService.AccountForgotPasswordEmail(account, "XPay.World Forgot Password Successfully Updated");
                         details = new AccountForgotChangePasswordModel { IsSend = isSend, Message = "Success", IsChange = true, Username = viewModel.Username };
                    } catch (Exception ex) {
                         string message           = ex.Message + (!string.IsNullOrEmpty(ex.InnerException.Message) && ex.Message != ex.InnerException.Message ? " Reason : " + ex.InnerException.Message : string.Empty);
                         ErrorDetails.Add(message);
                         ErrorMessage             = message;
                         MethodBase methodBase    = MethodBase.GetCurrentMethod();
                         StackTrace trace         = new StackTrace(ex, true);
                         string sourceFile        = trace.GetFrame(0).GetFileName();
                         await ErrorLogs.Write(new ErrorLogsModel {
                              Application         = Assembly.GetExecutingAssembly().GetName().Name,
                              Controller          = GetType().Name,
                              CurrentAction       = methodBase.Name.Split('>')[0].TrimStart('<'),
                              ErrorCode           = ErrorCode,
                              Message             = message,
                              SourceFile          = sourceFile,
                              LineNumber          = trace.GetFrame(0).GetFileLineNumber(),
                              StackTrace          = ex.ToString(),
                              Method              = methodBase.Name.Split('>')[0].TrimStart('<')
                         }, ex);
                         details = new AccountForgotChangePasswordModel { IsSend = false, Message = message, IsChange = false, Username = viewModel.Username };
                    }
                    return new GenericResponseModel<AccountForgotChangePasswordModel>() {
                         Code                = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success : Utilities.Enums.CodeStatus.Error,
                         CodeStatus          = string.IsNullOrEmpty(ErrorMessage) ? Utilities.Enums.CodeStatus.Success.ToString() : Utilities.Enums.CodeStatus.Error.ToString(),
                         ReferenceObject     = string.IsNullOrEmpty(ErrorMessage) ? details : null,
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
