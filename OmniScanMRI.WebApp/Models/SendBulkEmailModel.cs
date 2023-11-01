using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class SendBulkEmailModel
    {
        [Required]
        [Display(Name = "Subject")]
        public string Subject { get; set; }

        [Required]
        [Display(Name = "Message Content")]
        public string Content { get; set; }

        [Display(Name = "CC Email")]
        [EmailAddress]
        public string CcEmail { get; set; }
        public List<string> ToEmails { get; set; }

    }
}