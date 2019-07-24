namespace XPW.Admin.XPWAdminContextMigration {
     using System.Data.Entity.Migrations;

     internal sealed class Configuration : DbMigrationsConfiguration<XPW.Admin.Configurations.DataContext.XPWAdminContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"XPWAdminContextMigration";
        }

        protected override void Seed(XPW.Admin.Configurations.DataContext.XPWAdminContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
