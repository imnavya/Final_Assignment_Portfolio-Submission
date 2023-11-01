using Microsoft.AspNet.Identity;
using Microsoft.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OmniScanMRI.WebApp.Models;
using System.Data.Entity.Validation;
using System.IO;


namespace OmniScanMRI.WebApp.Controllers
{
    public class DoctorController : Controller
    {
        OmniScanContext OmniContext = new OmniScanContext();

        // GET: Doctor
        public ActionResult DoctorHome()
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }

        public ActionResult ManageAppointmentsByDoctor()
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            string loggedInDoctorID = User.Identity.GetUserId();

            var confirmedAppointments = OmniContext.Appointments
                .Where(a => a.DoctorID == loggedInDoctorID && a.Status == "Confirmed")
                .ToList();

            return View(confirmedAppointments);
        }

        [HttpGet]
        public ActionResult ViewAppointmentByDoctor(string id)
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var appointment = OmniContext.Appointments.FirstOrDefault(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);
        }


        [HttpGet]
        public ActionResult CancelAppointmentByDoctor(string id)
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var appointment = OmniContext.Appointments.FirstOrDefault(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment); 
        }

        [HttpPost, ActionName("CancelAppointmentByDoctor")]
        public ActionResult CancelAppointmentConfirmed(string id)
        {
            var appointment = OmniContext.Appointments.FirstOrDefault(a => a.AppointmentID == id);

            if (appointment != null)
            {
                appointment.Status = "Cancelled";
                OmniContext.Entry(appointment).State = EntityState.Modified;
                OmniContext.SaveChanges();
            }

            return RedirectToAction("ManageAppointmentsByDoctor");
        }


        //MANAGE REPORTS

        [HttpGet]
        public ActionResult ManageReports()
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }

        //POST: UploadScan to ScanDetails DB
        [HttpPost]
        public ActionResult UploadScan(UploadScanViewModel model)
        {

            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Upload Scan Model called");
                try
                {
                    if (model.UploadFile != null && model.UploadFile.ContentLength > 0)
                    {

                        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                        var fileExtension = Path.GetExtension(model.UploadFile.FileName).ToLower();

                        if (!allowedExtensions.Contains(fileExtension))
                        {
                            ModelState.AddModelError("", "Only .jpg, .jpeg, .png, and .gif file types are allowed.");
                            System.Diagnostics.Debug.WriteLine("File type not supported");
                            return View(model);
                        }


                        var fileName = $"{Guid.NewGuid()}{fileExtension}";
                        var path = Path.Combine(Server.MapPath("~/ScanUploads"), fileName);

                        model.UploadFile.SaveAs(path);

                        var currentUserId = User.Identity.GetUserId();

                        var scanImage = new ScanDetails
                        {
                            ScanID = Guid.NewGuid().ToString(),
                            FileName = model.ScanName,
                            FilePath = $"~/ScanUploads/{fileName}",
                            DateTaken = model.DateTaken,
                            UploadBy_UserId = currentUserId,
                            Patient_PatientID = model.SelectedPatientId,
                            DoctorsNotes = model.DoctorsNotes
                            
                            
                        };


                        OmniContext.ScanImages.Add(scanImage);
                        OmniContext.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Upload Done");
                        return RedirectToAction("UploadSuccess", "Doctor");

                    }
                    else
                    {
                        ModelState.AddModelError("", "Please select a correct file.");
                    }
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {

                            System.Diagnostics.Debug.WriteLine("Property: {0} Error: {1}",
                                validationError.PropertyName,
                                validationError.ErrorMessage);

                            ModelState.AddModelError(validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                    ModelState.AddModelError("", "Error uploading file: " + ex.Message);
                }


            }
            model.Patients = OmniContext.Patients
                .Select(p => new SelectListItem
                {
                    Value = p.PatientID.ToString(),
                    Text = p.FirstName + " " + p.LastName
                }).ToList();

            return View(model);
        }


        public ActionResult UploadScan()
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var viewUploadScanModel = new UploadScanViewModel();

            viewUploadScanModel.Patients = OmniContext.Patients
                                        .Select(p => new SelectListItem
                                        {
                                            Value = p.PatientID.ToString(),
                                            Text = p.FirstName + " " + p.LastName
                                        }).ToList();

            return View(viewUploadScanModel);
        }

        public JsonResult GetPatients(string searchTerm)
        {
            var patients = OmniContext.Patients
                            .Where(p => p.LastName.Contains(searchTerm))
                            .Select(p => new { id = p.PatientID, text = p.FirstName + " " + p.LastName })
                            .ToList();

            return Json(patients, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UploadSuccess()
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }


        public ActionResult ViewReport()
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var currentUserId = User.Identity.GetUserId();
            var scans = OmniContext.ScanImages.Where(s => s.UploadBy_UserId == currentUserId).ToList();
            return View(scans);
        }

        public ActionResult ViewReportDetails(string id)
        {
            if (!User.IsInRole("Doctor"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            var scanDetails = OmniContext.ScanImages.Include(s => s.Patient).FirstOrDefault(s => s.ScanID == id);
            if (scanDetails == null)
            {
                return HttpNotFound(); 
            }
            return View(scanDetails);
        }

        









    }
}