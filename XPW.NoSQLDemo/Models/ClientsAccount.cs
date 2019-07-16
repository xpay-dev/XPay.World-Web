﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.BaseContext;
using XPW.Utilities.Enums;

namespace XPW.NoSQLDemo.Models {
     public class ClientsAccount : BaseModelGuid {
          public ClientsAccount() {
               IsVerified = false;
               Tag = Status.Inactive;
          }
          [Required]
          [StringLength(20)]
          public string Username { get; set; }
          [Required]
          [StringLength(60)]
          public string EmailAddress { get; set; }
          [Required]
          public string Password { get; set; }
          public Guid XPayContactInformationId { get; set; }
          [ForeignKey("ClientsContactInformationId")]
          public ClientsContactInformation ClientsContactInformation { get; set; }
          [Required]
          public Guid ClientsRoleId { get; set; }
          [ForeignKey("ClientsRoleId")]
          public ClientsRole ClientsRole { get; set; }
          public int? ParentClientsRoleId { get; set; }
          [ForeignKey("ParentClientsRoleId")]
          public ClientsRole ParentClientsRole { get; set; }
          public Guid? ParentClientId { get; set; }
          public bool IsVerified { get; set; }
          public Status Tag { get; set; }
     }
}