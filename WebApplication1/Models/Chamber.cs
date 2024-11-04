using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Chamber
    {
        [Key]
        public int ChamberId { get; set; }

        [Required(ErrorMessage = "Please provide the chamber name.")]
        [Display(Name = "Chamber Name")]
        public string ChamberName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "An email address is required to proceed.")]
        [Display(Name = "Email Address")]
        public string Email {  get; set; }

        [Required(ErrorMessage = "Please provide the chamber address.")]
        [Display(Name = "Chamber Address")]
        public string Address { get; set; }

        [Required(ErrorMessage = "A phone number is required.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(?:\+?88)?01[3-9]\d{8}$", ErrorMessage = "Please enter a valid Bangladeshi phone number.")]
        public string Phone { get; set; }   
    }
}