using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace OAuth2WebClient
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // DotNetOpenAuth only issues access tokens when the client uses an HTTPS connection. As we will most
            // likely run the server on our local development machine with only a self-signed SSL certificate, setting up 
            // connection to the server will fail as the SSL certificate is considered invalid by the .NET framework. 
            // To circumvent this, we add the line below that will consider all SSL certificates as valid, including
            // self-signed certificaties. Note: this should only be used for testing purposes.
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, errors) => true;

            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}