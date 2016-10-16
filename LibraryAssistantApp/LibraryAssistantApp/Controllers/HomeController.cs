using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LibraryAssistantApp.Controllers
{
    public class HomeController : Controller
    {

        LibraryAssistantEntities db = new LibraryAssistantEntities();

        [Authorize]
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
                return RedirectToAction("Index", "Dashboard");
            return View();                  
        }

        //get pictures
        public JsonResult getPictures()
        {
            //access xml
            var path = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement settings = XElement.Load(path);

            //get pictures
            IEnumerable<XElement> pictures = from el in settings.Elements("picture")
                                             select el;

            List<string> spictures = new List<string>();

            foreach (var picture in pictures)
            {
                var link = picture.Value;
                var filename = Path.GetFileName(link);
                var spath = "/img/" + filename;
                spictures.Add(spath);
            }

            return Json(spictures.ToArray(), JsonRequestBehavior.AllowGet);
        }
    }
}