using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Prescription
    {
        [Key]
        public int PrescriptionId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Issued")]
        public DateTime PrescriptionDate { get; set; }

        [StringLength(1000, MinimumLength = 2, ErrorMessage = "Diagnosis must be between 10 and 1000 characters.")]
        public string Diagnosis { get; set; }

        [StringLength(2000, ErrorMessage = "Additional notes cannot exceed 2000 characters.")]
        [Display(Name = "Additional Notes")]
        public string AdditionalNotes { get; set; }

        // Foreign key to Appointment
        public int AppointmentId { get; set; }
        public Appointment Appointment { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        public User User { get; set; }

        // Foreign key to Chamber
        public int ChamberId { get; set; }
        public Chamber Chamber { get; set; }

    }
}