namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateWalletTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Wallets",
                c => new
                    {
                        WalletId = c.Int(nullable: false, identity: true),
                        BillingDate = c.DateTime(nullable: false),
                        BillAmount = c.Double(nullable: false),
                        AppointmentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ChamberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.WalletId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: true)
                .ForeignKey("dbo.Chambers", t => t.ChamberId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.AppointmentId)
                .Index(t => t.UserId)
                .Index(t => t.ChamberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Wallets", "UserId", "dbo.Users");
            DropForeignKey("dbo.Wallets", "ChamberId", "dbo.Chambers");
            DropForeignKey("dbo.Wallets", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Wallets", new[] { "ChamberId" });
            DropIndex("dbo.Wallets", new[] { "UserId" });
            DropIndex("dbo.Wallets", new[] { "AppointmentId" });
            DropTable("dbo.Wallets");
        }
    }
}
