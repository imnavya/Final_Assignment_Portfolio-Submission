using OmniScanMRI.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OmniScanMRI.WebApp.Controllers
{
    public class PatientController : Controller
    {
        private OmniScanContext OmniContext = new OmniScanContext();
        private AppointmentRatingController ratings = new AppointmentRatingController();
        // GET: Patient
        public ActionResult PatientHome()
        {
            if (!User.IsInRole("Patient"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var overallRating = ratings.GetOverallAggregatedRating();
            System.Diagnostics.Debug.WriteLine(overallRating);
            ViewBag.OverallRating = overallRating;
            ViewBag.TotalRatings = OmniContext.Ratings.Count();

            return View();
        }

        public ActionResult AccessScanResults(string patientId)
        {
            if (!User.IsInRole("Patient"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var scanResults = OmniContext.ScanImages
                              .Where(sd => sd.Patient_PatientID == patientId)
                              .Include(sd => sd.ApplicationUser)
                              .Include(sd => sd.Patient)
                              .Select(sd => new ScanResultViewModel
                              {
                                  DateTaken = sd.DateTaken,
                                  UploadedBy = OmniContext.Doctors
                                                        .FirstOrDefault(d => d.DoctorID == sd.UploadBy_UserId)
                                                        .FirstName,
                                  FilePath = sd.FilePath
                              })
                              .ToList();

            return View(scanResults);
        }




    }


}