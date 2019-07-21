using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.BaseContext;
using XPW.Utilities.Enums;

namespace XPW.NoSQLDemo.Models {
     public class XPayAccount : BaseModelGuid {
          public XPayAccount() {
               IsVerified = false;
               Tag        = Status.Inactive;
               IsDeleted  = false;
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
          public Guid XPayContactInformationId { get; set; }
          [ForeignKey("XPayContactInformationId")]
          public virtual XPayContactInformation XPayContactInformation { get; set; }
          [Required]
          public Guid XPayRoleId { get; set; }
          [ForeignKey("XPayRoleId")]
          public virtual XPayRole XPayRole { get; set; }
          public bool IsVerified { get; set; }
          public bool IsDeleted { get; set; }
          public Status Tag { get; set; }
     }
}