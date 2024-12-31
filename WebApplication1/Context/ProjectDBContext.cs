using System;
using System.Data.Entity;
using System.Linq;
using WebApplication1.Models;

namespace WebApplication1.Context
{
    public class ProjectDBContext : DbContext
    {
        // Your context has been configured to use a 'ProjectDBContext' connection string from your application's 
        // configuration file (App.config or Web.config). By default, this connection string targets the 
        // 'WebApplication1.Context.ProjectDBContext' database on your LocalDb instance. 
        // 
        // If you wish to target a different database and/or database provider, modify the 'ProjectDBContext' 
        // connection string in the application configuration file.
        public ProjectDBContext()
            : base("name=ProjectDBContext")
        {
        }

        // Add a DbSet for each entity type that you want to include in your model. For more information 
        // on configuring and using a Code First model, see http://go.microsoft.com/fwlink/?LinkId=390109.

        // public virtual DbSet<MyEntity> MyEntities { get; set; }
        public virtual DbSet<Role> Role { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<PatientDetails> PatientDetail { get; set; }
        public virtual DbSet<AssistantDetails> AssistantDetail { get; set; }
        public virtual DbSet<Chamber> Chamber { get; set; }
        public virtual DbSet<TimeSlot> TimeSlot { get; set; }
        public virtual DbSet<Appointment> Appointment { get; set; }
        public virtual DbSet<Wallet> Wallet { get; set; }
        public virtual DbSet<Prescription> Prescription { get; set; }
        public virtual DbSet<Medication> Medication { get; set; }
        public virtual DbSet<Medicine> Medicine { get; set; } 

    }

    //public class MyEntity
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}
}