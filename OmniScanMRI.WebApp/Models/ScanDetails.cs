using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace OmniScanMRI.WebApp.Models
{
    public class ScanDetails
    {
        [Key]
        [Required]
        public string ScanID { get; set; }

        [Required]
        [StringLength(255)]
        [Display(Name = "Image File Path")]
        public string FilePath { get; set; }

        [Required]
        [StringLength(100)]
        public string FileName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Column(TypeName = "Date")]
        [Display(Name = "Date Taken")]
        public DateTime DateTaken { get; set; }

        [Required]
        [ForeignKey("ApplicationUser")]
        [Display(Name = "User ID")]
        public string UploadBy_UserId { get; set; }

        [ForeignKey("Patient")]
        public string Patient_PatientID { get; set; }
        public virtual Patient Patient { get; set; }



        public virtual ApplicationUser ApplicationUser { get; set; }

        [Display(Name = "Doctor's Notes")]
        [MaxLength(1000)]
        public string DoctorsNotes { get; set; }

        // Patient's medical history
        [Display(Name = "Medical History")]
        [MaxLength(1000)]
        public string MedicalHistory { get; set; }
    }
}
