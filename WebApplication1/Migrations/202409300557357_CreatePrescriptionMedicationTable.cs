namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePrescriptionMedicationTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Medications",
                c => new
                    {
                        MedicationId = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 200),
                        Dosage = c.String(maxLength: 500),
                        Instructions = c.String(maxLength: 1000),
                        PrescriptionId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.MedicationId)
                .ForeignKey("dbo.Prescriptions", t => t.PrescriptionId, cascadeDelete: true)
                .Index(t => t.PrescriptionId);
            
            CreateTable(
                "dbo.Prescriptions",
                c => new
                    {
                        PrescriptionId = c.Int(nullable: false, identity: true),
                        PrescriptionDate = c.DateTime(nullable: false),
                        Diagnosis = c.String(nullable: false, maxLength: 1000),
                        AdditionalNotes = c.String(maxLength: 2000),
                        AppointmentId = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                        ChamberId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PrescriptionId)
                .ForeignKey("dbo.Appointments", t => t.AppointmentId, cascadeDelete: false)
                .ForeignKey("dbo.Chambers", t => t.ChamberId, cascadeDelete: false)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: false)
                .Index(t => t.AppointmentId)
                .Index(t => t.UserId)
                .Index(t => t.ChamberId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Medications", "PrescriptionId", "dbo.Prescriptions");
            DropForeignKey("dbo.Prescriptions", "UserId", "dbo.Users");
            DropForeignKey("dbo.Prescriptions", "ChamberId", "dbo.Chambers");
            DropForeignKey("dbo.Prescriptions", "AppointmentId", "dbo.Appointments");
            DropIndex("dbo.Prescriptions", new[] { "ChamberId" });
            DropIndex("dbo.Prescriptions", new[] { "UserId" });
            DropIndex("dbo.Prescriptions", new[] { "AppointmentId" });
            DropIndex("dbo.Medications", new[] { "PrescriptionId" });
            DropTable("dbo.Prescriptions");
            DropTable("dbo.Medications");
        }
    }
}
