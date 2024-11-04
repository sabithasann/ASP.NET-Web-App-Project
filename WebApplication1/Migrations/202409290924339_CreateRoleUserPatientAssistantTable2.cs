namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRoleUserPatientAssistantTable2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.AssistantDetails", "ChamberId", "dbo.Chambers");
            DropIndex("dbo.AssistantDetails", new[] { "ChamberId" });
            AddColumn("dbo.Users", "ChamberId", c => c.Int(nullable: true));
            CreateIndex("dbo.Users", "ChamberId");
            AddForeignKey("dbo.Users", "ChamberId", "dbo.Chambers", "ChamberId", cascadeDelete: false);
            DropColumn("dbo.AssistantDetails", "ChamberId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssistantDetails", "ChamberId", c => c.Int(nullable: false));
            DropForeignKey("dbo.Users", "ChamberId", "dbo.Chambers");
            DropIndex("dbo.Users", new[] { "ChamberId" });
            DropColumn("dbo.Users", "ChamberId");
            CreateIndex("dbo.AssistantDetails", "ChamberId");
            AddForeignKey("dbo.AssistantDetails", "ChamberId", "dbo.Chambers", "ChamberId", cascadeDelete: true);
        }
    }
}
