using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class HomeController : Controller
    {

        LibraryAssistantEntities db = new LibraryAssistantEntities();

        [Authorize]
        public ActionResult Index()
        {
            var persons = db.Registered_Person.Where(p => p.Person_ID == User.Identity.Name);

            return View();                  
        }

        [Authorize(Roles ="Admin")]
        public ActionResult adminView()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult studentView()
        {
            return View();
        }

        [Authorize(Roles = "Student")]
        public ActionResult empView()
        {
            return View();
        }
    }
}