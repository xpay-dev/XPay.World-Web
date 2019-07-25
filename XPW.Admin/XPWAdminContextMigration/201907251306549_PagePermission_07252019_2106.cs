namespace XPW.Admin.XPWAdminContextMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PagePermission_07252019_2106 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ApplicationAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        ParentName = c.String(),
                        Order = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ApplicationUserAccesses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Guid(nullable: false),
                        Username = c.String(),
                        ApplicationAccessId = c.Int(nullable: false),
                        ApplicationAccessName = c.String(),
                        Tag = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ApplicationUserAccesses");
            DropTable("dbo.ApplicationAccesses");
        }
    }
}
