using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Appointment
    {
        [Key]
        public int AppointmentId { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string Time {  get; set; }
        public bool? CheckIn { get; set; }
        public bool? Prescribed { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign key to Chamber
        public int ChamberId { get; set; }
        public Chamber Chamber { get; set; }
    }
}