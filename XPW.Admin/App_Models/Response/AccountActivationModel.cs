using XPW.CommonData.XPWAdmin.Entities;

namespace XPW.Admin.App_Models.Response {
     public class AccountActivationModel {
          public string Username { get; set; }
          public bool IsValidated { get; set; }
          public string Message { get; set; }
     }
}