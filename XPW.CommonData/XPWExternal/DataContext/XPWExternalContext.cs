using System.Data.Entity;
using XPW.CommonData.XPWExternal.Entities;

namespace XPW.CommonData.XPWExternal.DataContext {
     public partial class XPWExternalContext : DbContext {
          public XPWExternalContext()
              : base("name=XPWExternal") {
          }
          protected override void OnModelCreating(DbModelBuilder modelBuilder) {
          }
          public DbSet<PasswordHistory> PasswordHistories { get; set; }
     }
}

