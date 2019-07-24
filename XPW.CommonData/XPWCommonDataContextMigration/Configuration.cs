namespace XPW.CommonData.XPWCommonDataContextMigration {
     using System.Data.Entity.Migrations;

     internal sealed class Configuration : DbMigrationsConfiguration<XPW.CommonData.DataContext.XPWCommonDataContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"XPWCommonDataContextMigration";
        }

        protected override void Seed(XPW.CommonData.DataContext.XPWCommonDataContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
