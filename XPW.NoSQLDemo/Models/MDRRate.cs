using System.ComponentModel.DataAnnotations;
using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class MDRRate : BaseModelInt {
          [Required]
          public int MerchantCategoryId { get; set; }
          public decimal? ClientsRate { get; set; }
          public decimal? XPayRate { get; set; }
     }
}