﻿using System;
using System.ComponentModel.DataAnnotations;

namespace XPW.Admin.App_Models.Request {
     public class RoleModel {
          public int? Id { get; set; }
          [Required]
          [MaxLength(15, ErrorMessage = "Invalid string length, must be less than 15 characters")]
          [MinLength(4, ErrorMessage = "Invalid string length, must be greater than four characters")]
          public string Name { get; set; }
          [Required]
          [Range(1,30, ErrorMessage = "Out of range, must be between from 1 to 30 only")]
          public int Order { get; set; }
          public DateTime DateCreated { get; set; }
          public DateTime? DateUpdated { get; set; }
     }
}