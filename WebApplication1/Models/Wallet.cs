using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Wallet
    {
        [Key]
        public int WalletId { get; set; }

        [Required]
        public DateTime BillingDate { get; set; }

        [Required]
        public double BillAmount { get; set; }

        // Foreign key to Appointment
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        // Foreign key to User (Assistant)
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign key to Chamber
        public int ChamberId { get; set; }
        public Chamber Chamber { get; set; }
    }
}