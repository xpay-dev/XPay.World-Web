using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.BaseContext;
using XPW.Utilities.Enums;

namespace XPW.Admin.Configurations.Entities {
     public class Account : BaseModelGuid {
          public Account() {
               IsVerified          = false;
               Tag                 = Status.Inactive;
               IsDeleted           = false;
          }
          [Required]
          [StringLength(20)]
          public string Username { get; set; }
          [Required]
          [StringLength(60)]
          public string EmailAddress { get; set; }
          [Required]
          public string Password { get; set; }
          [Required]
          public int AccountInformationId { get; set; }
          [ForeignKey("AccountInformationId")]
          public virtual AccountInformation AccountInformation { get; set; }
          [Required]
          public int RoleId { get; set; }
          [ForeignKey("RoleId")]
          public virtual Role Role { get; set; }
          public bool IsVerified { get; set; }
          public bool IsDeleted { get; set; }
          public Status Tag { get; set; }
     }
}