using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class VenueController : Controller
    {
        //db connection
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        [Authorize(Roles = "Admin")]
        //load venue maintainence screen
        public ActionResult venueMaintainence()
        {
            ViewBag.Campuses = from c in db.Campus
                               select c;
            return View();
        }    

        [Authorize(Roles = "Admin, Employee")]
        //capture venue selection
        public void captureVenue(int id)
        {
            var idHolder = (Venue)Session["idHolder"];
            idHolder.Venue_ID = id;
            Session["idHolder"] = idHolder;
        }

        [Authorize(Roles = "Admin")]
        //add a campus - get
        public PartialViewResult addCampus()
        {
            return PartialView();
        }

        //add a campus - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult addCampus(string campus)
        {
            var a = campus.ToUpper();
            a = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(a.ToLower());
            Campus newCampus = new Campus();
            newCampus.Campus_Name = a;
            db.Campus.Add(newCampus);
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Add Campus", "Create", User.Identity.Name);

            var campuses = db.Campus.ToList();
            var jsonObj = (from c in campuses
                           select new
                           {
                               id = c.Campus_ID,
                               name = c.Campus_Name
                           }).ToArray();
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        //check campus exists already
        [HttpGet]
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public PartialViewResult deleteCampus(int id)
        {
            var campus = db.Campus.Where(c => c.Campus_ID == id).FirstOrDefault();
            Venue idHolder = new Venue();
            idHolder.Campus_ID = campus.Campus_ID;
            Session["idHolder"] = idHolder;
            return PartialView(campus);
        }

        //delete campus - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult confirmDelete()
        {
            var idHolder = (Venue)Session["idHolder"];

            //check if campus has existing buildings
            var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID);
            
            if (buildings.Any())
            {
                return Json("CLASH", JsonRequestBehavior.AllowGet);
            }
            else
            {
                var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
                db.Campus.Attach(campus);
                db.Campus.Remove(campus);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Delete Campus", "Delete", User.Identity.Name);

                var campuses = db.Campus.ToList();
                var jsonObj = (from c in campuses
                               select new
                               {
                                   id = c.Campus_ID,
                                   name = c.Campus_Name,
                               }).ToArray();
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }  
        }

        //update campus - get
        [Authorize(Roles = "Admin")]
        public PartialViewResult updateCampus(int id)
        {
            var campus = db.Campus.Where(c => c.Campus_ID == id).FirstOrDefault();
            Venue idHolder = new Venue();
            idHolder.Campus_ID = campus.Campus_ID;
            Session["idHolder"] = idHolder;
            return PartialView(campus);
        }

        //update campus - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult captureUpdateCampus(string campusName)
        {
            var idHolder = (Venue)Session["idHolder"];
            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();

            var a = campusName.ToUpper();
            a = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(a.ToLower());

            campus.Campus_Name = a;
            db.Entry(campus).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Update Campus", "Update", User.Identity.Name);

            var campuses = db.Campus.ToList();
            var jsonObj = (from c in campuses
                           select new
                           {
                               id = c.Campus_ID,
                               name = c.Campus_Name,
                           }).ToArray();
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        //check updated campus
        [Authorize(Roles = "Admin")]
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
        [Authorize]
        public PartialViewResult getBuildings(int id)
        {
            var campus = db.Campus.Where(c => c.Campus_ID == id).FirstOrDefault();
            Venue idHolder = new Venue();
            idHolder.Campus_ID = campus.Campus_ID;

            Session["idHolder"] = idHolder;

            var buildings = (from b in db.Buildings
                            where b.Campus_ID == idHolder.Campus_ID
                            select b).ToList();

            return PartialView(buildings);
        }

        //add building - get
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public JsonResult addBuilding(string buildingName)
        {
            var idHolder = (Venue)Session["idHolder"];
            var building = new Building();

            var a = buildingName.ToUpper();
            a = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(a.ToLower());

            building.Building_Name = a;
            building.Campus_ID = idHolder.Campus_ID;
            db.Buildings.Add(building);
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Add Building", "Create", User.Identity.Name);

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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

                //record action
                global.addAudit("Venues", "Venues: Delete Building", "Delete", User.Identity.Name);

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public JsonResult updateBuilding(string buildingName, int campusID)
        {
            var idHolder = (Venue)Session["idHolder"];

            var building = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();

            building.Campus_ID = campusID;
            building.Building_Name = buildingName;

            db.Entry(building).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Update Building", "Update", User.Identity.Name);

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
        [Authorize(Roles = "Admin")]
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
        [Authorize]
        public PartialViewResult getBuildingFloors(int id)
        {
            var buildingFloors = (from f in db.Building_Floor
                                  where f.Building_ID == id
                                  select f).ToList();
            var idHolder = (Venue)Session["idHolder"];
            idHolder.Building_ID = id;
            Session["idHolder"] = idHolder;
            return PartialView(buildingFloors);
        }

        //add building floors - get
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public JsonResult addBuildingFloor(string floorName)
        {
            var idHolder = (Venue)Session["idHolder"];

            var building_floor = new Building_Floor();

            building_floor.Campus_ID = idHolder.Campus_ID;
            building_floor.Building_ID = idHolder.Building_ID;
            building_floor.Floor_Name = floorName;

            db.Building_Floor.Add(building_floor);
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Add Floor", "Create", User.Identity.Name);

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
        public JsonResult deleteFloor()
        {
            var idHolder = (Venue)Session["idHolder"];

            var venues = db.Venues.Where(v => v.Building_Floor_ID == idHolder.Building_Floor_ID);

            if (venues.Any())
                return Json("CLASH", JsonRequestBehavior.AllowGet);
            else
            {
                var building_floor = db.Building_Floor.Where(f => f.Building_Floor_ID == idHolder.Building_Floor_ID).FirstOrDefault();
                db.Building_Floor.Attach(building_floor);
                db.Building_Floor.Remove(building_floor);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Delete Floor", "Delete", User.Identity.Name);

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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize(Roles = "Admin")]
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
        [Authorize]
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
        [Authorize]
        public PartialViewResult getVenues(int id)
        {
            var idHolder = (Venue)Session["idHolder"];
            idHolder.Building_Floor_ID = id;
            Session["idHolder"] = idHolder;

            var venues = db.Venues.Where(v => v.Building_Floor_ID == id).ToList();

            return PartialView(venues);
        }

        //add venue - get
        [Authorize(Roles = "Admin")]
        public PartialViewResult addVenue()
        {
            var idHolder = (Venue)Session["idHolder"];

            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            var building = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();
            var building_floor = db.Building_Floor.Where(f => f.Building_Floor_ID == idHolder.Building_Floor_ID).FirstOrDefault();
            var types = db.Venue_Type.ToList();
            var characteristics = db.Characteristics.ToList();

            ViewBag.Campus = campus;
            ViewBag.Building = building;
            ViewBag.Building_Floor = building_floor;
            ViewBag.Characteristics = characteristics;
            ViewBag.Types = types;

            return PartialView();
        }

        //add venue - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult addVenue(string venueName, string characteristicsJson, string type, int capacity)
        {
            var idHolder = (Venue)Session["idHolder"];

            var characteristics = Deserialise<IEnumerable<int>>(characteristicsJson);

            var venue = new Venue();

            venue.Campus_ID = idHolder.Campus_ID;
            venue.Building_ID = idHolder.Building_ID;
            venue.Building_Floor_ID = idHolder.Building_Floor_ID;
            venue.Venue_Name = venueName;
            venue.Venue_Type = type;
            venue.Capacity = capacity;

            db.Venues.Add(venue);
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Add Venue", "Create", User.Identity.Name);

            foreach (int characteristic in characteristics)
            {
                var ch = new Venue_Characteristic
                {
                    Venue_ID = venue.Venue_ID,
                    Characteristic_ID = characteristic,
                };
                db.Venue_Characteristic.Add(ch);
            }

            db.SaveChanges();

            var venues = db.Venues.Where(v => v.Building_Floor_ID == idHolder.Building_Floor_ID).ToList();
            var jsonObj = (from v in venues
                           select new
                           {
                               id = v.Venue_ID,
                               name = v.Venue_Name,
                               vtype = v.Venue_Type,
                               vcapacity = v.Capacity,
                          }).ToArray();

            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }

        //check venue
        [Authorize(Roles = "Admin")]
        public JsonResult checkVenue(string venueName)
        {
            var idHolder = (Venue)Session["idHolder"];

            var venues = db.Venues.Where(v => v.Building_Floor_ID == idHolder.Building_Floor_ID);

            var venue = venues.Where(v => v.Venue_Name.ToLower() == venueName.ToLower());

            if (venue.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //delete venue - get
        [Authorize(Roles = "Admin")]
        public PartialViewResult deleteVenue(int id)
        {
            var idHolder = (Venue)Session["idHolder"];

            var campus = db.Campus.Where(c => c.Campus_ID == idHolder.Campus_ID).FirstOrDefault();
            var building = db.Buildings.Where(b => b.Building_ID == idHolder.Building_ID).FirstOrDefault();
            var floor = db.Building_Floor.Where(f => f.Building_Floor_ID == idHolder.Building_Floor_ID).FirstOrDefault();
            var venue = db.Venues.Where(v => v.Venue_ID == id).FirstOrDefault();

            ViewBag.Campus = campus;
            ViewBag.Building = building;
            ViewBag.Floor = floor;
            ViewBag.Venue = venue;

            idHolder.Venue_ID = id;
            Session["idHolder"] = idHolder;

            return PartialView();
        }

        //delete venue - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult deleteVenue()
        {
            var idHolder = (Venue)Session["idHolder"];

            var bookingCheck = db.Venue_Booking.Where(b => b.Venue_ID == idHolder.Venue_ID);
            var problemCheck = db.Venue_Problem.Where(p => p.Venue_ID == idHolder.Venue_ID);
            var questionnaireCheck = db.Questionnaires.Where(q => q.Venue_ID == idHolder.Venue_ID);

            if (bookingCheck.Any() || problemCheck.Any() || questionnaireCheck.Any())
                return Json("CLASH", JsonRequestBehavior.AllowGet);
            else
            {
                var characteristics = db.Venue_Characteristic.Where(c => c.Venue_ID == idHolder.Venue_ID).ToList();
                foreach (var ch in characteristics)
                {
                    db.Venue_Characteristic.Attach(ch);
                    db.Venue_Characteristic.Remove(ch);
                }

                var venue = db.Venues.Where(v => v.Venue_ID == idHolder.Venue_ID).FirstOrDefault();

                db.Venues.Attach(venue);
                db.Venues.Remove(venue);

                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Delete Venue", "Delete", User.Identity.Name);

                var venues = db.Venues.Where(v => v.Building_Floor_ID == idHolder.Building_Floor_ID).ToList();
                var jsonObj = (from v in venues
                               select new
                               {
                                   id = v.Venue_ID,
                                   name = v.Venue_Name,
                                   vtype = v.Venue_Type,
                                   vcapacity = v.Capacity,
                               }).ToArray();

                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            
        }

        //update venue - get
        [Authorize(Roles = "Admin")]
        public PartialViewResult updateVenue(int id)
        {
            var idHolder = (Venue)Session["idHolder"];
            idHolder.Venue_ID = id;
            Session["idHolder"] = idHolder;

            var campuses = db.Campus.ToList();
            var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID);
            var floors = db.Building_Floor.Where(f => f.Building_ID == idHolder.Building_ID);
            var types = db.Venue_Type;
            var venue = db.Venues.Where(v => v.Venue_ID == id).FirstOrDefault();
            var currentType = "yrdy";

            var currentCharacteristics = (from c in db.Venue_Characteristic
                                          where c.Venue_ID == id
                                          select c.Characteristic_ID).ToList();
            var characteristicsVenue = db.Characteristics.Where(c => currentCharacteristics.Contains(c.Characteristic_ID)).ToList();
            var allCharacteristics = db.Characteristics.ToList();
            var characteristicsNot = allCharacteristics.Except(characteristicsVenue).ToList();

            var charList = new List<checkedCharacteristics>();

            foreach (var characteristic in characteristicsVenue)
            {
                var a = new checkedCharacteristics();
                a.characteristic = characteristic;
                a.has = true;
                charList.Add(a);
            }

            foreach (var characteristic in characteristicsNot)
            {
                var a = new checkedCharacteristics();
                a.characteristic = characteristic;
                a.has = false;
                charList.Add(a);
            }

            ViewBag.Campuses = campuses;
            ViewBag.Buildings = buildings;
            ViewBag.Floors = floors;
            ViewBag.Types = types;
            ViewBag.Characteristics = charList;
            ViewBag.CurrentType = currentType;

            return PartialView(venue);
        }

        //update venue - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public JsonResult updateVenue(string venueName, int venueCapacity, int campus, int building, int floor, string type, string characteristics)
        {
            var characteristicsList = Deserialise<IEnumerable<int>>(characteristics);

            var idHolder = (Venue)Session["idHolder"];

            var venue = db.Venues.Where(v => v.Venue_ID == idHolder.Venue_ID).FirstOrDefault();

            venue.Campus_ID = campus;
            venue.Building_ID = building;
            venue.Building_Floor_ID = floor;
            venue.Venue_Name = venueName;
            venue.Capacity = venueCapacity;
            venue.Venue_Type = type;

            db.Entry(venue).State = System.Data.Entity.EntityState.Modified;

            var vc = db.Venue_Characteristic.Where(v => v.Venue_ID == idHolder.Venue_ID).ToList();

            foreach (var c in vc)
            {
                db.Venue_Characteristic.Attach(c);
                db.Venue_Characteristic.Remove(c);
            }

            foreach(var c in characteristicsList)
            {
                var venChar = new Venue_Characteristic();
                venChar.Venue_ID = idHolder.Venue_ID;
                venChar.Characteristic_ID = c;
                db.Venue_Characteristic.Add(venChar);
            }

            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Update Venue", "Update", User.Identity.Name);

            var venues = db.Venues.Where(v => v.Building_Floor_ID == idHolder.Building_Floor_ID).ToList();
            var jsonObj = (from v in venues
                           select new
                           {
                               id = v.Venue_ID,
                               name = v.Venue_Name,
                               vtype = v.Venue_Type,
                               vcapacity = v.Capacity,
                           }).ToArray();

            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        //get building floors
        [Authorize]
        public JsonResult getBuildFloors(int id)
        {
            var floors = db.Building_Floor.Where(f => f.Building_ID == id).ToList();
            var jsonObj = (from f in floors
                           select new
                           {
                               id = f.Building_Floor_ID,
                               text = f.Floor_Name,
                           }).ToArray();

            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        //search campus
        [Authorize(Roles = "Admin")]
        public JsonResult searchCampus(string search)
        {
            if (search != null)
            {
                var campuses = db.Campus.ToList();

                var result = campuses.Where(c => c.Campus_Name.ToLower().Contains(search.ToLower())).ToList();
                var jsonObj = (from c in result
                               select new
                               {
                                   id = c.Campus_ID,
                                   name = c.Campus_Name,
                               }).ToArray();
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var campuses = db.Campus.ToList();
                var jsonObj = (from c in campuses
                               select new
                               {
                                   id = c.Campus_ID,
                                   name = c.Campus_Name,
                               }).ToArray();
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            

        }

        //search building
        [Authorize(Roles = "Admin")]
        public JsonResult searchBuilding(string search)
        {
            var idHolder = (Venue)Session["idHolder"];

            if (search != null)
            {
                var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID).ToList();

                var result = buildings.Where(c => c.Building_Name.ToLower().Contains(search.ToLower())).ToList();
                var jsonObj = (from c in result
                               select new
                               {
                                   id = c.Building_ID,
                                   name = c.Building_Name,
                               }).ToArray();
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var buildings = db.Buildings.Where(b => b.Campus_ID == idHolder.Campus_ID).ToList();
                var jsonObj = (from c in buildings
                               select new
                               {
                                   id = c.Building_ID,
                                   name = c.Building_Name,
                               }).ToArray();
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }


        }

        //search floors
        [Authorize(Roles = "Admin")]
        public JsonResult searchFloor(string search)
        {
            var idHolder = (Venue)Session["idHolder"];

            if (search != null)
            {
                var floors = db.Building_Floor.Where(b => b.Building_ID == idHolder.Building_ID).ToList();

                var result = floors.Where(c => c.Floor_Name.ToLower().Contains(search.ToLower())).ToList();
                var jsonObj = (from c in result
                               select new
                               {
                                   id = c.Building_Floor_ID,
                                   name = c.Floor_Name,
                               }).ToArray();
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var floors = db.Building_Floor.Where(b => b.Building_ID == idHolder.Building_ID).ToList();
                var jsonObj = (from c in floors
                               select new
                               {
                                   id = c.Building_Floor_ID,
                                   name = c.Floor_Name,
                               }).ToArray();
                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }


        }

        //search venues
        [Authorize(Roles = "Admin")]
        public JsonResult searchVenue(string search)
        {
            var idHolder = (Venue)Session["idHolder"];

            if (search != null)
            {
                var venues = db.Venues.Where(b => b.Building_Floor_ID == idHolder.Building_Floor_ID).ToList();

                var result = venues.Where(c => c.Venue_Name.ToLower().Contains(search.ToLower())).ToList();
                var jsonObj = (from v in result
                               select new
                               {
                                   id = v.Venue_ID,
                                   name = v.Venue_Name,
                                   vtype = v.Venue_Type,
                                   vcapacity = v.Capacity,
                               }).ToArray();

                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var venues = db.Venues.Where(b => b.Building_Floor_ID == idHolder.Building_Floor_ID).ToList();
                var jsonObj = (from v in venues
                               select new
                               {
                                   id = v.Venue_ID,
                                   name = v.Venue_Name,
                                   vtype = v.Venue_Type,
                                   vcapacity = v.Capacity,
                               }).ToArray();

                return Json(jsonObj, JsonRequestBehavior.AllowGet);
            }


        }

        //report a venue problem
        [Authorize]
        public PartialViewResult reportProblem()
        {
            var campuses = db.Campus.ToList();
            ViewBag.Campuses = campuses;
            return PartialView();
        }

        [Authorize]
        //get buildings for selected floor
        public JsonResult getFloorVenues(int id)
        {
            var venues = db.Venues.Where(v => v.Building_Floor_ID == id).ToList();
            var jsonObj = (from v in venues
                           select new
                           {
                               id = v.Venue_ID,
                               text = v.Venue_Name,
                           }).ToArray();
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //get common problem types
        public JsonResult getProblemTypes()
        {
            var types = db.Common_Problem_Type.ToList();
            var jsonObj = (from t in types
                           select new
                           {
                               id = t.Common_Problem_Type_ID,
                               name = t.Common_Problem_Type_Name,
                           }).ToArray();
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        //get problems
        public JsonResult getProblems(int id)
        {
            var problems = db.Common_Problem.Where(p => p.Common_Problem_Type_ID == id).ToList();
            var jsonObj = (from p in problems
                           select new
                           {
                               id = p.Common_Problem_ID,
                               name = p.Common_Problem_Name,
                           }).ToArray();
            return Json(jsonObj, JsonRequestBehavior.AllowGet);
        }

        //capture venue problem
        [HttpPost]
        [Authorize]
        public void captureProblem(int venue, int problem, string comment)
        {
            var vp = new Venue_Problem
            {
                DateTime_Logged = DateTime.Now,
                Person_ID_Logged = User.Identity.Name,
                Description = comment,
                Common_Problem_ID = problem,
                Status = "Open",
                Venue_ID = venue,
            };

            db.Venue_Problem.Add(vp);
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Add Venue Problem", "Create", User.Identity.Name);
        }

        //view venue problems
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult viewProblems()
        {
            var campuses = db.Campus.ToList();
            var buildings = db.Buildings.ToList();
            var floors = db.Building_Floor.ToList();
            var problems = db.Venue_Problem.Where(p => p.Status == "Open").ToList();

            var list = new List<problemList>();

            foreach(var problem in problems)
            {
                var campus = campuses.Where(c => c.Campus_ID == problem.Venue.Campus_ID).FirstOrDefault();
                var building = buildings.Where(b => b.Building_ID == problem.Venue.Building_ID).FirstOrDefault();
                var floor = floors.Where(f => f.Building_Floor_ID == problem.Venue.Building_Floor_ID).FirstOrDefault();

                var p = new problemList();
                p.campus = campus.Campus_Name;
                p.building = building.Building_Name;
                p.floor = floor.Floor_Name;
                p.problem = problem;

                list.Add(p);
            }

            ViewBag.Problems = list;
            return View();
        }

        //view problem details
        [Authorize(Roles = "Admin, Employee")]
        public PartialViewResult viewProblemDetails(int id)
        {
            var problem = db.Venue_Problem.Where(p => p.Problem_Seq == id).FirstOrDefault();
            var campus = db.Campus.Where(c => c.Campus_ID == problem.Venue.Campus_ID).FirstOrDefault();
            var building = db.Buildings.Where(b => b.Building_ID == problem.Venue.Building_ID).FirstOrDefault();
            var floor = db.Building_Floor.Where(f => f.Building_Floor_ID == problem.Venue.Building_Floor_ID).FirstOrDefault();

            ViewBag.Campus = campus;
            ViewBag.Building = building;
            ViewBag.Floor = floor;

            return PartialView(problem);
        }

        //resolve venue problem - get
        [Authorize(Roles = "Admin, Employee")]
        public PartialViewResult resolveProblem(int id)
        {
            Session["problem"] = id;
            return PartialView();
        }

        //resolve venue problem - post
        [HttpPost]
        [Authorize(Roles = "Admin, Employee")]
        public string resolveProblem()
        {
            var id = (int)Session["problem"];
            var problem = db.Venue_Problem.Where(p => p.Problem_Seq == id).FirstOrDefault();
            problem.Status = "Closed";
            problem.DateTime_Closed = DateTime.Now;
            problem.Person_ID_Closed = User.Identity.Name;
            db.Entry(problem).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Resolve Problem", "Update", User.Identity.Name);

            return id.ToString();
        }

        //add problem type - get
        [Authorize(Roles = "Admin")]
        public ActionResult addProblemType()
        {
            return View();
        }

        //add problem type - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult addProblemType(problemTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var type = new Common_Problem_Type
                {
                    Common_Problem_Type_Name = model.name,
                    Description = model.description,
                };

                db.Common_Problem_Type.Add(type);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Add Problem Type", "Create", User.Identity.Name);

                return RedirectToAction("viewProblemTypes");
            }
            else return View();
        }

        //view problem types
        [Authorize(Roles = "Admin")]
        public ActionResult viewProblemTypes()
        {
            var types = db.Common_Problem_Type;
            return View(types);
        }

        //delete problem types - get
        [Authorize(Roles = "Admin")]
        public ActionResult deleteProblemType(int id)
        {
            var type = db.Common_Problem_Type.Where(t => t.Common_Problem_Type_ID == id).FirstOrDefault();
            Session["typeID"] = type.Common_Problem_Type_ID;
            return View(type);
        }

        //delete problem types - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult deleteProblemType()
        {
            var id = (int)Session["typeID"];
            var type = db.Common_Problem_Type.Where(t => t.Common_Problem_Type_ID == id).FirstOrDefault();

            var problems = db.Common_Problem.Where(p => p.Common_Problem_Type_ID == id);

            if (problems.Any())
            {
                var old = db.Common_Problem_Type.Where(p => p.Common_Problem_Type_ID == id).FirstOrDefault();
                TempData["Message"] = "Problem Type Has Existing Dependencies";
                TempData["classStyle"] = "danger";
                return View(old);
            }
            else
            {
                db.Common_Problem_Type.Attach(type);
                db.Common_Problem_Type.Remove(type);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Delete Problem Types", "Delete", User.Identity.Name);

                return RedirectToAction("viewProblemTypes");
            }
            
        }

        //edit problem type - get
        [Authorize(Roles = "Admin")]
        public ActionResult editProblemType(int id)
        {
            var type = db.Common_Problem_Type.Where(t => t.Common_Problem_Type_ID == id).FirstOrDefault();
            return View(type);
        }

        //edit problem type - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult editProblemType(Common_Problem_Type model)
        {
            if (ModelState.IsValid)
            {
                db.Entry(model).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Update Problem Types", "Update", User.Identity.Name);

                return RedirectToAction("viewProblemTypes");
            }
            else return View();
        }

        //view problems
        [Authorize(Roles = "Admin")]
        public ActionResult viewCommonProblems()
        {
            var problems = db.Common_Problem;
            return View(problems);
        }

        //add common problem - get
        [Authorize(Roles = "Admin")]
        public ActionResult addCommonProblem()
        {
            var list = new SelectList(db.Common_Problem_Type, "Common_Problem_Type_ID", "Common_Problem_Type_Name");
            ViewBag.Common_Problem_Type_ID = list;
            return View();
        }

        //add common problem - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult addCommonProblem(Common_Problem model)
        {
            if (ModelState.IsValid)
            {
                db.Common_Problem.Add(model);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Add Common Problem", "Create", User.Identity.Name);

                return RedirectToAction("viewCommonProblems");
            }
            else return View();         
        }

        //edit common problem - get
        [Authorize(Roles = "Admin")]
        public ActionResult editCommonProblem(int id)
        {
            var problem = db.Common_Problem.Where(p => p.Common_Problem_ID == id).FirstOrDefault();
            var model = new commonProblemModel
            {
                Common_Problem_ID = problem.Common_Problem_ID,
                Common_Problem_Name = problem.Common_Problem_Name,
                Description = problem.Description,
                Common_Problem_Type_ID = problem.Common_Problem_Type_ID
            };
            var list = new SelectList(db.Common_Problem_Type, "Common_Problem_Type_ID", "Common_Problem_Type_Name");
            ViewBag.Common_Problem_Type_ID = list;
            return View(model);
        }

        //edit common problem - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult editCommonProblem(commonProblemModel model)
        {
            if (ModelState.IsValid)
            {
                var problem = db.Common_Problem.Where(p => p.Common_Problem_ID == model.Common_Problem_ID).FirstOrDefault();

                problem.Common_Problem_Name = model.Common_Problem_Name;
                problem.Description = model.Description;
                problem.Common_Problem_Type_ID = model.Common_Problem_Type_ID;

                db.Entry(problem).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Update Common Problem", "Update", User.Identity.Name);

                return RedirectToAction("viewCommonProblems");
            }
            else
            {
                var list = new SelectList(db.Common_Problem_Type, "Common_Problem_Type_ID", "Common_Problem_Type_Name");
                ViewBag.Common_Problem_Type_ID = list;
                return View();
            }
            
        }

        //delte common problem - get
        [Authorize(Roles = "Admin")]
        public ActionResult deleteCommonProblem(int id)
        {
            Session["problemID"] = id;
            var problem = db.Common_Problem.Where(p => p.Common_Problem_ID == id).FirstOrDefault();
            return View(problem);
        }

        //delete common problem - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult deleteCommonProblem()
        {
            var id = (int)Session["problemID"];
            var vp = db.Venue_Problem.Where(p => p.Common_Problem_ID == id);
            if (vp.Any())
            {
                var problem = db.Common_Problem.Where(p => p.Common_Problem_ID == id).FirstOrDefault();
                TempData["Message"] = "Problem Has Existing Dependencies";
                TempData["classStyle"] = "danger";
                return View(problem);
            }
            else
            {
                var problem = db.Common_Problem.Where(p => p.Common_Problem_ID == id).FirstOrDefault();
                db.Common_Problem.Attach(problem);
                db.Common_Problem.Remove(problem);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Delete Common Problem", "Delete", User.Identity.Name);

                return RedirectToAction("viewCommonProblems");
            }
        }

        //view characteristic
        [Authorize(Roles = "Admin")]
        public ActionResult viewCharacteristics()
        {
            var characteristics = db.Characteristics.ToList();
            return View(characteristics);
        }

        //add characteristics - get
        [Authorize(Roles = "Admin")]
        public ActionResult addCharacteristic()
        {
            return View();
        }

        //add characteristic - partial
        [Authorize(Roles = "Admin")]
        public PartialViewResult addVenueCharacteristic()
        {
            return PartialView();
        }

        //add characteristic - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult addCharacteristic(Characteristic model)
        {
            if (ModelState.IsValid)
            {
                db.Characteristics.Add(model);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Add Characteristic", "Create", User.Identity.Name);

                return RedirectToAction("viewCharacteristics");
            }
            else
            {
                return View();
            }
        }

        //update characteristic - get
        [Authorize(Roles = "Admin")]
        public ActionResult updateCharacteristic(int id)
        {
            var c = db.Characteristics.Where(e => e.Characteristic_ID == id).FirstOrDefault();
            var model = new UpdateCharModel
            {
                id = c.Characteristic_ID,
                name = c.Characteristic_Name,
                description = c.Description,
            };
            return View(model);
        }

        //update characteristic - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult updateCharacteristic(UpdateCharModel model)
        {
            var c = db.Characteristics.Where(e => e.Characteristic_ID == model.id).FirstOrDefault();
            c.Characteristic_Name = model.name;
            c.Description = model.description;
            db.Entry(c).State = System.Data.Entity.EntityState.Modified;
            db.SaveChanges();

            //record action
            global.addAudit("Venues", "Venues: Update Characteristic", "Update", User.Identity.Name);

            return RedirectToAction("viewCharacteristics");
        }

        //delete characteristic - get
        [Authorize(Roles = "Admin")]
        public ActionResult deleteCharacteristic(int id)
        {
            Session["charID"] = id;
            var c = db.Characteristics.Where(e => e.Characteristic_ID == id).First();
            return View(c);
        }

        //delete characteristic - post
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult deleteCharacteristic()
        {
            var id = (int)Session["charID"];
            var c = db.Characteristics.Where(e => e.Characteristic_ID == id).First();

            var check = db.Venue_Characteristic.Where(a => a.Characteristic_ID == id);

            if (check.Any())
            {
                TempData["Message"] = "Characteristic Has Existing Dependencies";
                TempData["classStyle"] = "danger";
                return View(c);
            }
            else
            {
                db.Characteristics.Attach(c);
                db.Characteristics.Remove(c);
                db.SaveChanges();

                //record action
                global.addAudit("Venues", "Venues: Delete Characteristic", "Delete", User.Identity.Name);

                TempData["Message"] = "Characteristic Succesfully Deleted";
                TempData["classStyle"] = "success";
                return RedirectToAction("viewCharacteristics");
            }
        }
        
        //controler dependant classes
        private T Deserialise<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serialiser = new DataContractJsonSerializer(typeof(T));
                return (T)serialiser.ReadObject(ms);
            }
        }
    }
}