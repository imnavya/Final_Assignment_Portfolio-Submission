using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using OmniScanMRI.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OmniScanMRI.WebApp.Controllers
{
    
    public class AdministratorController : Controller
    {

        OmniScanContext OmniContext = new OmniScanContext();
        // GET: Administrator
        public ActionResult AdministratorHome()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }


        public ActionResult ManagePatients()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            var patients = OmniContext.Patients.ToList();

            var model = patients.Select(p => new Patient
            {
                PatientID = p.PatientID,
                FirstName = p.FirstName,
                LastName = p.LastName,
                DateOfBirth = p.DateOfBirth,
                Gender = p.Gender,
                Address = p.Address,
                ContactNumber = p.ContactNumber,
                Email = p.Email,
                MedicalHistory = p.MedicalHistory
            }).ToList();

            return View(model);
        }


        public ActionResult EditPatient(string id)
        {

            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            var patient = OmniContext.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            var model = new Patient
            {
                PatientID = patient.PatientID,
                FirstName = patient.FirstName,
                LastName = patient.LastName,
                DateOfBirth = patient.DateOfBirth,
                Gender = patient.Gender,
                Address = patient.Address,
                ContactNumber = patient.ContactNumber,
                Email = patient.Email,
                MedicalHistory = patient.MedicalHistory
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult EditPatient(Patient model)
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            if (ModelState.IsValid)
            {
                var patient = OmniContext.Patients.Find(model.PatientID);
                if (patient == null)
                {
                    return HttpNotFound();
                }

                patient.FirstName = model.FirstName;
                patient.LastName = model.LastName;
                patient.DateOfBirth = model.DateOfBirth;
                patient.Gender = model.Gender;
                patient.Address = model.Address;
                patient.ContactNumber = model.ContactNumber;
                patient.Email = model.Email;
                patient.MedicalHistory = model.MedicalHistory;

                OmniContext.SaveChanges();

                return RedirectToAction("ManagePatients");
            }

            return View(model);
        }

        public ActionResult DeletePatient(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            var patient = OmniContext.Patients.Find(id);
            if (patient == null)
            {
                return HttpNotFound();
            }

            OmniContext.Patients.Remove(patient);
            OmniContext.SaveChanges();

            return RedirectToAction("ManagePatients");
        }

        public ActionResult AddPatient()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }

        [HttpPost]
        public ActionResult AddPatient(Patient model)
        {
            if (ModelState.IsValid)
            {
                var patient = new Patient
                {
                    PatientID = model.PatientID,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    DateOfBirth = model.DateOfBirth,
                    Gender = model.Gender,
                    Address = model.Address,
                    ContactNumber = model.ContactNumber,
                    Email = model.Email,
                    MedicalHistory = model.MedicalHistory
                };

                OmniContext.Patients.Add(patient);
                OmniContext.SaveChanges();

                return RedirectToAction("ManagePatients");
            }

            return View(model);
        }

        //DOCTORS

        public ActionResult ManageDoctors()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            var doctors = OmniContext.Doctors.ToList();
            var model = doctors.Select(d => new Doctors
            {
                DoctorID = d.DoctorID,
                FirstName = d.FirstName,
                LastName = d.LastName,
                Specialization = d.Specialization,
                LicenseNumber = d.LicenseNumber,
                ContactNumber = d.ContactNumber,
                Email = d.Email
            }).ToList();

            return View(model);
        }

        [HttpPost]
        public ActionResult AddDoctor(Doctors model)
        {
            var doctor = new Doctors
            {
                
                DoctorID = model.DoctorID,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Specialization = model.Specialization,
                LicenseNumber = model.LicenseNumber,
                ContactNumber = model.ContactNumber,
                Email = model.Email
            };
            OmniContext.Doctors.Add(doctor);
            OmniContext.SaveChanges();
            return RedirectToAction("ManageDoctors");
        }

        public ActionResult EditDoctor(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctor = OmniContext.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditDoctor([Bind(Include = "DoctorID,FirstName,LastName,Specialization,LicenseNumber,ContactNumber,Email")] Doctors doctor)
        {

            if (ModelState.IsValid)
            {
                OmniContext.Entry(doctor).State = EntityState.Modified;
                OmniContext.SaveChanges();
                return RedirectToAction("ManageDoctors");
            }
            return View(doctor);
        }

        public ActionResult ViewDoctor(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Doctors doctor = OmniContext.Doctors.Find(id);
            if (doctor == null)
            {
                return HttpNotFound();
            }
            return View(doctor);
        }

        //public ActionResult DeleteDoctor(string id)
        //{
        //    if (!User.IsInRole("Administrator"))
        //    {
        //        if (User.IsInRole("Patient"))
        //        {
        //            return RedirectToAction("PatientHome", "Patient");
        //        }
        //        else if (User.IsInRole("Doctor"))
        //        {
        //            return RedirectToAction("DoctorHome", "Doctor");
        //        }

        //        return RedirectToAction("UnauthorizedAccess", "Account");
        //    }

        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Doctors doctor = OmniContext.Doctors.Find(id);
        //    if (doctor == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(doctor);
        //}

        
        public ActionResult DeleteDoctor(string id)
        {
            Doctors doctor = OmniContext.Doctors.Find(id);
            OmniContext.Doctors.Remove(doctor);
            OmniContext.SaveChanges();
            return RedirectToAction("ManageDoctors");
        }

        public ActionResult UserManagement()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            return View();
        }

        public ActionResult BookAppointmentByAdmin()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            var viewModel = new Appointments
            {
                DoctorsList = OmniContext.Doctors.Select(d => new SelectListItem
                {
                    Value = d.DoctorID.ToString(),
                    Text = d.FirstName + " " + d.LastName
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookAppointmentByAdmin(Appointments model)
        {
            try
            {
                var userStore = new UserStore<ApplicationUser>(OmniContext);
                var userManager = new UserManager<ApplicationUser>(userStore);

                // Create a placeholder user in AspNetUsers
                var newUser = new ApplicationUser
                {
                    // Populate required fields for the user. Adjust as needed.
                    UserName = model.Email,  // Assuming the model has an Email property
                    Email = model.Email,
                    // Add other properties if required
                };

                // Add the user and check for success
                IdentityResult result = userManager.Create(newUser);
                if (!result.Succeeded)
                {
                    // Handle the error, log it and return an error view
                    System.Diagnostics.Debug.WriteLine($"Error creating user: {result.Errors.FirstOrDefault()}");
                    return View("Error", new HandleErrorInfo(new Exception(result.Errors.FirstOrDefault()), "ControllerName", "BookAppointmentByAdmin")); // Replace "ControllerName" with your actual controller's name
                }

                var newPatient = new Patient
                {
                    PatientID = newUser.Id,
                    Email = newUser.Email
                    // Populate other required fields for the patient
                };

                OmniContext.Patients.Add(newPatient);

                // Try saving the patient
                try
                {
                    OmniContext.SaveChanges();
                }
                catch (DbUpdateException ex)
                {
                    // Handle error
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    return View("Error", new { message = "Error creating patient." });
                }

                model.PatientID = newUser.Id;
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
                        AdminID = User.Identity.GetUserId()
                    };

                    OmniContext.Appointments.Add(appointment);
                    OmniContext.SaveChanges();

                    System.Diagnostics.Debug.WriteLine("Context Save Success");
                    return RedirectToAction("AppointmentSuccess","Appointment", new { id = appointment.AppointmentID });
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
                    model.DoctorsList = OmniContext.Doctors.Select(d => new SelectListItem
                    {
                        Value = d.DoctorID.ToString(),
                        Text = d.FirstName + " " + d.LastName
                    }).ToList();

                    return View(model);
                }
            }
            catch (DbUpdateException ex)
            {
                System.Diagnostics.Debug.WriteLine($"DbUpdateException: {ex.Message}");
                var innerEx = ex.InnerException;
                while (innerEx != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {innerEx.Message}");
                    innerEx = innerEx.InnerException; // Go to the next inner exception
                }
                // This will handle and log any validation errors
                var validationErrors = ex.Entries.SelectMany(e => e.GetValidationResult().ValidationErrors).Select(ve => ve.ErrorMessage);
                foreach (var validationError in validationErrors)
                {
                    System.Diagnostics.Debug.WriteLine($"ValidationError: {validationError}");
                }

                return Content("This is here");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                return View("Error", new HandleErrorInfo(ex, "Administrator", "BookAppointmentByAdmin"));  
            }


        }


        public ActionResult EditAppointmentByAdmin(string id) 
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            var appointment = OmniContext.Appointments.FirstOrDefault(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            var model = new Appointments
            {
                AppointmentID = appointment.AppointmentID,
                DoctorID = appointment.DoctorID,
                PatientID = appointment.PatientID,
                AdminID = appointment.AdminID,
                AppointmentDttm = appointment.AppointmentDttm,
                Notes = appointment.Notes,
                Status = appointment.Status,

                DoctorsList = OmniContext.Doctors.Select(d => new SelectListItem
                {
                    Value = d.DoctorID,
                    Text = d.FirstName + " " + d.LastName
                }).ToList()
            };

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditAppointmentByAdmin(Appointments appointment)
        {
            if (ModelState.IsValid)
            {
                OmniContext.Entry(appointment).State = EntityState.Modified;
                OmniContext.SaveChanges();
                return RedirectToAction("ManageAppointmentsByAdmin");
            }

            // Repopulating the DoctorsList if ModelState is invalid when returning to the view.
            appointment.DoctorsList = OmniContext.Doctors.Select(d => new SelectListItem
            {
                Value = d.DoctorID,
                Text = d.FirstName + " " + d.LastName
            }).ToList();

            return View(appointment);
        }


        public ActionResult CancelAppointmentByAdmin(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            Appointments appointment = OmniContext.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost, ActionName("CancelAppointmentByAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmAppointmentCancellation(string id)
        {
            Appointments appointment = OmniContext.Appointments.Find(id);
            appointment.Status = "Cancelled";
            OmniContext.SaveChanges();
            return RedirectToAction("ManageAppointmentsByAdmin");
        }

        public ActionResult DeleteAppointmentByAdmin(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            Appointments appointment = OmniContext.Appointments.Find(id);
            if (appointment == null)
            {
                return HttpNotFound();
            }
            return View(appointment);
        }

        [HttpPost, ActionName("DeleteAppointmentByAdmin")]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmAppointmentDelete(string id)
        {
            Appointments appointment = OmniContext.Appointments.Find(id);
            OmniContext.Appointments.Remove(appointment);
            OmniContext.SaveChanges();
            return RedirectToAction("ManageAppointmentsByAdmin");
        }

        public ActionResult ManageAppointmentsByAdmin()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            // Fetch all appointments from the database including related Patient and Doctor data
            var appointments = OmniContext.Appointments.Include(a => a.Patient).Include(a => a.Doctor).ToList();

            return View(appointments);
        }

        public ActionResult ViewAppointment(string id)
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            if (string.IsNullOrEmpty(id))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Fetch the appointment from the database including related Patient and Doctor data
            var appointment = OmniContext.Appointments.Include(a => a.Patient).Include(a => a.Doctor).FirstOrDefault(a => a.AppointmentID == id);

            if (appointment == null)
            {
                return HttpNotFound();
            }

            return View(appointment);
        }

        public JsonResult GetAppointmentsPerDoctor()
        {
            var appointmentData = OmniContext.Appointments
                                    .GroupBy(a => a.DoctorID)
                                    .Select(group => new
                                    {
                                        DoctorName = OmniContext.Doctors.FirstOrDefault(d => d.DoctorID == group.Key).FirstName,
                                        AppointmentCount = group.Count()
                                    })
                                    .Where(data => data.AppointmentCount > 0)
                                    .ToList();

            return Json(appointmentData, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DoctorAppointmentStatsChart()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }
        public ActionResult SendNotifications()
        {
            if (!User.IsInRole("Administrator"))
            {
                if (User.IsInRole("Patient"))
                {
                    return RedirectToAction("PatientHome", "Patient");
                }
                else if (User.IsInRole("Doctor"))
                {
                    return RedirectToAction("DoctorHome", "Doctor");
                }

                return RedirectToAction("UnauthorizedAccess", "Account");
            }

            return View();
        }

    }
}