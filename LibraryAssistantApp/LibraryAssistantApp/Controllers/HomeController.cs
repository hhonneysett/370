using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class HomeController : Controller
    {

        [AllowAnonymous] //this is for un-authorized users
        public ActionResult Index()
        {
            return View();
        }

        [Authorize] //this is for authorized users
        public ActionResult MyProfile()
        {
            return View();
        }

        [Authorize(Roles ="Admin")]
        public ActionResult AdminIndex()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult StudentIndex()
        {
            return View();
        }
    }
}