using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class TrackEmail
    {
        [Key]
        [Required]
        public string TrackID { get; set; }

        [Required]
        public string ToEmail { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }

        [Required]
        [ForeignKey("Administrator")]
        public string AdminID { get; set; }
        public virtual Administrators Administrator { get; set; }

        [Required]
        public DateTime SentDate { get; set; }
        public string CCEmail { get; set; }

        
    }
}