using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.OpenIdConnect;
using OmniScanMRI.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OmniScanMRI.WebApp.Controllers
{

    public class AppointmentController : Controller
    {
        OmniScanContext Omnicontext = new OmniScanContext();

        

        public ActionResult BookAppointment()
        {

            if (User.Identity.IsAuthenticated)
            {
                if (!User.IsInRole("Patient"))
                {
                    return RedirectToAction("UnauthorizedAccess", "Account");
                }
                else
                {
                    var viewModel = new Appointments
                    {
                        DoctorsList = Omnicontext.Doctors.Select(d => new SelectListItem
                        {
                            Value = d.DoctorID.ToString(),
                            Text = d.FirstName + " " + d.LastName
                        }).ToList()
                    };

                    return View(viewModel);
                }
            }
            else
            {
                Session["RedirectToBooking"] = true;

                var testSession = Session["RedirectToBooking"];
                System.Diagnostics.Debug.WriteLine($"BookAppointment session value: {testSession}");
                string returnUrl = Request.Url.AbsolutePath;
                return Redirect($"/Account/Login?returnUrl={returnUrl}");
            }

            
        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointment(Appointments model)
        {
            System.Diagnostics.Debug.WriteLine("Book Appointment called");
            model.PatientID = User.Identity.GetUserId(); ;
            model.Status = "Confirmed";
            model.AppointmentID = Guid.NewGuid().ToString();
            ModelState.Remove("Status");
            ModelState.Remove("PatientID");
            ModelState.Remove("AppointmentID");

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Model success");
                var appointment = new Appointments
                {
                    AppointmentID = model.AppointmentID,
                    PatientID = model.PatientID,
                    DoctorID = model.DoctorID,
                    AppointmentDttm = model.AppointmentDttm.ToUniversalTime(),
                    Notes = model.Notes,
                    Status = model.Status,
                    AdminID = null
                };

                Omnicontext.Appointments.Add(appointment);
                Omnicontext.SaveChanges();
                
                System.Diagnostics.Debug.WriteLine("Context Save Success");
                return RedirectToAction("AppointmentSuccess", new { id = appointment.AppointmentID });
            }

            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }
                model.DoctorsList = Omnicontext.Doctors.Select(d => new SelectListItem
                {
                    Value = d.DoctorID.ToString(),
                    Text = d.FirstName + " " + d.LastName
                }).ToList();

                return View(model);
            }

            
        }

        bool IsConflict(string doctorId, DateTime desiredAppointment)
        {
            return Omnicontext.Appointments.Any(a => a.DoctorID.Equals(doctorId) && a.AppointmentDttm == desiredAppointment);
        }

        public ActionResult AppointmentSuccess(string id)
        {
            if (User.IsInRole("Patient") || User.IsInRole("Administrator"))
            {
                var appointment = Omnicontext.Appointments.FirstOrDefault(a => a.AppointmentID == id);
                if (appointment == null)
                {
                    return RedirectToAction("ManageAppointments", "Appointment");
                }
                return View(appointment);
            }
            return RedirectToAction("UnauthorizedAccess", "Account");

        }

        public ActionResult ViewAppointments(string patientId)
        {
            if (!User.IsInRole("Patient"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var appointments = Omnicontext.Appointments.Where(a => a.PatientID.Equals(patientId)).ToList();
            return View(appointments);
        }

        public ActionResult ManageAppointments()
        {
            if (!User.IsInRole("Patient"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult CancelAppointment(string appointmentId)
        {
            if (!User.IsInRole("Patient"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var appointment = Omnicontext.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                return HttpNotFound();
            }

            appointment.Status = "Cancelled";

            Omnicontext.SaveChanges();

            return RedirectToAction("ViewAppointments", new { patientId = appointment.PatientID });
        }


        //GET
        public ActionResult EditAppointments(Appointments model, string appointmentId)
        {
            if (!User.IsInRole("Patient"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            var appointment = Omnicontext.Appointments.Find(appointmentId);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            model.DoctorsList = Omnicontext.Doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorID.ToString(),
                Text = d.FirstName + " " + d.LastName
            }).ToList();

            model.PatientID = appointment.PatientID;
            model.AppointmentID = appointment.AppointmentID;
            model.DoctorID = appointment.DoctorID;
            model.AppointmentDttm = appointment.AppointmentDttm;
            model.Notes = appointment.Notes;
            model.Status = "Confirmed";

            return View(model);
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointments(Appointments model)
        {

            if (ModelState.IsValid)
            {
                var appointmentInDb = Omnicontext.Appointments.Find(model.AppointmentID);
                if (appointmentInDb == null)
                {
                    return HttpNotFound();
                }
                
                appointmentInDb.AppointmentDttm = model.AppointmentDttm;
                appointmentInDb.DoctorID = model.DoctorID;
                appointmentInDb.Notes = model.Notes;
              
                Omnicontext.SaveChanges();

                return RedirectToAction("ViewAppointments", new { patientId = appointmentInDb.PatientID });
            }
            else
            {
                foreach (var modelState in ModelState.Values)
                {
                    foreach (var error in modelState.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine(error.ErrorMessage);
                    }
                }

                model.DoctorsList = Omnicontext.Doctors.Select(d => new SelectListItem
                {
                    Value = d.DoctorID.ToString(),
                    Text = d.FirstName + " " + d.LastName
                }).ToList();

                return View(model);
            }
            
        }


        public JsonResult GetBookedApptTimes(string doctorId)
        {
            var bookedAppointments = Omnicontext.Appointments
                                .Where(a => a.DoctorID.Equals(doctorId))
                                .ToList();

            var events = bookedAppointments.Select(appt => new
            {
                title = "Booked-NotAvailable",
                start = appt.AppointmentDttm.ToString("dd-MM-yyyy hh:mm tt"),
                allDay = false
            }).ToList();

            return Json(events, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetAvailableApptTimes(string doctorId)
        {
            var availableDateTimes = Omnicontext.ApptSlots
                                    .Where(a => a.DoctorID.Equals(doctorId))
                                    .Select(a => a.AvailableDateTime)
                                    .ToList();

            var availableEvents = availableDateTimes.Select(dt => new
            {
                groupId = "availableSlots",
                start = dt.ToString("dd-MM-yyyy hh:mm tt"),
                display = "background",
                allDay = false
            }).ToList();

            return Json(availableEvents, JsonRequestBehavior.AllowGet);
        }

        //private void ForceSignOut()
        //{
        //    // Remove the application's local authentication cookie.
        //    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
        //    // Remove external authentication cookies.
        //    HttpContext.GetOwinContext().Authentication.SignOut(DefaultAuthenticationTypes.ExternalCookie);
        //    // Sign out from Microsoft's session.
        //    HttpContext.GetOwinContext().Authentication.SignOut(OpenIdConnectAuthenticationDefaults.AuthenticationType);
        //}

        //public JsonResult GetApptTimeSlot(string doctorId)
        //{
        //    var availableDateTimes = _context.ApptSlots
        //                        .Where(a => a.DoctorID.Equals(doctorId))
        //                        .Select(a => a.AvailableDateTime)
        //                        .ToList();

        //    var formattedTimes = availableDateTimes.Select(dt => dt.ToString("dd-MM-yyyyTHH:mm tt")).ToList();

        //    return Json(formattedTimes, JsonRequestBehavior.AllowGet);
        //}

    }
}