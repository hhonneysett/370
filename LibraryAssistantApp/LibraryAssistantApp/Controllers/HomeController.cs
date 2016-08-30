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
            
            return View();                  
        }
    }
}