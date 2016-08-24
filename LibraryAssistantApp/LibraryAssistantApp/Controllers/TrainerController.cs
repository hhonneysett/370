using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class TrainerController : Controller
    {
        //initiate instance of database
        LibraryAssistantEntities db = new LibraryAssistantEntities();
        
        //display all existing categories
        [HttpGet]
        public ActionResult viewCategory ()
        {

            //get list of existing categories
            var existingCategories = from c in db.Categories
                                     select c;

            //populate a select list from list of categories
            ViewBag.Category_ID = new SelectList(existingCategories, "Category_ID", "Category_Name");
            return View();
        }

        [HttpGet]
        public ActionResult addCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addCategory([Bind(Exclude ="categoryId")]CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                //check if a category with the same name already exists
                var categoryCheck = from a in db.Categories
                                    where a.Category_Name.ToLower().Equals(model.categoryName)
                                    select a;

                if (categoryCheck.Any())
                {
                    TempData["Message"] = "Category already exists with the same name";
                    TempData["classStyle"] = "warning";
                    return View(model);
                }
                else
                {
                    //create new instanc of a category object and assing model values to object
                    Category a = new Category
                    {
                        Category_Name = model.categoryName,
                        Description = model.description,
                    };

                    //add the new category to the database and save
                    db.Categories.Add(a);
                    db.SaveChanges();


                    TempData["Message"] = "Category successfully added";
                    TempData["classStyle"] = "success";
                    return RedirectToAction("viewCategory");
                }                               
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult updateCategory(int id)
        {
            //get selected category
            var model = (from c in db.Categories
                         where c.Category_ID.Equals(id)
                         select c).FirstOrDefault();

            //assign selected category to tempdata
            TempData["selectedCat"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult updateCategory(Category model, string submitButton)
        {           
            if(ModelState.IsValid)
            {
                switch(submitButton)
                {
                    //check if update button was pressed
                    case "Update Category":
                        //check if changed category name already exists
                        var check = from c in db.Categories
                                    where c.Category_Name.Equals(model.Category_Name) && c.Category_ID != model.Category_ID
                                    select c;

                        if (check.Any())
                        {
                            //name already exists, dislpay error message and go back
                            TempData["Message"] = "Category with the provided name already exists";
                            TempData["classStyle"] = "warning";
                            return View(model);
                        }
                        else
                        {
                            //capture the updated category
                            db.Entry(model).State = EntityState.Modified;
                            db.SaveChanges();

                            //display success message
                            TempData["Message"] = "Category successfully updated";
                            TempData["classStyle"] = "success";
                            return RedirectToAction("viewCategory");
                        };

                    //check if delete button was pressed
                    case "Delete Category":
                        return RedirectToAction("deleteCategory", "Trainer", new { id = model.Category_ID });

                    //if no button was pressed return form
                    default:
                        return View(model);
                }               
            }
            else
            {
                return View(model);
            }                   
        }

        [HttpGet]
        public ActionResult deleteCategory(int id)
        {
            //get selected category
            var model = (from c in db.Categories
                         where c.Category_ID.Equals(id)
                         select c).FirstOrDefault();

            //assign selected category to tempdata
            TempData["selectedCat"] = model;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult deleteCategory()
        {
            var model = (Category)TempData["selectedCat"];

            //check if category is in use in category topic table
            var check = (from ct in db.Topic_Category
                         where ct.Category_ID.Equals(model.Category_ID)
                         select ct).ToList() ;

            if (check.Any())
            {
                TempData["Message"] = "Unable to delete category. Category is in use";
                TempData["classStyle"] = "warning";
                return View(model);
            }
            else
            {
                //get the seleceted category
                Category selectedCat = db.Categories.Find(model.Category_ID);

                //delete the selected category from the database
                db.Categories.Remove(selectedCat);
                db.SaveChanges();

                //alert successful deletion
                TempData["Message"] = "Category successfully deleted";
                TempData["classStyle"] = "success";

                //return to the view categories page
                return RedirectToAction("ViewCategory");
            }
        }

        [HttpGet]
        public ActionResult viewTopic()
        {
            //get list of existing categories
            var existingCategories = from c in db.Categories
                                     select c;

            ViewBag.Category = existingCategories;
            return View();
        }

        [HttpGet]
        public JsonResult getCatTopic(int id)
        {
            var Topics = (from t in db.Topics
                          join c in db.Topic_Category on t.Topic_Seq equals c.Topic_Seq
                          where c.Category_ID.Equals(id)
                          select t).ToList();

            var jsonList = from b in Topics
                           select new
                           {
                               id = b.Topic_Seq,
                               text = b.Topic_Name,
                           };

            var rows = jsonList.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult updateTopic(int id)
        {
            //get list of existing categories
            var existingCategories = from c in db.Categories
                                     select c;

            ViewBag.Category = existingCategories;

            var currentTopCat = (from t in db.Topics
                                   join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                   where tc.Topic_Seq.Equals(id)
                                   select tc).FirstOrDefault();

            var currentCat = (from c in db.Categories
                              where c.Category_ID.Equals(currentTopCat.Category_ID)
                              select c.Category_ID).FirstOrDefault().ToString();

            ViewBag.currentCat = currentCat;

            var model = (from t in db.Topics
                         where t.Topic_Seq.Equals(id)
                         select t).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult updateTopic(Topic model, string submitButton, int category)
        {
            if (ModelState.IsValid)
            {
                switch (submitButton)
                {

                    //check if save button was clicked
                    case "Save":

                        //check if any topics with the same name already exist
                        var check = (from t in db.Topics
                                     where t.Topic_Name.Equals(model.Topic_Name) && t.Topic_Seq != model.Topic_Seq
                                     select t);
                        
                        //if topic with the same name already exists return the form and display an error message
                        if (check.Any())
                        {
                            //get list of existing categories
                            var existCat = from c in db.Categories
                                                     select c;

                            //get current topic category
                            ViewBag.Category = existCat;

                            //get the current topic category object for the selected category
                            var currentTC = (from t in db.Topics
                                                 join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                                 where tc.Topic_Seq.Equals(model.Topic_Seq)
                                                 select tc).FirstOrDefault();

                            //get the current category for the selected topic
                            var currentC = (from c in db.Categories
                                              where c.Category_ID.Equals(currentTC.Category_ID)
                                              select c.Category_ID).FirstOrDefault().ToString();

                            ViewBag.currentCat = currentC;

                            //display error message
                            TempData["Message"] = "Topic already exists with the same name";
                            TempData["classStyle"] = "warning";

                            //return form
                            return View(model);
                        }
                        else
                        {

                            //get current topic category object
                            var currentTC = (from tc in db.Topic_Category
                                                 where tc.Topic_Seq.Equals(model.Topic_Seq)
                                                 select tc).FirstOrDefault();

                            //check if the category for the topic has changed
                            if (currentTC.Category_ID.Equals(category))
                            {

                                //if category hasnt changed only update the topic object
                                db.Entry(model).State = EntityState.Modified;
                                db.SaveChanges();


                                //display success message
                                TempData["Message"] = "Topic successfully updated";
                                TempData["classStyle"] = "success";

                                return RedirectToAction("viewTopic");
                            }
                            else
                            {
                                //topic category has been changed so must remove old topic category object
                                db.Topic_Category.Remove(currentTC);

                                //add details for the new topic category details
                                Topic_Category a = new Topic_Category
                                {
                                    Topic_Seq = model.Topic_Seq,
                                    Category_ID = category,
                                };

                                //capture the new topic category object
                                db.Topic_Category.Add(a);

                                //capture the updated topic details
                                db.Entry(model).State = EntityState.Modified;

                                //save changes to the database
                                db.SaveChanges();


                                //display message of success
                                TempData["Message"] = "Topic successfully updated";
                                TempData["classStyle"] = "success";

                                return RedirectToAction("viewTopic");
                            }
                        }
                    
                        //delete button was pressed go to delete action
                    case "Delete Topic":
                        return RedirectToAction("deleteTopic", "Trainer", new { id = model.Topic_Seq });

                    default:
                        //get list of existing categories
                        var existingCategories = from c in db.Categories
                                                 select c;

                        //pass list of existing categories to viewbag for select list
                        ViewBag.Category = existingCategories;

                        //get the topic category obkect for the selected topic
                        var currentTopCat = (from t in db.Topics
                                             join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                             where tc.Topic_Seq.Equals(model.Topic_Seq)
                                             select tc).FirstOrDefault();


                        //get the category for the selected topic
                        var currentCat = (from c in db.Categories
                                          where c.Category_ID.Equals(currentTopCat.Category_ID)
                                          select c.Category_ID).FirstOrDefault().ToString();


                        //assign the current category to a viewbag
                        ViewBag.currentCat = currentCat;


                        //return the view
                        return View(model);
                }               
            }
            else
            {
                //get list of existing categories
                var existingCategories = from c in db.Categories
                                         select c;

                //assign selected category to a viewbag
                ViewBag.Category = existingCategories;

                //get the selected topic topic category object
                var currentTopCat = (from t in db.Topics
                                     join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                     where tc.Topic_Seq.Equals(model.Topic_Seq)
                                     select tc).FirstOrDefault();


                //get the selected topic curremt category
                var currentCat = (from c in db.Categories
                                  where c.Category_ID.Equals(currentTopCat.Category_ID)
                                  select c.Category_ID).FirstOrDefault().ToString();

                //assing the current category to a viewbag
                ViewBag.currentCat = currentCat;

                //return view
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult addTopic()
        {
            //get list of current categories
            var categories = from c in db.Categories
                             select c;

            //assign current categories to a viewbag
            ViewBag.Categories = categories;

            //return the view
            return View();
        }

        [HttpPost]
        public ActionResult addTopic(Topic model, int category)
        {
            if (ModelState.IsValid)
            {
                //check if a topic with the same name already exists
                var check = from t in db.Topics
                            where t.Topic_Name.ToLower().Equals(model.Topic_Name)
                            select t;

                if (check.Any())
                {
                    //display error message
                    TempData["Message"] = "Topic with the same name already exists";
                    TempData["classStyle"] = "warning";

                    //return form
                    return View(model);
                }
                else
                {
                    //add new instance of topic to database
                    db.Topics.Add(model);
                    db.SaveChanges();

                    var newTopicSeq = (from t in db.Topics
                                       where t.Topic_Name.Equals(model.Topic_Name)
                                       select t.Topic_Seq).FirstOrDefault();
                    //create new instance of topic category
                    Topic_Category a = new Topic_Category
                    {
                        Topic_Seq = newTopicSeq,
                        Category_ID = category,
                    };

                    //add new instance of topic category to the database
                    db.Topic_Category.Add(a);

                    //save changes to the database
                    db.SaveChanges();

                    //display success message
                    TempData["Message"] = "Topic successfully added";
                    TempData["classStyle"] = "success";

                    //return view
                    return RedirectToAction("viewTopic");
                }
            }
            else
            {
                //get list of current categories
                var categories = from c in db.Categories
                                 select c;

                //assign current categories to a viewbag
                ViewBag.Categories = categories;

                return View(model);
            }                   
        }

        [HttpGet]
        public ActionResult deleteTopic(int id)
        {
            var topic = (from t in db.Topics
                         where t.Topic_Seq.Equals(id)
                         select t).FirstOrDefault();

            var topicSeq = topic.Topic_Seq;

            Session["selectedTopic"] = topicSeq;

            var topicCat = (from t in db.Topics
                            join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                            where tc.Topic_Seq.Equals(id)
                            select tc.Category_ID).FirstOrDefault();

            var category = (from c in db.Categories
                            where c.Category_ID.Equals(topicCat)
                            select c).FirstOrDefault();

            ViewBag.Category = category;

            return View(topic);
        }

        [HttpPost]
        public ActionResult deleteTopic()
        {
            var topicSeq = (int)Session["selectedTopic"];

            var topic = (from t in db.Topics
                         where t.Topic_Seq.Equals(topicSeq)
                         select t).FirstOrDefault();

            var topicCat = (from tc in db.Topic_Category
                            where tc.Topic_Seq.Equals(topic.Topic_Seq)
                            select tc).FirstOrDefault();

            //db.Topics.Attach(test);
            //db.Topic_Category.Attach(topicCat);

            db.Topic_Category.Remove(topicCat);
            db.Topics.Remove(topic);

            db.SaveChanges();

            TempData["Message"] = "Topic successfully deleted";
            TempData["classStyle"] = "success";

            return RedirectToAction("viewTopic");
        }

        [HttpGet]
        public ActionResult addTrainingSession()
        {
            //get list of existing categories
            var categories = from c in db.Categories
                             select c;
            //get list of existing campuses
            var campuses = from c in db.Campus
                           select c;

            //get list of characterisstics
            var characteristics = from c in db.Characteristics
                                  select c;

            //assign list of categories to viewbag
            ViewBag.Categories = categories;


            //assign list of campuses to viewbag
            ViewBag.Campuses = campuses;

            //assing list of characteristics to viewbag
            ViewBag.Characteristics = characteristics;

            return View();
        }

        [HttpGet]
        public ActionResult getTrainingVenues(string model, string characteristics)
        {
            //deserialise the training session model
            TrainingSessionModel sessionDetails = Deserialise<TrainingSessionModel>(model);

            //capture the session details to a session variable
            Session["sessionDetails"] = sessionDetails;

            //deserialize the list of characteristics
            IEnumerable<int> characteristicList = Deserialise<IEnumerable<int>>(characteristics);

            DateTime date = Convert.ToDateTime(sessionDetails.startDate);

            TimeSpan duration = new TimeSpan(0, sessionDetails.duration, 0);

            DateTime startTime = new DateTime();
            TimeSpan ts = new TimeSpan(7, 30, 0);
            startTime = date + ts;

            DateTime endTime = new DateTime();
            TimeSpan ets = new TimeSpan(17, 00, 0);
            endTime = date + ets;

            List<timeslot> timeslots = new List<timeslot>();
            int tsCount = 1;

            while ((startTime + duration) < endTime)
            {
                timeslot a = new timeslot();
                a.startDate = startTime;
                DateTime tempDate = startTime + duration;
                startTime = tempDate;
                a.endDate = startTime;
                a.id = tsCount;
                tsCount = tsCount + 1;
                timeslots.Add(a);
            }

            List<venueTimeslot> timeslotList = new List<venueTimeslot>();

            foreach(timeslot t in timeslots)
            {
                venueTimeslot vt = new venueTimeslot();
                vt.timeslot = t;
                vt.venues = db.findBookingVenuesFunc(t.startDate, t.endDate, "Training", sessionDetails.Campus_ID).ToList() ;
                timeslotList.Add(vt);
            }

            //assing timeslot list to session variable
            Session["timeslotList"] = timeslotList;

            List<Venue> distinctVenue = new List<Venue>();

            foreach(venueTimeslot vt in timeslotList)
            {
                foreach(Venue v in vt.venues)
                {
                    if (distinctVenue.Contains(v))
                    {
                        //do nothing because list already contains the venue
                    }
                    else
                    {
                        distinctVenue.Add(v);
                    }
                }
            }

            return PartialView(distinctVenue);          
        }

        [HttpGet]
        public PartialViewResult addTrainingSessionDetails()
        {
            //create local variable of selected venue
            var venue = (Venue)Session["venueSelect"];

            //create local variable of session details
            var session = (TrainingSessionModel)Session["sessionDetails"];

            //create local variable of venue timeslot lists
            var timeslotList = (List<venueTimeslot>)Session["timeslotList"];

            //get building details and assing to a viewbag
            var building = (from b in db.Buildings
                            where b.Building_ID.Equals(venue.Building_ID)
                            select b.Building_Name).FirstOrDefault();
            ViewBag.building = building;

            //get campus details and assing to a viewbag
            var campus = (from c in db.Campus
                          where c.Campus_ID.Equals(session.Campus_ID)
                          select c.Campus_Name).FirstOrDefault();
            ViewBag.campus = campus;

            //get topic details and assing to a viewbag
            var topic = (from t in db.Topics
                         where t.Topic_Seq.Equals(session.Topic_ID)
                         select t.Topic_Name).FirstOrDefault();
            ViewBag.topic = topic;

            //get timeslots available for the selected venue
            List<timeslot> available = new List<timeslot>();

            //go through the venue timeslots and check for the venue
            foreach(venueTimeslot vt in timeslotList)
            {
                foreach(Venue v in vt.venues)
                {
                    if (v.Venue_ID.Equals(venue.Venue_ID))
                    {
                        available.Add(vt.timeslot);
                    }
                }
            }

            //assing the avialable timeslots to a session variable
            Session["availableVen"] = available;
           
            //return partial view
            return PartialView();
        }

        [HttpGet]
        public PartialViewResult getTrainers(int id)
        {
            //create local variable of timeslots
            var timeslotList = (List<venueTimeslot>)Session["timeslotList"];

            //create local variable of session details
            var session = (TrainingSessionModel)Session["sessionDetails"];

            //get selected timeslot from id
            timeslot timeslot = new timeslot();

            foreach(venueTimeslot ts in timeslotList)
            {
                if (ts.timeslot.id.Equals(id))
                {
                    timeslot = ts.timeslot;
                }
            }

            //assing selected timeslot to session data
            Session["selectedTimeslot"] = timeslot;

            var venue_booking_person = db.Venue_Booking_Person.Include(vb => vb.Venue_Booking).Where(vbp => vbp.Attendee_Status.Equals("Active") && vbp.Attendee_Type.Equals("Trainer")).ToList();

            var vbpClash = (from a in venue_booking_person
                           where (a.Venue_Booking.DateTime_From >= timeslot.startDate && a.Venue_Booking.DateTime_From <= timeslot.endDate) || (a.Venue_Booking.DateTime_To >= timeslot.startDate && a.Venue_Booking.DateTime_To <= timeslot.endDate) || (a.Venue_Booking.DateTime_From <= timeslot.startDate && a.Venue_Booking.DateTime_To >= timeslot.endDate)
                           select a.Person_ID).ToList();

            var personRole = db.Person_Role.Include(r => r.Role).ToList() ;

            var allTrainers = (from a in personRole
                               where a.Role.Role_Name.Equals("Trainer")
                               select a.Registered_Person).Distinct().ToList();

            var availableTrainers = (from a in allTrainers
                                    where !vbpClash.Contains(a.Person_ID)
                                    select a.Person_ID).ToList();


            var trainerTopics = (from a in db.Person_Topic
                                where a.Topic_Seq.Equals(session.Topic_ID)
                                select a.Registered_Person).ToList();

            var topicMatch = (from a in trainerTopics
                             where availableTrainers.Contains(a.Person_ID)
                             select a);

            //assing available trainers to session data
            Session["availableTrainers"] = topicMatch;

            //return partial view
            return PartialView();
                                
        }

        [HttpGet]
        public void selectTrainer(string id)
        {
            //get the registered person by provided ID
            var trainer = db.Registered_Person.Where(r => r.Person_ID.Equals(id));

            //assing registered person to a session variable
            Session["trainer"] = trainer;
        }

        [HttpGet]
        public PartialViewResult additionalDetails()
        {
            return PartialView();
        }

        [HttpGet]
        public bool reapeatCheck(string repeatType, int multiple)
        {
            //local variable of selected timeslot and venue details
            var timeslot = (timeslot)Session["selectedTimeslot"];
            var venue = (Venue)Session["venueSelect"];


            switch (repeatType)
            {
                case "daily":
                    for (int i = 1; i <= multiple; i++)
                    {
                        var clash = from a in db.Venue_Booking
                                    where (a.DateTime_From >= timeslot.startDate && a.DateTime_From <= timeslot.endDate) || (a.DateTime_To >= timeslot.startDate && a.DateTime_To <= timeslot.endDate) || (a.DateTime_From <= timeslot.startDate && a.DateTime_To >= timeslot.endDate) && a.Venue_ID.Equals(venue.Venue_ID)
                                    select a;
                        if (clash.Any())
                        {
                            return false;
                        }
                        else
                        {                           
                            timeslot.startDate = timeslot.startDate.AddDays(1);
                            timeslot.endDate = timeslot.endDate.AddDays(1);
                        }
                    }
                    return true;

                case "weekly":
                    for (int i = 1; i <= multiple; i++)
                    {
                        var clash = from a in db.Venue_Booking
                                    where (a.DateTime_From >= timeslot.startDate && a.DateTime_From <= timeslot.endDate) || (a.DateTime_To >= timeslot.startDate && a.DateTime_To <= timeslot.endDate) || (a.DateTime_From <= timeslot.startDate && a.DateTime_To >= timeslot.endDate) && a.Venue_ID.Equals(venue.Venue_ID)
                                    select a;
                        if (clash.Any())
                        {
                            return false;
                        }
                        else
                        {
                            timeslot.startDate = timeslot.startDate.AddDays(7);
                            timeslot.endDate = timeslot.endDate.AddDays(7);
                        }
                    }
                    return true;

                case "monthly":
                    for (int i = 1; i <= multiple; i++)
                    {
                        var clash = from a in db.Venue_Booking
                                    where (a.DateTime_From >= timeslot.startDate && a.DateTime_From <= timeslot.endDate) || (a.DateTime_To >= timeslot.startDate && a.DateTime_To <= timeslot.endDate) || (a.DateTime_From <= timeslot.startDate && a.DateTime_To >= timeslot.endDate) && a.Venue_ID.Equals(venue.Venue_ID)
                                    select a;
                        if (clash.Any())
                        {
                            return false;
                        }
                        else
                        {
                            timeslot.startDate = timeslot.startDate.AddMonths(1);
                            timeslot.endDate = timeslot.endDate.AddMonths(1);
                        }
                    }
                    return true;

                case "yearly":
                    for (int i = 1; i <= multiple; i++)
                    {
                        var clash = from a in db.Venue_Booking
                                    where (a.DateTime_From >= timeslot.startDate && a.DateTime_From <= timeslot.endDate) || (a.DateTime_To >= timeslot.startDate && a.DateTime_To <= timeslot.endDate) || (a.DateTime_From <= timeslot.startDate && a.DateTime_To >= timeslot.endDate) && a.Venue_ID.Equals(venue.Venue_ID)
                                    select a;
                        if (clash.Any())
                        {
                            return false;
                        }
                        else
                        {
                            timeslot.startDate = timeslot.startDate.AddYears(1);
                            timeslot.endDate = timeslot.endDate.AddYears(1);
                        }
                    }
                    return true;

                default:
                    return true;
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