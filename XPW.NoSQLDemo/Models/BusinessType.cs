using System.ComponentModel.DataAnnotations;
using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class BusinessType : BaseModelInt {
          [Required]
          [StringLength(50)]
          public string Name { get; set; }
     }
}