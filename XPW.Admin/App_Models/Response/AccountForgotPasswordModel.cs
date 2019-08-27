namespace XPW.Admin.App_Models.Response {
     public class AccountForgotPasswordModel {
          public string Username { get; set; }
          public bool IsSend { get; set; }
          public string Token { get; set; }
          public string Message { get; set; }
     }
     public class AccountForgotPasswordValidationModel { 
          public string Username { get; set; }
          public bool IsValidated { get; set; }
          public string Message { get; set; }
     }
     public class AccountForgotChangePasswordModel {
          public string Username { get; set; }
          public bool IsChange { get; set; }
          public bool IsSend { get; set; }
          public string Message { get; set; }
     }
}