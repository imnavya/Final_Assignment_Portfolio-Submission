using OmniScanMRI.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Windows.Forms;

namespace OmniScanMRI.WebApp.Controllers
{
    public class HomeController : Controller
    {
        
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public async Task<ActionResult> ProxyMapboxRequest(string query, string apiKey)
        {
            using (var client = new HttpClient())
            {
                var url = $"https://api.mapbox.com/{query}";
                System.Diagnostics.Debug.WriteLine(url);
                var response = await client.GetAsync(url);
                return Content(await response.Content.ReadAsStringAsync(), "application/json");
            }
        }


        [AllowAnonymous]
        public ActionResult Contact()
        {
            ViewBag.Message = "We are here to help!";
            string mapboxApiKey = Environment.GetEnvironmentVariable("MB_PUB_API_KEY", EnvironmentVariableTarget.User);
            ViewBag.MapboxApiKey = mapboxApiKey;
            System.Diagnostics.Debug.WriteLine("API Key: " + mapboxApiKey);
            return View();
        }

        [AllowAnonymous]
        public ActionResult CustomerSupport()
        {
            ViewBag.Message = "Omni Scan MRI Customer Support - GET IN TOUCH WITH US!";

            return View();
        }

        
    }
}