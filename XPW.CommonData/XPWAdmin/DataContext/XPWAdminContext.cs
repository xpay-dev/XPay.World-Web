using System.Data.Entity;
using XPW.CommonData.XPWAdmin.Entities;

namespace XPW.CommonData.XPWAdmin.DataContext {
     public partial class XPWAdminContext : DbContext {
          public XPWAdminContext()
              : base("name=XPWAdmin") {
          }
          protected override void OnModelCreating(DbModelBuilder modelBuilder) {
          }
          public DbSet<Account> Accounts { get; set; }
          public DbSet<AccountInformation> AccountInformation { get; set; }
          public DbSet<Role> Roles { get; set; }
          public DbSet<ApplicationAccess> ApplicationAccesses { get; set; }
          public DbSet<ApplicationUserAccess> ApplicationUserAccesses { get; set; }
     }
}
