using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Medication
    {
        [Key]
        public int MedicationId { get; set; }

        [Required]
        [StringLength(200, ErrorMessage = "Medication name cannot exceed 200 characters.")]
        [Display(Name = "Medication Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Dosage information cannot exceed 500 characters.")]
        public string Dosage {  get; set; }

        [StringLength(1000, ErrorMessage = "Instructions cannot exceed 1000 characters.")]
        public string Instructions { get; set; }

        // Foreign key to Prescription
        public int PrescriptionId { get; set; }
        public Prescription Prescription { get; set; }

    }
}