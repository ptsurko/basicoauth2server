namespace OAuth2Server.Controllers
{
    using System;
    using System.Net;
    using System.Web.Mvc;

    using DotNetOpenAuth.OAuth2;
    
    /// <summary>
    /// This controller will server as a test client, allowing the user
    /// </summary>
    //[RequireHttps]
    public class HomeController : Controller
    {
        /// <summary>
        /// This action will show a short introduction on what can be done with this website.
        /// </summary>
        /// <returns>The view result.</returns>
        [HttpGet]
        public ViewResult Index()
        {
            return this.View();
        }
    }
}