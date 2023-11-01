using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class Ratings
    {
        [Key]
        [Required]
        public string RatingID { get; set; }

        
        [ForeignKey("RatedByPatient")]
        [Required(ErrorMessage = "Rater's patient code is required.")]
        [Display(Name = "Rater's Patient ID")]
        public string RatedByPatientID { get; set; }
        public virtual Patient RatedByPatient { get; set; }


        [ForeignKey("RatedAppointment")]
        [Required(ErrorMessage = "Booking reference is required.")]
        [Display(Name = "Rated Appointment ID")]
        public string RatedAppointmentID { get; set; }
        public virtual Appointments RatedAppointment { get; set; }

        [Range(1, 5, ErrorMessage = "Rating value must be between 1 and 5.")]
        [Required(ErrorMessage = "Rating value is required.")]
        public int RatingValue { get; set; }

        [StringLength(500, ErrorMessage = "Comment can be a maximum of 500 characters long.")]
        [Display(Name = "Rating Feedback Comment")]
        public string Comments { get; set; }
    }
}