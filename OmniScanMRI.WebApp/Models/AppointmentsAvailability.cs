using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class AppointmentsAvailability
    {
        [Key]
        [Required]
        public string TimeSlotID { get; set; }

        [Required(ErrorMessage = "Doctor selection is required.")]
        [Display(Name = "Doctor")]
        [ForeignKey("Doctor")]
        public string DoctorID { get; set; }

        public virtual Doctors Doctor { get; set; }

        [Required(ErrorMessage = "Date and time is required.")]
        [Display(Name = "Available Date and Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy HH:mm}")]
        public DateTime AvailableDateTime { get; set; }

       
    }
}