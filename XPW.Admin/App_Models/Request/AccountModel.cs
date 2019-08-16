﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace XPW.Admin.App_Models.Request {
     public class AccountModel {
          [Required]
          [StringLength(20)]
          public string Username { get; set; }
          [Required]
          [StringLength(60)]
          public string EmailAddress { get; set; }
          public string Password { get; set; }
          [Required]
          public int RoleId { get; set; }
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
     }
}