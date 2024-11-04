namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateUserModel : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Users", "ChamberId", "dbo.Chambers");
            DropIndex("dbo.Users", new[] { "ChamberId" });
            AlterColumn("dbo.Users", "ChamberId", c => c.Int(nullable: true));
            CreateIndex("dbo.Users", "ChamberId");
            AddForeignKey("dbo.Users", "ChamberId", "dbo.Chambers", "ChamberId", cascadeDelete: false);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "ChamberId", "dbo.Chambers");
            DropIndex("dbo.Users", new[] { "ChamberId" });
            AlterColumn("dbo.Users", "ChamberId", c => c.Int(nullable: false));
            CreateIndex("dbo.Users", "ChamberId");
            AddForeignKey("dbo.Users", "ChamberId", "dbo.Chambers", "ChamberId", cascadeDelete: true);
        }
    }
}
