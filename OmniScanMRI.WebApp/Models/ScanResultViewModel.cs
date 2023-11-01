using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OmniScanMRI.WebApp.Models
{
    public class ScanResultViewModel
    {
        
        public DateTime DateTaken { get; set; }
        public string UploadedBy { get; set; } 
        public string FilePath { get; set; }
        

    }
}