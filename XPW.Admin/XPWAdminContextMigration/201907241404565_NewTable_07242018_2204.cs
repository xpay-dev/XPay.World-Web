namespace XPW.Admin.XPWAdminContextMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NewTable_07242018_2204 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AccountInformations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FirstName = c.String(nullable: false, maxLength: 20),
                        MiddleName = c.String(nullable: false, maxLength: 20),
                        LastName = c.String(nullable: false, maxLength: 20),
                        Birthday = c.DateTime(nullable: false, storeType: "date"),
                        MobileNumber = c.String(nullable: false, maxLength: 30),
                        Address1 = c.String(nullable: false, maxLength: 100),
                        Address2 = c.String(nullable: false, maxLength: 100),
                        City_Town = c.String(nullable: false, maxLength: 50),
                        Province_State = c.String(nullable: false, maxLength: 50),
                        Country = c.String(nullable: false, maxLength: 50),
                        ZipCode = c.String(nullable: false, maxLength: 50),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Accounts",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Username = c.String(nullable: false, maxLength: 20),
                        EmailAddress = c.String(nullable: false, maxLength: 60),
                        Password = c.String(nullable: false),
                        AccountInformationId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                        IsVerified = c.Boolean(nullable: false),
                        IsDeleted = c.Boolean(nullable: false),
                        Tag = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AccountInformations", t => t.AccountInformationId, cascadeDelete: true)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.AccountInformationId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Order = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Accounts", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.Accounts", "AccountInformationId", "dbo.AccountInformations");
            DropIndex("dbo.Accounts", new[] { "RoleId" });
            DropIndex("dbo.Accounts", new[] { "AccountInformationId" });
            DropTable("dbo.Roles");
            DropTable("dbo.Accounts");
            DropTable("dbo.AccountInformations");
        }
    }
}
