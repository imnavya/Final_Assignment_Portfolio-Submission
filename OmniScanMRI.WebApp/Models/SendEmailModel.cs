using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class SendEmailModel
    {
        [Required(ErrorMessage = "Recipient Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address entered, please try again")]
        [Display(Name = "Recipient Email")]
        public string ToEmail { get; set; }

        [Required(ErrorMessage = "Subject is required")]
        [StringLength(100, ErrorMessage = "Subject cannot be longer than 100 characters, please try again")]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "Email Content is required")]
        [StringLength(1000, ErrorMessage = "Content length cannot be longer than 1000 characters, please try again")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Display(Name = "Sent Date")]
        [DataType(DataType.Date)]
        public DateTime SentDate { get; set; }

        [Display(Name = "CC Email")]
        [EmailAddress(ErrorMessage = "Invalid CC email address")]
        public string CcEmail { get; set; }

        public HttpPostedFileBase FileAttachment { get; set; }
    }
}