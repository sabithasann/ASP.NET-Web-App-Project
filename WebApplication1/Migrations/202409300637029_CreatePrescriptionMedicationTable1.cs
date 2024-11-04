namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreatePrescriptionMedicationTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Prescriptions", "Diagnosis", c => c.String(maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Prescriptions", "Diagnosis", c => c.String(nullable: false, maxLength: 1000));
        }
    }
}
