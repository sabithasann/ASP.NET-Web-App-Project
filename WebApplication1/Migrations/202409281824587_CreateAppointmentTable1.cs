namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateAppointmentTable1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Appointments", "CheckIn", c => c.Boolean());
            AlterColumn("dbo.Appointments", "Prescribed", c => c.Boolean());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Appointments", "Prescribed", c => c.String());
            AlterColumn("dbo.Appointments", "CheckIn", c => c.String());
        }
    }
}
