using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class VenueController : Controller
    {
        //db connection
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        //load venue maintainence screen
        public ActionResult venueMaintainence()
        {
            ViewBag.Campuses = from c in db.Campus
                               select c;
            return View();
        }    

        //get list of building floors
        public PartialViewResult getBuildingFloors(int id)
        {
            var buildingFloors = from b in db.Building_Floor
                                 where b.Building_ID == id
                                 select b;

            var idHolder = (Venue)Session["idHolder"];
            idHolder.Building_ID = id;
            Session["idHolder"] = idHolder;

            return PartialView(buildingFloors);
        }

        //get list of venues
        public PartialViewResult getVenues(int id)
        {
            var venues = from v in db.Venues
                         where v.Building_Floor_ID == id
                         select v;

            var idHolder = (Venue)Session["idHolder"];
            idHolder.Building_Floor_ID = id;
            Session["idHolder"] = idHolder;

            return PartialView(venues);
        }

        //capture venue selection
        public void captureVenue(int id)
        {
            var idHolder = (Venue)Session["idHolder"];
            idHolder.Venue_ID = id;
            Session["idHolder"] = idHolder;
        }

        //add a campus - get
        public PartialViewResult addCampus()
        {
            return PartialView();
        }

        //add a campus - post
        [HttpPost]
        public void addCampus(string campus)
        {
            var a = campus.ToUpper();
            a = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(a.ToLower());
            Campus newCampus = new Campus();
            newCampus.Campus_Name = a;
            db.Campus.Add(newCampus);
            db.SaveChanges();
        }

        //check campus exists already
        [HttpGet]
        public JsonResult checkCampus(string campusName)
        {
            var campuses = db.Campus.ToList();
            var result = campuses.Any(c => c.Campus_Name.ToLower() == campusName.ToLower());

            if (result)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            } 
            else
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

        }

        //delete campus - get
        public PartialViewResult deleteCampus(string campusName)
        {
            var campus = db.Campus.Where(c => c.Campus_Name == campusName).FirstOrDefault();
            Venue idHolder = new Venue();
            idHolder.Campus_ID = campus.Campus_ID;
            Session["idHolder"] = idHolder;
            return PartialView(campus);
        }

        //delete campus - post
        [HttpPost]
        public string confirmDelete()
        {
            var idHolder = (Venue)Session["idHolder"];

            //check if campus has existing buildings
            var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID);
            
            if (buildings.Any())
            {
                return "CLASH";
            }
            else
            {
                var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
                var campusName = campus.Campus_Name;
                db.Campus.Attach(campus);
                db.Campus.Remove(campus);
                db.SaveChanges();

                return campusName;
            }   
        }

        //update campus - get
        public PartialViewResult updateCampus(string campusName)
        {
            var campus = db.Campus.Where(c => c.Campus_Name == campusName).FirstOrDefault();
            Venue idHolder = new Venue();
            idHolder.Campus_ID = campus.Campus_ID;
            Session["idHolder"] = idHolder;
            return PartialView(campus);
        }

        //update campus - post
        [HttpPost]
        public string captureUpdateCampus(string campusName)
        {
            var idHolder = (Venue)Session["idHolder"];
            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            var oldName = campus.Campus_Name;

            var a = campusName.ToUpper();
            a = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(a.ToLower());

            campus.Campus_Name = a;
            db.Entry(campus).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            return oldName;
        }

        //check updated campus
        public JsonResult checkUpdateCampus(string Campus_Name)
        {
            var idHolder = (Venue)Session["idHolder"];

            var a = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).First();

            var campuses = db.Campus.ToList();

            var campus = campuses.Where(c => c.Campus_Name.ToLower() == Campus_Name.ToLower() & c.Campus_Name.ToLower() != a.Campus_Name.ToLower());

            if (campus.Any())
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //get buildings matching selected campus
        public PartialViewResult getBuildings(string campusName)
        {
            var campus = db.Campus.Where(c => c.Campus_Name == campusName).FirstOrDefault();
            Venue idHolder = new Venue();
            idHolder.Campus_ID = campus.Campus_ID;

            var buildings = (from b in db.Buildings
                            where b.Campus_ID == idHolder.Campus_ID
                            select b).ToList();

            return PartialView(buildings);
        }
    }
}