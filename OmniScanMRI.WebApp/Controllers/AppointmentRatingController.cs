using Microsoft.AspNet.Identity;
using Microsoft.Graph.Models;
using OmniScanMRI.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OmniScanMRI.WebApp.Controllers
{
    public class AppointmentRatingController : Controller
    {
        OmniScanContext OmniContext = new OmniScanContext();

        [HttpPost]
        public JsonResult SaveRating(int ratedValue, string appointmentId)
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var rating = new Ratings
                {
                    RatingID = Guid.NewGuid().ToString(),
                    RatedByPatientID = userId,
                    RatedAppointmentID = appointmentId,
                    RatingValue = ratedValue,
                    Comments = null 
                };

                OmniContext.Ratings.Add(rating);
                OmniContext.SaveChanges();

                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false });
            }
        }

        public double GetOverallAggregatedRating()
        {
            var allRatings = OmniContext.Ratings.ToList();
            if (!allRatings.Any()) 
                return 0; 

            double totalScore = allRatings.Sum(r => r.RatingValue);
            return totalScore / allRatings.Count;
        }


    }
}