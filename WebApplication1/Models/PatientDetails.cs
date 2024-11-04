using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class PatientDetails
    {
        [Key]
        public int PatientDetailsId { get; set; }

        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Display(Name = "Date Of Birth")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public Nullable<DateTime> DateOfBirth { get; set; }

        public string Gender { get; set; }

        public string Address { get; set; }

        [Display(Name = "Blood Group")]
        public string BloodGroup { get; set; }

        [Display(Name = "Emergency Contact")]
        public string EmergencyContact { get; set; }

        // Foreign key to User
        public int UserId { get; set; }
        public User User { get; set; }
    }
}