using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class ClientsContactInformation : BaseModelGuid {

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
          public string CityState { get; set; }
          [Required]
          [StringLength(50)]
          public string Province { get; set; }
          [Required]
          [StringLength(50)]
          public string Country { get; set; }
          [Required]
          [StringLength(50)]
          public string ZipCode { get; set; }
     }
}