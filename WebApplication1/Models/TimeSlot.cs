using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class TimeSlot
    {
        [Key]
        public int TimeSlotId { get; set; }

        [Required(ErrorMessage = "A Time is required.")]
        public string Time { get; set; }

        // Foreign key to Chamber
        public int ChamberId { get; set; }
        public Chamber Chamber { get; set; }
    }
}