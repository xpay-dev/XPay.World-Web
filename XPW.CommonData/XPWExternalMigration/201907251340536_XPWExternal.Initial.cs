namespace XPW.CommonData.XPWExternalMigration
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class XPWExternalInitial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PasswordHistories",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        AccountId = c.Guid(nullable: false),
                        Password = c.String(nullable: false),
                        Source = c.Int(nullable: false),
                        Status = c.Int(nullable: false),
                        DateCreated = c.DateTime(nullable: false),
                        DateUpdated = c.DateTime(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PasswordHistories");
        }
    }
}
