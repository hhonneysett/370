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

            Session["idHolder"] = idHolder;

            var buildings = (from b in db.Buildings
                            where b.Campus_ID == idHolder.Campus_ID
                            select b).ToList();

            return PartialView(buildings);
        }

        //add building - get
        public PartialViewResult addBuilding()
        {
            var idHolder = (Venue)Session["idHolder"];
            var campuses = db.Campus.ToList();
            var campus = campuses.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            ViewBag.Campus = campus;
            return PartialView();
        }

        //add building - post
        [HttpPost]
        public JsonResult addBuilding(string buildingName)
        {
            var lastBuilding = db.Buildings.OrderByDescending(b => b.Building_ID).FirstOrDefault();
            var id = 0;
            if (lastBuilding == null)
            {
                id = 1;
            }
            else
            {
                id = lastBuilding.Building_ID + 1;
            }

            var idHolder = (Venue)Session["idHolder"];
            var building = new Building();

            var a = buildingName.ToUpper();
            a = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(a.ToLower());

            building.Building_Name = a;
            building.Campus_ID = idHolder.Campus_ID;
            building.Building_ID = id;
            db.Buildings.Add(building);
            db.SaveChanges();

            var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID).ToList();

            var jsonObj = from b in buildings
                          select new
                          {
                              id = b.Building_ID,
                              name = b.Building_Name,
                          };

            var rows = jsonObj.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //check building
        public JsonResult checkBuilding(string buildingName)
        {
            var idHolder = (Venue)Session["idHolder"];

            var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID).ToList();

            var building = buildings.Where(b => b.Building_Name.ToLower() == buildingName.ToLower()).ToList();

            if (building.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //delete building - get
        public PartialViewResult deleteBuilding(int id)
        {
            var idHolder = (Venue)Session["idHolder"];

            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();

            var building = db.Buildings.Where(b => b.Building_ID == id).FirstOrDefault();

            ViewBag.Campus = campus;
            ViewBag.Building = building;

            idHolder.Building_ID = building.Building_ID;
            Session["idHolder"] = idHolder;

            return PartialView();
        }

        //delete building - post
        [HttpPost]
        public JsonResult deleteBuilding()
        {
            var idHolder = (Venue)Session["idHolder"];

            var buildingFloors = db.Building_Floor.Where(b => b.Building_ID == idHolder.Building_ID);

            if (buildingFloors.Any())
                return Json("CLASH", JsonRequestBehavior.AllowGet);
            else
            {
                var building = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();

                var buildingID = building.Building_ID;

                db.Buildings.Attach(building);
                db.Buildings.Remove(building);
                db.SaveChanges();

                var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID).ToList();
                var jsonObj = from b in buildings
                              select new
                              {
                                  id = b.Building_ID,
                                  name = b.Building_Name,
                              };
                var rows = jsonObj.ToArray();

                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }

        //update building - get
        public PartialViewResult updateBuilding(int id)
        {
            var campuses = db.Campus.ToList();
            var building = db.Buildings.Where(b => b.Building_ID == id).FirstOrDefault();

            var idHolder = (Venue)Session["idHolder"];
            idHolder.Building_ID = id;
            Session["idHolder"] = idHolder;

            ViewBag.Campuses = campuses;

            return PartialView(building);
        }

        //update building - post
        [HttpPost]
        public JsonResult updateBuilding(string buildingName, int campusID)
        {
            var idHolder = (Venue)Session["idHolder"];

            var building = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();

            building.Campus_ID = campusID;
            building.Building_Name = buildingName;

            db.Entry(building).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID).ToList();

            var jsonObj = from b in buildings
                          select new
                          {
                              id = b.Building_ID,
                              name = b.Building_Name,
                          };

            var rows = jsonObj.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //check updated building
        public JsonResult checkUpdateBuilding(string Building_Name)
        {
            var idHolder = (Venue)Session["idHolder"];
            var oldBuilding = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();

            var buildings = db.Buildings.ToList();

            var building = buildings.Where(b => b.Building_Name.ToLower() == Building_Name.ToLower() && b.Building_Name != oldBuilding.Building_Name);

            if (building.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //get building floors
        public PartialViewResult getBuildingFloors(int id)
        {
            var buildingFloors = db.Building_Floor.Where(b => b.Building_ID == id).ToList();
            var idHolder = (Venue)Session["idHolder"];
            idHolder.Building_ID = id;
            Session["idHolder"] = idHolder;
            return PartialView(buildingFloors);
        }

        //add building floors - get
        public PartialViewResult addBuildingFloor()
        {
            var idHolder = (Venue)Session["idHolder"];

            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            var building = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();

            ViewBag.Campus = campus;
            ViewBag.Building = building;

            return PartialView();
        }

        //add building floor - post
        [HttpPost]
        public JsonResult addBuildingFloor(string floorName)
        {
            var idHolder = (Venue)Session["idHolder"];

            var lastBuildingFloor = db.Building_Floor.OrderByDescending(b => b.Building_Floor_ID).FirstOrDefault();
            var id = 0;
            if (lastBuildingFloor == null)
            {
                id = 1;
            }
            else
            {
                id = lastBuildingFloor.Building_Floor_ID + 1;
            }

            var building_floor = new Building_Floor();

            building_floor.Building_Floor_ID = id;
            building_floor.Campus_ID = idHolder.Campus_ID;
            building_floor.Building_ID = idHolder.Building_ID;
            building_floor.Floor_Name = floorName;

            db.Building_Floor.Add(building_floor);
            db.SaveChanges();

            var floors = db.Building_Floor.Where(f => f.Building_ID == idHolder.Building_ID).ToList();
            var jsonObj = from f in floors
                          select new
                          {
                              id = f.Building_Floor_ID,
                              name = f.Floor_Name,
                          };
            var rows = jsonObj.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //check building floor
        public JsonResult checkFloor(string floorName)
        {
            var idHolder = (Venue)Session["idHolder"];

            var floors = db.Building_Floor.Where(f => f.Building_ID == idHolder.Building_ID).ToList();

            var floor = floors.Where(f => f.Floor_Name.ToLower() == floorName.ToLower());

            if (floor.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //delete building floor - get
        public PartialViewResult deleteFloor(int id)
        {
            var idHolder = (Venue)Session["idHolder"];

            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            var building = db.Buildings.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            var buildingFloor = db.Building_Floor.Where(f => f.Building_Floor_ID == id).FirstOrDefault();

            idHolder.Building_Floor_ID = id;
            Session["idHolder"] = idHolder;

            ViewBag.Campus = campus;
            ViewBag.Building = building;
            ViewBag.BuildingFloor = buildingFloor;

            return PartialView();
        }

        //delete building floor - post
        [HttpPost]
        public JsonResult deleteFloor()
        {
            var idHolder = (Venue)Session["idHolder"];

            var venues = db.Venues.Where(v => v.Building_Floor_ID == idHolder.Building_ID);

            if (venues.Any())
                return Json("CLASH", JsonRequestBehavior.AllowGet);
            else
            {
                var building_floor = db.Building_Floor.Where(f => f.Building_Floor_ID == idHolder.Building_Floor_ID).FirstOrDefault();
                db.Building_Floor.Attach(building_floor);
                db.Building_Floor.Remove(building_floor);
                db.SaveChanges();

                var buildingFloors = db.Building_Floor.Where(f => f.Building_ID == idHolder.Building_ID).ToList();
                var jsonObj = from f in buildingFloors
                              select new
                              {
                                  id = f.Building_Floor_ID,
                                  name = f.Floor_Name,
                              };
                var rows = jsonObj.ToArray();
                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }

        //update building floor - get
        public PartialViewResult updateFloor(int id)
        {
            var idHolder = (Venue)Session["idHolder"];

            var campuses = db.Campus.ToList();
            var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID).ToList();
            var buildingFloor = db.Building_Floor.Where(f => f.Building_Floor_ID == id).FirstOrDefault();

            idHolder.Building_Floor_ID = id;
            Session["idHolder"] = idHolder;

            ViewBag.Campuses = campuses;
            ViewBag.Buildings = buildings;

            return PartialView(buildingFloor);
        }

        //update building floor - post
        [HttpPost]
        public JsonResult updateBuildingFloor(string floorName, int campus, int building)
        {
            var idHolder = (Venue)Session["idHolder"];

            var buildingFloor = db.Building_Floor.Where(f => f.Building_Floor_ID == idHolder.Building_Floor_ID).FirstOrDefault();

            buildingFloor.Floor_Name = floorName;
            buildingFloor.Campus_ID = campus;
            buildingFloor.Building_ID = building;

            db.Entry(buildingFloor).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            var floors = db.Building_Floor.Where(f => f.Building_ID == idHolder.Building_ID).ToList();
            var jsonObj = from f in floors
                          select new
                          {
                              id = f.Building_Floor_ID,
                              name = f.Floor_Name,
                          };
            var rows = jsonObj.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //check update floor
        public JsonResult checkUpdateFloor(string Floor_Name, int campus, int building)
        {
            var idHolder = (Venue)Session["idHolder"];
            var oldFloor = db.Building_Floor.Where(f => f.Building_Floor_ID == idHolder.Building_Floor_ID).FirstOrDefault();
            
            var floors = db.Building_Floor.Where(f => f.Building_ID == building);

            var floor = floors.Where(f => f.Floor_Name.ToLower() == Floor_Name.ToLower() && f.Floor_Name.ToLower() != oldFloor.Floor_Name.ToLower());

            if (floor.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //get campus buildings
        public JsonResult getCamBuildings(int id)
        {
            var buildings = db.Buildings.Where(b => b.Campus_ID == id).ToList();
            var jsonObj = from b in buildings
                          select new
                          {
                              id = b.Building_ID,
                              text = b.Building_Name
                          };
            var rows = jsonObj.ToArray();
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //get venues for selected building floor
        public PartialViewResult getVenues(int id)
        {
            var idHolder = (Venue)Session["idHolder"];
            idHolder.Building_Floor_ID = id;
            Session["idHolder"] = idHolder;

            var venues = db.Venues.Where(v => v.Building_Floor_ID == id).ToList();

            return PartialView(venues);
        }

        //add venue - get
        public PartialViewResult addVenue()
        {
            var idHolder = (Venue)Session["idHolder"];

            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            var building = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();
            var building_floor = db.Building_Floor.Where(f => f.Building_Floor_ID == idHolder.Building_Floor_ID).FirstOrDefault();
            var characteristics = db.Characteristics.ToList();

            ViewBag.Campus = campus;
            ViewBag.Building = building;
            ViewBag.Building_Floor = building_floor;
            ViewBag.Characteristics = characteristics;

            return PartialView();
        }

        //check venue
        public JsonResult checkVenue(string venueName)
        {
            var idHolder = (Venue)Session["idHolder"];

            var venues = db.Venues.Where(v => v.Building_Floor_ID == idHolder.Building_Floor_ID);

            var venue = venues.Where(v => v.Venue_Name.ToLower() == venueName.ToLower());

            if (venue.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}