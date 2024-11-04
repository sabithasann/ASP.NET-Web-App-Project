namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateChamberTimeSlotTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Chambers",
                c => new
                    {
                        ChamberId = c.Int(nullable: false, identity: true),
                        ChamberName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.ChamberId);
            
            AddColumn("dbo.AssistantDetails", "ChamberId", c => c.Int(nullable: false));
            CreateIndex("dbo.AssistantDetails", "ChamberId");
            AddForeignKey("dbo.AssistantDetails", "ChamberId", "dbo.Chambers", "ChamberId", cascadeDelete: true);
            DropColumn("dbo.AssistantDetails", "Chamber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.AssistantDetails", "Chamber", c => c.String());
            DropForeignKey("dbo.AssistantDetails", "ChamberId", "dbo.Chambers");
            DropIndex("dbo.AssistantDetails", new[] { "ChamberId" });
            DropColumn("dbo.AssistantDetails", "ChamberId");
            DropTable("dbo.Chambers");
        }
    }
}
