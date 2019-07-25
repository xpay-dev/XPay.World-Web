using System;
using System.ComponentModel.DataAnnotations;
using XPW.Utilities.BaseContext;
using XPW.Utilities.Enums;

namespace XPW.CommonData.XPWExternal.Entities {
     public class PasswordHistory : BaseModelInt {
          [Required]
          public Guid AccountId { get; set; }
          [Required]
          public string Password { get; set; }
          [Required]
          public AccountSource Source { get; set; }
          [Required]
          public Status Status { get; set; }
     }
}
