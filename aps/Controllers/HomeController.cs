using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using aps.Models;

namespace aps.Controllers
{
    public class HomeController : Controller
    {
#if !DEBUG
        [Authorize]
#endif
        public ActionResult Index()
        {
            /*
            var manager = HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();            
            // Get the current logged in User and look up the user in ASP.NET Identity
            var task = manager.FindByNameAsync(User.Identity.Name);
            task.Wait(10000);
            var currentUser = task.Result;
            */

            ViewBag.UserName = User.Identity.Name;
            ViewBag.Copyright = "2015";
            return View();
        }
    }
}