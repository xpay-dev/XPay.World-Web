using System.Data.Entity;
using XPW.CommonData.Entities;

namespace XPW.CommonData.DataContext {
     public partial class XPWCommonDataContext : DbContext {
          public XPWCommonDataContext()
              : base("name=XPWCommonData") {
          }
          protected override void OnModelCreating(DbModelBuilder modelBuilder) {
          }
          public DbSet<PasswordHistory> PasswordHistories { get; set; }
     }
}

