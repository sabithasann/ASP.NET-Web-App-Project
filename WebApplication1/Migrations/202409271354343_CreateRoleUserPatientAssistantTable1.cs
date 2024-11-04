namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRoleUserPatientAssistantTable1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AssistantDetails", "FullName", c => c.String());
            AddColumn("dbo.PatientDetails", "FullName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PatientDetails", "FullName");
            DropColumn("dbo.AssistantDetails", "FullName");
        }
    }
}
