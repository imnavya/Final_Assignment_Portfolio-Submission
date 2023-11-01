using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class Doctors
    {
        [Key]
        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "User ID")]
        public string DoctorID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Specialization { get; set; }
        public string LicenseNumber { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        
        public virtual ICollection<Appointments> Appointments { get; set; }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}