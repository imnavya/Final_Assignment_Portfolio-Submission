using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security;
using Owin;
using System;
using System.Threading.Tasks;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Linq;
using System.Security.Claims;

[assembly: OwinStartupAttribute(typeof(OmniScanMRI.WebApp.Startup))]
namespace OmniScanMRI.WebApp
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
            ConfigureAuth(app);
        }
    }
}
