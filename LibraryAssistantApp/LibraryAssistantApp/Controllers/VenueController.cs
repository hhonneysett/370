using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class VenueController : Controller
    {
        //initialise an instance of the database
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: View existing campus
        public ActionResult ViewCampus()
        {
            var existingCampus = from c in db.Campus
                                 select c;
            return View(existingCampus);
        }

        // GET: Add Campus
        [HttpGet]
        public ActionResult AddCampus()
        {
            return View();
        }

        // POST: Add Campus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddCampus(CampusModel model)
        {
            if (ModelState.IsValid)
            {
                //check if campus already exists
                var campusExists = db.Campus.Any(c => c.Campus_Name.ToLower().Equals(model.Campus_Name.ToLower()));
                if (campusExists)
                {
                    TempData["Message"] = "Campus already exists";
                    TempData["classStyle"] = "warning";
                    ModelState.Clear();
                    return View();
                }
                else
                {
                    //add campus to database
                    Campus campus = new Campus
                    {
                        Campus_Name = model.Campus_Name,
                    };
                    db.Campus.Add(campus);
                    db.SaveChanges();
                    TempData["Message"] = "Campus successfuly added";
                    TempData["classStyle"] = "success";
                    return RedirectToAction("ViewCampus");
                }
            }           
            return View(model);
        }

        // GET: Update Campus
        [HttpGet]
        public ActionResult UpdateCampus(int id)
        {
            var selectedCampus = db.Campus.Where(c => c.Campus_ID.Equals(id)).FirstOrDefault();
            CampusModel a = new CampusModel
            {
                Campus_Name = selectedCampus.Campus_Name,
                Campus_ID = selectedCampus.Campus_ID,
            };
            return View(a);
        }

        // POST: Update Campus
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateCampus(CampusModel model)
        {
            if (ModelState.IsValid)
            {
                Campus a = db.Campus.Where(c => c.Campus_ID.Equals(model.Campus_ID)).FirstOrDefault();
                a.Campus_Name = model.Campus_Name;
                db.Entry(a).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "Campus successfuly updated";
                TempData["classStyle"] = "success";
                return RedirectToAction("ViewCampus");
            }
            else
            {
                return View(model);
            }
                      
        }

        // GET: Delete Campus
        [HttpGet]
        public ActionResult DeleteCampus(int id)
        {
            var campus = db.Campus.Where(c => c.Campus_ID.Equals(id)).FirstOrDefault();
            return View(campus);
        }
    }
}