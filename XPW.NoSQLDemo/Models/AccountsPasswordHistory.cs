using System;
using System.ComponentModel.DataAnnotations;
using XPW.Utilities.BaseContext;
using XPW.Utilities.Enums;

namespace XPW.NoSQLDemo.Models {
     public class AccountsPasswordHistory : BaseModelGuid {
          public AccountsPasswordHistory() {
               Tag = Status.Active;
          }
          [Required]
          public Guid AccountId { get; set; }
          [Required]
          public string Password { get; set; }
          public Status Tag { get; set; }
     }
}