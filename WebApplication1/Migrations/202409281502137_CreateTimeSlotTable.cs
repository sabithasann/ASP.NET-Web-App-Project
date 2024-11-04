namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateTimeSlotTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TimeSlots",
                c => new
                    {
                        TimeSlotId = c.Int(nullable: false, identity: true),
                        Time = c.String(nullable: false),
                        ChamberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.TimeSlotId)
                .ForeignKey("dbo.Chambers", t => t.ChamberId, cascadeDelete: true)
                .Index(t => t.ChamberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TimeSlots", "ChamberId", "dbo.Chambers");
            DropIndex("dbo.TimeSlots", new[] { "ChamberId" });
            DropTable("dbo.TimeSlots");
        }
    }
}
