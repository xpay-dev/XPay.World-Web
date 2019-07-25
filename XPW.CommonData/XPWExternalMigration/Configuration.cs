namespace XPW.CommonData.XPWExternalMigration
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<XPW.CommonData.XPWExternal.DataContext.XPWExternalContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            MigrationsDirectory = @"XPWExternalMigration";
        }

        protected override void Seed(XPW.CommonData.XPWExternal.DataContext.XPWExternalContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
