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

        //get list of buildings
        public PartialViewResult getBuildings(int id)
        {
            var buildings = from b in db.Buildings
                            where b.Campus_ID == id
                            select b;

            //create session variable to hold selected ids
            Venue idHolder = new Venue();
            idHolder.Campus_ID = id;
            Session["idHolder"] = idHolder;

            return PartialView(buildings);
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
    }
}