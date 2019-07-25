namespace XPW.CommonData.XPWAdminMigration {
     using System.Data.Entity.Migrations;

     internal sealed class Configuration : DbMigrationsConfiguration<XPW.CommonData.XPWAdmin.DataContext.XPWAdminContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"XPWAdminMigration";
        }

        protected override void Seed(XPW.CommonData.XPWAdmin.DataContext.XPWAdminContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
