using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OmniScanMRI.WebApp.Models
{
    public class UploadScanViewModel
    {
        [Required(ErrorMessage = "Please patient MRI scan image to upload")]
        public HttpPostedFileBase UploadFile { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The name cannot be longer than 100 characters, please try again")]
        public string ScanName { get; set; }

        [Required(ErrorMessage = "Please click and start typing patient name to select a patient from the list")]
        public string SelectedPatientId { get; set; }
        public IEnumerable<SelectListItem> Patients { get; set; }


        [Required]
        [DataType(DataType.Date)]
        public DateTime DateTaken { get; set; }

        [Display(Name = "Doctor's Notes")]
        [MaxLength(1000)]
        public string DoctorsNotes { get; set; }

        // Patient's medical history
        [Display(Name = "Medical History")]
        [MaxLength(1000)]
        public string MedicalHistory { get; set; }

    }
}