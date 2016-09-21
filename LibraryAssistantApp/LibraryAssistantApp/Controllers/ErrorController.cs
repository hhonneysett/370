using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class ErrorController : Controller
    {
        // GET: Error
        public ActionResult error()
        {
            return View();
        }
    }
}