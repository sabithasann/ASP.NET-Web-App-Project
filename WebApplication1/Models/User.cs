using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Required(ErrorMessage = "Please provide your unique user name.")]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [EmailAddress(ErrorMessage = "Please enter a valid email address.")]
        [Required(ErrorMessage = "An email address is required to proceed.")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "A phone number is required.")]
        [Display(Name = "Phone Number")]
        [RegularExpression(@"^(?:\+?88)?01[3-9]\d{8}$", ErrorMessage = "Please enter a valid Bangladeshi phone number.")]
        public string Phone {  get; set; }

        [Required(ErrorMessage = "Please enter your password.")]
        [DataType(DataType.Password)]
        [MinLength(3, ErrorMessage = "Password must be at least 3 characters long.")]
        public string Password { get; set; }

        [Display(Name = "Confirm Password")]
        [Required(ErrorMessage = "Please re-enter your password for confirmation.")]
        [Compare("Password", ErrorMessage = "Passwords do not match. Please make sure both passwords are the same.")]
        [DataType(DataType.Password)]
        public string RePassword { get; set; }

        // Foreign key to Role
        public int RoleId { get; set; }
        public Role Role { get; set; }

        // Foreign key to Chamber
        public int? ChamberId { get; set; }
        public Chamber Chamber { get; set; }
    }
}