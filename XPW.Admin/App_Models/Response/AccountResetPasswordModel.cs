namespace XPW.Admin.App_Models.Response {
     public class AccountResetPasswordModel {
          public string Username { get; set; }
          public bool IsChange { get; set; }
          public bool IsSend { get; set; }
          public string Message { get; set; }
     }
}