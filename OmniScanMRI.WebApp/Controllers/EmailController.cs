using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using SendGrid;
using SendGrid.Helpers.Mail;
using OmniScanMRI.WebApp.Models;
using System.Data.Entity.Validation;
using Azure;

namespace OmniScanMRI.WebApp.Controllers
{
    public class EmailController : Controller
    {
        // GET: SendEmail View
        public ActionResult SendEmail()
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }

        private OmniScanContext OmniContext = new OmniScanContext();
        private readonly string sendGridApiKey = Environment.GetEnvironmentVariable("SENDGRID_API_KEY", EnvironmentVariableTarget.User);

        //POST: SendEmail transaction to send email
        [HttpPost]
        public async Task<ActionResult> Send(SendEmailModel model)
        {
            if (ModelState.IsValid)
            {
                // Getting the current logged-in user's Id
                var userId = User.Identity.GetUserId();
                
                var admin = OmniContext.Administrators.FirstOrDefault(a => a.AdminID.Equals(userId));
                System.Diagnostics.Debug.WriteLine(admin);
                var aspUser = admin;
                System.Diagnostics.Debug.WriteLine(aspUser);
                var emailLog = new TrackEmail
                {
                    TrackID = Guid.NewGuid().ToString(),
                    ToEmail = model.ToEmail,
                    Subject = model.Subject,
                    Content = model.Content,
                    AdminID = userId, 
                    SentDate = DateTime.Now,
                    CCEmail = model.CcEmail
                };

                try
                {
                    OmniContext.EmailLogs.Add(emailLog);
                    OmniContext.SaveChanges();
                }
                catch (DbEntityValidationException ex)
                {
                    var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.PropertyName + ": " + x.ErrorMessage);
                    var fullErrorMessage = string.Join("; ", errorMessages);
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                    System.Diagnostics.Debug.WriteLine(exceptionMessage);  // or log it
                    throw;
                }

                var client = new SendGridClient(sendGridApiKey);
                var from = new EmailAddress(aspUser.Email, "OmniScanMRI");
                var to = new EmailAddress(model.ToEmail);

                var msg = new SendGridMessage()
                {
                    From = from,
                    Subject = model.Subject,
                    PlainTextContent = model.Content,
                    HtmlContent = model.Content
                };

                msg.AddTo(to);
               
                if (model.FileAttachment != null && model.FileAttachment.ContentLength > 0)
                {
                    byte[] fileData = new byte[model.FileAttachment.InputStream.Length];
                    model.FileAttachment.InputStream.Read(fileData, 0, (int)model.FileAttachment.InputStream.Length);
                    var file = Convert.ToBase64String(fileData);
                    var mimeType = model.FileAttachment.ContentType;
                    msg.AddAttachment(model.FileAttachment.FileName, file, mimeType);
                }

                var response = await client.SendEmailAsync(msg);


                System.Diagnostics.Debug.WriteLine(response.StatusCode);
                System.Diagnostics.Debug.WriteLine(response.Body.ReadAsStringAsync().Result);

                return RedirectToAction("EmailSuccess");

            }

            
            return View(model);
        }

        public ActionResult EmailSuccess()
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            return View();
        }


        [HttpPost]
        public async Task<ActionResult> SendBulkEmailToPatients(SendBulkEmailModel model)
        {
            System.Diagnostics.Debug.WriteLine("Bulk Email Post called");
            if (ModelState.IsValid)
            {
                System.Diagnostics.Debug.WriteLine("Bulk Email Model Validated");

                var userId = User.Identity.GetUserId();
                var admin = OmniContext.Administrators.FirstOrDefault(a => a.AdminID.Equals(userId));
                var aspUser = admin;

                foreach (var patientEmail in model.ToEmails)
                {
                    var patient = OmniContext.Patients.FirstOrDefault(p => p.Email.Equals(patientEmail));

                    if (patient == null)
                    {
                        continue;
                    }

                    var emailLog = new TrackEmail
                    {
                        TrackID = Guid.NewGuid().ToString(),
                        ToEmail = patient.Email,
                        Subject = model.Subject,
                        Content = model.Content,
                        AdminID = userId,
                        SentDate = DateTime.Now,
                        CCEmail = model.CcEmail 
                    };

                    try
                    {
                        OmniContext.EmailLogs.Add(emailLog);
                        OmniContext.SaveChanges();
                        System.Diagnostics.Debug.WriteLine("Bulk Email changes saved");
                    }
                    catch (DbEntityValidationException ex)
                    {
                        var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.PropertyName + ": " + x.ErrorMessage);
                        var fullErrorMessage = string.Join("; ", errorMessages);
                        var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                        System.Diagnostics.Debug.WriteLine(exceptionMessage);
                        throw;
                    }

                    var client = new SendGridClient(sendGridApiKey);
                    var from = new EmailAddress(aspUser.Email, "OmniScanMRI");
                    var to = new EmailAddress(patient.Email);
                    var msg = MailHelper.CreateSingleEmail(from, to, model.Subject, model.Content, model.Content);
                    var response = await client.SendEmailAsync(msg);

                    System.Diagnostics.Debug.WriteLine(response.StatusCode);
                    System.Diagnostics.Debug.WriteLine(response.Body.ReadAsStringAsync().Result);
                }

                return RedirectToAction("BulkEmailSuccess");
            }
            else
            {
                foreach (var modelStateKey in ModelState.Keys)
                {
                    var modelStateVal = ModelState[modelStateKey];
                    foreach (var error in modelStateVal.Errors)
                    {
                        System.Diagnostics.Debug.WriteLine($"Key: {modelStateKey}, Error: {error.ErrorMessage}");
                    }
                }
                return View(model);
            }
        }


        public ActionResult SendBulkToPatients()
        {
            if (!User.IsInRole("Administrator"))
            {
                return RedirectToAction("UnauthorizedAccess", "Account");
            }
            List<string> testEmails = new List<string>
            {
                "naveenkhokher@hotmail.com",
                "everme.connect@gmail.com"
            };

            List<string> patientEmails = OmniContext.Patients
                .Where(p => testEmails.Contains(p.Email))
                .Select(p => p.Email)
                .ToList();

            var model = new SendBulkEmailModel
            {
                ToEmails = patientEmails
            };

            return View(model);
        }




    }
}