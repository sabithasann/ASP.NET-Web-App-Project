namespace WebApplication1.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class CreateRoleUserPatientAssistantTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.AssistantDetails",
                c => new
                    {
                        AssistantDetailsId = c.Int(nullable: false, identity: true),
                        Chamber = c.String(),
                        DateOfBirth = c.DateTime(),
                        Gender = c.String(),
                        Address = c.String(),
                        BloodGroup = c.String(),
                        EmergencyContact = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.AssistantDetailsId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        Phone = c.String(nullable: false),
                        Password = c.String(nullable: false),
                        RePassword = c.String(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        RoleName = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.PatientDetails",
                c => new
                    {
                        PatientDetailsId = c.Int(nullable: false, identity: true),
                        DateOfBirth = c.DateTime(),
                        Gender = c.String(),
                        Address = c.String(),
                        BloodGroup = c.String(),
                        EmergencyContact = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.PatientDetailsId)
                .ForeignKey("dbo.Users", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PatientDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.AssistantDetails", "UserId", "dbo.Users");
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropIndex("dbo.PatientDetails", new[] { "UserId" });
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.AssistantDetails", new[] { "UserId" });
            DropTable("dbo.PatientDetails");
            DropTable("dbo.Roles");
            DropTable("dbo.Users");
            DropTable("dbo.AssistantDetails");
        }
    }
}
