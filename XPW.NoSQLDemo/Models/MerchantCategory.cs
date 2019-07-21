using System.ComponentModel.DataAnnotations;
using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class MerchantCategory : BaseModelInt {
          public int MCC { get; set; }

          [Required]
          public string MerchantType { get; set; }
     }
}