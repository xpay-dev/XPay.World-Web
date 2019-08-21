using XPW.CommonData.XPWAdmin.Entities;

namespace XPW.Admin.App_Models.Response {
     public class AccountActivationModel {
          public Account Account { get; set; }
          public bool IsValidated { get; set; }
          public string Message { get; set; }
     }
}