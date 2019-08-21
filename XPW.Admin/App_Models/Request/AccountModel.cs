using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XPW.Admin.App_Models.Request {
     public class AccountModel {
          public Guid? Id { get; set; }
          [Required]
          [StringLength(20)]
          public string Username { get; set; }
          [Required]
          [StringLength(60)]
          public string EmailAddress { get; set; }
          public string Password { get; set; }
          [Required]
          public int RoleId { get; set; }
          public int? AccountInformationId { get; set; }
          [Required]
          [StringLength(20)]
          public string FirstName { get; set; }
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
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
          public string Token { get; set; }
          public DateTime TokenExpiry { get; set; }
     }
     public class AccountUserModel {
          public Guid? Id { get; set; }
          [Required]
          [StringLength(20)]
          public string Username { get; set; }
          [Required]
          [StringLength(60)]
          public string EmailAddress { get; set; }
          public string Password { get; set; }
          [Required]
          public int RoleId { get; set; }
          public int? AccountInformationId { get; set; }
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
     }
     public class AccountUserInformation {
          public int? Id { get; set; }

          [Required]
          [StringLength(20)]
          public string FirstName { get; set; }
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
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
     }
}