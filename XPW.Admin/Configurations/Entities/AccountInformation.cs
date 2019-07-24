using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.BaseContext;

namespace XPW.Admin.Configurations.Entities {
     public class AccountInformation : BaseModelInt {
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
          [Required]
          [StringLength(100)]
          public string Address1 { get; set; }
          [Required]
          [StringLength(100)]
          public string Address2 { get; set; }
          [Required]
          [StringLength(50)]
          public string City_Town { get; set; }
          [Required]
          [StringLength(50)]
          public string Province_State { get; set; }
          [Required]
          [StringLength(50)]
          public string Country { get; set; }
          [Required]
          [StringLength(50)]
          public string ZipCode { get; set; }
     }
}