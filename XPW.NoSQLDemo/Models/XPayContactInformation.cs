using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class XPayContactInformation : BaseModelGuid {
          [Required]
          [StringLength(20)]
          public string FirstName { get; set; }
          [Required]
          [StringLength(20)]
          public string MiddleName { get; set; }
          [Required]
          [StringLength(20)]
          public string LastName { get; set; }
          [Required]
          [Column(TypeName = "date")]
          public DateTime Birthday { get; set; }
          [Required]
          [StringLength(30)]
          public string MobileNumber { get; set; }
     }
}