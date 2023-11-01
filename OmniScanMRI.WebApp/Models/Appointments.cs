using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Mvc;


namespace OmniScanMRI.WebApp.Models
{

    public class Appointments
    {
        [Key]
        [Required]
        public string AppointmentID { get; set; }

        [Required]
        [ForeignKey("Patient")]
        public string PatientID { get; set; }
        public virtual Patient Patient { get; set; }

        [Required]
        [ForeignKey("Doctor")]
        public string DoctorID { get; set; }
        public virtual Doctors Doctor { get; set; }

        [ForeignKey("Administrator")]
        public string AdminID { get; set; }
        public virtual Administrators Administrator { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        [Display(Name = "Appointment Date & Time")]
        [Column(TypeName = "datetime2")]
        public DateTime AppointmentDttm { get; set; }

        [StringLength(500)]
        [Display(Name = "Additional Notes")]
        public string Notes { get; set; }


        [StringLength(50)]
        public string Status { get; set; } 

        public IEnumerable<SelectListItem> DoctorsList { get; set; }

        public string Email { get; set; }
    }
}