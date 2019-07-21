using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using XPW.Utilities.BaseContext;

namespace XPW.NoSQLDemo.Models {
     public class ClientsBusinessInformation : BaseModelGuid {
          [Required]
          [StringLength(30)]
          public string BusinessName { get; set; }
          [Required]
          public int YearsInOperation { get; set; }
          [Required]
          public int NumOfBranches { get; set; }
          [Required]
          public decimal MonthlyCreditCardSale { get; set; }
          [Required]
          public decimal MinimumSaleAmount { get; set; }
          [Required]
          public decimal MaximumSaleAmount { get; set; }
          [Required]
          [StringLength(100)]
          public string Website { get; set; }
          [Required]
          public int BusinessTypeId { get; set; }
          [ForeignKey("BusinessTypeId")]
          public virtual BusinessType BusinessType { get; set; }
          [Required]
          public int MerchantCategoryId { get; set; }
          [ForeignKey("MerchantCategoryId")]
          public virtual MerchantCategory MerchantCategory { get; set; }
          [Required]
          public Guid ClientAccountId { get; set; }
          [Required]
          public int MRDRateId { get; set; }
          [ForeignKey("MDRRateId")]
          public virtual MDRRate MDRRate { get; set; }
     }
}