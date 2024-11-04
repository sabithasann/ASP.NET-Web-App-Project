namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAppointmentTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Appointments",
                c => new
                    {
                        AppointmentId = c.Int(nullable: false, identity: true),
                        AppointmentDate = c.DateTime(nullable: false),
                        Time = c.String(),
                        CheckIn = c.String(),
                        Prescribed = c.String(),
                        UserId = c.Int(nullable: false),
                        ChamberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AppointmentId)
                .ForeignKey("dbo.Chambers", t => t.ChamberId, cascadeDelete: true)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.ChamberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Appointments", "UserId", "dbo.Users");
            DropForeignKey("dbo.Appointments", "ChamberId", "dbo.Chambers");
            DropIndex("dbo.Appointments", new[] { "ChamberId" });
            DropIndex("dbo.Appointments", new[] { "UserId" });
            DropTable("dbo.Appointments");
        }
    }
}
