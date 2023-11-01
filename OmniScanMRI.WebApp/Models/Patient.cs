using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Web;
using System.Web.Mvc;

namespace OmniScanMRI.WebApp.Models
{
    public class Patient
    {
        [Key]
        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "User ID")]
        public string PatientID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Column(TypeName = "Date")]
        public DateTime DateOfBirth { get; set; }
        public string Gender { get; set; }  
        public string Address { get; set; }
        public string ContactNumber { get; set; }
        public string Email { get; set; }
        public string MedicalHistory { get; set; }
        public ICollection<ScanDetails> ScanDetails { get; set; }

        public virtual ICollection<Appointments> Appointments { get; set; }
        
        public virtual ApplicationUser ApplicationUser { get; set; }
    }

    
}