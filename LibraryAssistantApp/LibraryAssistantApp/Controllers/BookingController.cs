using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class BookingController : Controller
    {
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        public ActionResult ViewBookings()
        {
            return View();
        }

        public ActionResult employeeViewBookings()
        {
            //get list of campuses
            var campus = from c in db.Campus
                         select c;

            //assign list of campuses to a select list
            ViewBag.Campus_ID = new SelectList(campus, "Campus_ID", "Campus_Name");
            return View();
        }
        
        // GET: Book discussion room (student side)
        [HttpGet]
        public ActionResult BookDiscussionRoom()
        {
            ViewBag.Campus_ID = new SelectList(db.Campus, "Campus_ID", "Campus_Name");
            return View();
        }

        // GET: Book discussion room via helpdesk
        [HttpGet]
        public ActionResult employeeBookDiscussionRoom()
        {
            ViewBag.Campus_ID = new SelectList(db.Campus, "Campus_ID", "Campus_Name");
            return View();
        }

        // POST: Book discussion room (student side)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult BookDiscussionRoom(DiscussionRoomBooking model)
        {            
            if (ModelState.IsValid)
            {
                var dateToday = DateTime.Today;
                if (model.date > dateToday)
                {
                    model.time = Convert.ToDateTime(model.inTime);

                    //get the time and date components
                    var time = model.time.TimeOfDay;
                    var date = model.date.Date;

                    //calculate the start time of the new session
                    DateTime startDateTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);

                    //calculate the end time of the new session
                    TimeSpan duration = new TimeSpan(0, model.length, 0);
                    DateTime endDateTime = startDateTime.Add(duration);
                    model.date = startDateTime;
                    model.endDate = endDateTime;

                    //get the selected campus name and assign it to the model
                    var campus_name = (from c in db.Campus
                                       where c.Campus_ID.Equals(model.campus_ID)
                                       select c.Campus_Name).FirstOrDefault();
                    model.campus_name = campus_name;

                    //capture the submitted booking details to a session variable
                    Session["details"] = model;

                    //get all available venues according to the submitted criteria
                    var venues = db.findBookingVenuesFunc(startDateTime, endDateTime, "Discussion", model.campus_ID);
                    Session["venues"] = venues.ToList();

                    //get all existing characteristics to provide venue filtering on the form, and save it to a session variable
                    var characteristics = from c in db.Characteristics
                                          select c;
                    Session["characteristicList"] = characteristics.ToList();

                    //load the GetDiscussionRoomVenues view
                    return RedirectToAction("GetDiscussionRoomVenues");
                }
                else
                {
                    ViewBag.Campus_ID = new SelectList(db.Campus, "Campus_ID", "Campus_Name");
                    TempData["Message"] = "Invalid date selection, date is in the past.";
                    TempData["classStyle"] = "warning";
                    return View(model);
                }
            }
            ViewBag.Campus_ID = new SelectList(db.Campus, "Campus_ID", "Campus_Name");
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult employeeBookDiscussionRoom(DiscussionRoomBooking model)
        {
            if (ModelState.IsValid)
            {
                var dateToday = DateTime.Today;
                if (model.date > dateToday)
                {
                    //get person object
                    var validPersonId = db.Registered_Person.Where(p => p.Person_ID.Equals(model.person_id));

                    //check if the submitted person id is valid
                    if (validPersonId.Any())
                    {
                        //get the time and date components
                        var dtTime = Convert.ToDateTime(model.time);

                        var time = dtTime.TimeOfDay;
                        var date = model.date.Date;

                        //calculate the start time of the new session
                        DateTime startDateTime = new DateTime(date.Year, date.Month, date.Day, time.Hours, time.Minutes, time.Seconds);

                        //calculate the end time of the new session
                        TimeSpan duration = new TimeSpan(0, model.length, 0);
                        DateTime endDateTime = startDateTime.Add(duration);
                        model.date = startDateTime;
                        model.endDate = endDateTime;

                        //get the selected campus name and assign it to the model
                        var campus_name = (from c in db.Campus
                                           where c.Campus_ID.Equals(model.campus_ID)
                                           select c.Campus_Name).FirstOrDefault();
                        model.campus_name = campus_name;

                        //capture the submitted booking details to a session variable
                        Session["details"] = model;

                        //get all available venues according to the submitted criteria
                        var venues = db.findBookingVenuesFunc(startDateTime, endDateTime, "Discussion", model.campus_ID);
                        Session["venues"] = venues.ToList();

                        //get all existing characteristics to provide venue filtering on the form, and save it to a session variable
                        var characteristics = from c in db.Characteristics
                                              select c;
                        Session["characteristicList"] = characteristics.ToList();

                        //load the GetDiscussionRoomVenues view
                        return RedirectToAction("GetDiscussionRoomVenues");
                    }
                    else
                    {
                        ViewBag.Campus_ID = new SelectList(db.Campus, "Campus_ID", "Campus_Name");
                        TempData["Message"] = "Invalid date selection, date is in the past.";
                        TempData["classStyle"] = "warning";
                        return View(model);
                    }
                }
                else
                {
                    ViewBag.Campus_ID = new SelectList(db.Campus, "Campus_ID", "Campus_Name");
                    TempData["Message"] = "Invalid Person ID";
                    TempData["classStyle"] = "warning";
                    return View(model);
                }          
            }
            ViewBag.Campus_ID = new SelectList(db.Campus, "Campus_ID", "Campus_Name");
            return View(model);
        }

        // GET: Show available discussion room booking venues
        [HttpGet]
        public ActionResult GetDiscussionRoomVenues()
        {
            //set a variable to the matching venues stored in the session data
            var model = Session["venues"];

            //get a distince list of all building_ids from the available venues
            var buildings = (from a in (IEnumerable<Venue>)model
                             select a.Building_ID).Distinct();

            //using the buildings variable, create a list of building objects
            List<Building> buildingList = new List<Building>();
            foreach (var item in buildings)
            {
                var building = db.Buildings.Where(b => b.Building_ID.Equals(item)).FirstOrDefault();
                buildingList.Add(building);
            }

            //save the list of buildings to a session variable
            Session["buildings"] = buildingList;

            //load the view
            return View(model);
        }

        // POST: Capture the selected venue from the GetDiscussuinRoomVenues view
        public ActionResult venueSelect(int id)
        {
            //get the selected venue object from the database from the provided id
            var venue = db.Venues.Where(v => v.Venue_ID.Equals(id)).FirstOrDefault();

            //save the venue object to a session variable
            Session["venueSelect"] = venue;

            //get the building name of the selected building
            var buildingName = (from b in db.Buildings
                                where b.Building_ID.Equals(venue.Building_ID)
                                select b.Building_Name).FirstOrDefault();

            //capture the building name of the selected venue to a session variable
            Session["buildingName"] = buildingName;

            //return an empty result, which ends the action
            return new EmptyResult();
        }

        // GET: Display the confirm details partial view
        [HttpGet]
        public PartialViewResult confirmDetails()
        {
            return PartialView("confirmDetails");
        }

        // POST: Capture the confirmed booking details to the database
        [HttpPost]
        public ActionResult captureDetails()
        {
            //assign session variables and cast
            var venue = (Venue)Session["venueSelect"];
            var details = (DiscussionRoomBooking)Session["details"];

            //create instance of new Venue_Booking object
            Venue_Booking vb = new Venue_Booking();
            Venue_Booking_Person vbp = new Venue_Booking_Person();

            //get booking seq of discussion room
            var BookingTypeSeq = (from a in db.Booking_Type
                                  where a.Booking_Type_Name.Equals("Discussion Room")
                                  select a.Booking_Type_Seq).FirstOrDefault();

            //set properties of venue_booking object to submitted properties
            vb.Venue_Booking_Name = "null"; //not sure what this value represents
            vb.DateTime_From = details.date;
            vb.DateTime_To = details.endDate;
            vb.Send_Email_To_Topic_Person_Ind = 0; //no email 
            vb.Max_Bookings = 0; //doesn't apply to discussion room bookings
            vb.Exclusive_ind = 0; //discussion room sessions cant be exclusive
            vb.Description = "Discussion Room Session";
            vb.Booking_Type_Seq = BookingTypeSeq;
            vb.Topic_Seq = 1;
            vb.Booking_Status = "Active";
            vb.Venue_ID = venue.Venue_ID;
            vb.Building_Floor_ID = venue.Building_Floor_ID;
            vb.Building_ID = venue.Building_ID;
            vb.Campus_ID = venue.Campus_ID;

            //add new venue booking to database
            db.Venue_Booking.Add(vb);
            db.SaveChanges();

            //get booking seq of booking just created

            var bookingSeq = vb.Venue_Booking_Seq;

            //set properties of venue booking person object
            vbp.Venue_Booking_Seq = bookingSeq;
            if (details.person_id == null)
            {
                vbp.Person_ID = User.Identity.Name;
            }
            else
            {
                vbp.Person_ID = details.person_id;
            }          
            vbp.Certificate_Ind = 0;
            vbp.Attendee_Type = "Student";
            vbp.Attendee_Status = "Active";

            //add new venue booking person object to database
            db.Venue_Booking_Person.Add(vbp);
            db.SaveChanges();

            //get return url
            var site = Url.Action("ViewBookings", "Booking");
            return Content(site);
        }

        // GET: Get bookings for fullcalendar
        [HttpGet]
        public JsonResult getBookings()
        {
            //get booking seq of all bookings that belong to the user
            var bookingSeq = (from s in db.Venue_Booking_Person
                              where s.Person_ID.Equals(User.Identity.Name) && s.Attendee_Status.Equals("Active")
                              select s.Venue_Booking_Seq);

            //get all bookings that belong to the user
            var bookingList = (from b in db.Venue_Booking
                               where bookingSeq.Contains(b.Venue_Booking_Seq)
                               select b);

            //convert the booking list to a list type
            List<Venue_Booking> listOfBuildings = new List<Venue_Booking>();
            listOfBuildings = bookingList.ToList();

            //for each booking that belongs to the user create a json object
            var eventList = from e in listOfBuildings
                            select new
                            {
                                id = e.Venue_Booking_Seq,
                                text = e.Description,
                                start_date = e.DateTime_From.ToString(),
                                end_date = e.DateTime_To.ToString(),
                            };

            //create an array object from the event list
            var rows = eventList.ToArray();

            //return a JSON object
            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        // GET: Get bookings based on selected criteria
        public JsonResult getEmpBookings(string id, string idType)
        {
            //create local list
            IEnumerable<Venue_Booking> bookings;

            //switch to get bookings based on idType
            switch (idType)
            {
                case "personID":
                    var bookingSeq = (from b in db.Venue_Booking_Person
                                      where b.Person_ID.Equals(id) && b.Attendee_Status.Equals("Active")
                                      select b.Venue_Booking_Seq);
                    bookings = (from a in db.Venue_Booking
                                where bookingSeq.Contains(a.Venue_Booking_Seq)
                                select a);
                    break;
                case "venueID":
                    var test = id;
                    int venueID = Convert.ToInt32(id);
                    bookings = (from c in db.Venue_Booking
                                where c.Venue_ID.Equals(venueID) && c.Booking_Status.Equals("Active")
                                select c);
                    break;
                default:
                    bookings = (from d in db.Venue_Booking
                                select d);
                    break;
            }

            //create list type of bookings
            List<Venue_Booking> listOfBookings = bookings.ToList();

            //create an event list
            var eventList = from e in listOfBookings
                            select new
                            {
                                id = e.Venue_Booking_Seq,
                                text = e.Description,
                                start_date = e.DateTime_From.ToString(),
                                end_date = e.DateTime_To.ToString(),
                            };

            //create an array object from the event list
            var rows = eventList.ToArray();

            //return a JSON object
            return Json(rows, JsonRequestBehavior.AllowGet);

        }

        // GET: Selected booking details
        [HttpGet]
        public PartialViewResult getBookingDetails(int id)
        {
            //get the selected venue booking object
            var booking = db.Venue_Booking.Where(v => v.Venue_Booking_Seq.Equals(id)).FirstOrDefault();

            //get building name
            var buildingName = (from b in db.Buildings
                                where b.Building_ID.Equals(booking.Building_ID)
                                select b.Building_Name).FirstOrDefault();

            //get campus name
            var campusName = (from c in db.Campus
                              where c.Campus_ID.Equals(booking.Campus_ID)
                              select c.Campus_Name).FirstOrDefault();

            //get venue name
            var venueName = (from v in db.Venues
                             where v.Venue_ID.Equals(booking.Venue_ID)
                             select v.Venue_Name).FirstOrDefault();

            //create an instance of the view model
            BookingDetailsModel a = new BookingDetailsModel
            {
                booking_seq = booking.Venue_Booking_Seq,
                type = booking.Description,
                building = buildingName,
                campus = campusName,
                date = booking.DateTime_From.ToShortDateString(),
                timeslot = booking.DateTime_From.TimeOfDay + " - " + booking.DateTime_To.TimeOfDay,
                venue = venueName,
            };

            //save booking details model to the session data
            Session["selectedBookingDetails"] = a;

            return PartialView(a);
        }

        //GET: Employee booking details
        [HttpGet]
        public PartialViewResult getEmpBookingDetails(int id)
        {
            //get the selected venue booking object
            var booking = db.Venue_Booking.Where(v => v.Venue_Booking_Seq.Equals(id)).FirstOrDefault();

            //get building name
            var buildingName = (from b in db.Buildings
                                where b.Building_ID.Equals(booking.Building_ID)
                                select b.Building_Name).FirstOrDefault();

            //get campus name
            var campusName = (from c in db.Campus
                              where c.Campus_ID.Equals(booking.Campus_ID)
                              select c.Campus_Name).FirstOrDefault();

            //get venue name
            var venueName = (from v in db.Venues
                             where v.Venue_ID.Equals(booking.Venue_ID)
                             select v.Venue_Name).FirstOrDefault();

            //get person id
            var personID = (from p in db.Venue_Booking_Person
                            where p.Venue_Booking_Seq.Equals(booking.Venue_Booking_Seq)
                            select p.Person_ID).FirstOrDefault();

            //get available booking status
            ViewBag.Booking_Status1 = new SelectList(db.Booking_Status, "Booking_Status1", "Booking_Status1");


            //create an instance of the view model
            BookingDetailsModel a = new BookingDetailsModel
            {
                booking_seq = booking.Venue_Booking_Seq,
                person_id = personID,
                type = booking.Description,
                building = buildingName,
                campus = campusName,
                date = booking.DateTime_From.ToShortDateString(),
                timeslot = booking.DateTime_From.TimeOfDay + " - " + booking.DateTime_To.TimeOfDay,
                venue = venueName,
            };

            //save booking details model to the session data
            Session["selectedBookingDetails"] = a;

            return PartialView(a);
        }

        // GET : Cancel selected booking
        [HttpGet]
        public ActionResult cancelBooking()
        {
            return View();
        }

        // POST: Cancel selected booking
        [HttpGet]
        public ActionResult captureCancel()
        {
            var a = (BookingDetailsModel)Session["selectedBookingDetails"];

            //get selected booking object from database
            var cancelledBooking = db.Venue_Booking.Where(b => b.Venue_Booking_Seq.Equals(a.booking_seq)).FirstOrDefault();
            var cancelledPersonBooking = db.Venue_Booking_Person.Where(p => p.Venue_Booking_Seq.Equals(a.booking_seq)).FirstOrDefault();

            //change booking status to cancelled
            cancelledBooking.Booking_Status = "Cancelled";
            cancelledPersonBooking.Attendee_Status = "Cancelled";

            //capture the cancellation
            db.Entry(cancelledBooking).State = EntityState.Modified;
            db.Entry(cancelledPersonBooking).State = EntityState.Modified;
            db.SaveChanges();

            //set notification information
            TempData["Message"] = "Booking successfuly cancelled";
            TempData["classStyle"] = "success";

            //go back to the main view bookings folder
            var site = Url.Action("ViewBookings", "Booking");
            return Content(site);
        }

        // GET: Get buildings that match selected campus
        public JsonResult getBuildingsList(int id)
        {
            var buildingQuery = db.Buildings.Where(b => b.Campus_ID.Equals(id));
            var buildingList = buildingQuery.ToList();
            var jsonList = from b in buildingList
                           select new
                           {
                               id = b.Building_ID,
                               text = b.Building_Name,
                           };

            var rows = jsonList.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //GET: Get levels of selected building
        public JsonResult getLevelList(int id)
        {
            var levelQuery = db.Building_Floor.Where(l => l.Building_ID.Equals(id));
            var levelList = levelQuery.ToList();
            var jsonList = from a in levelList
                           select new
                           {
                               id = a.Building_Floor_ID,
                               text = a.Floor_Name,
                           };

            var rows = jsonList.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //GET: Get venues of selected level
        public JsonResult getVenueList(int id)
        {
            var venueQuery = db.Venues.Where(v => v.Building_Floor_ID.Equals(id));
            var venueList = venueQuery.ToList();
            var jsonList = from a in venueList
                           select new
                           {
                               id = a.Venue_ID,
                               text = a.Venue_Name,
                           };

            var rows = jsonList.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //GET: Get venues for selected building that are available
        public JsonResult getUpdateVenues(DateTime start, DateTime time, int building, int campus, int length)
        {
            //get the time and date components
            var bookingTime = time.TimeOfDay;
            var booingDate = start.Date;

            //calculate the start time of the new session
            DateTime startDateTime = new DateTime(booingDate.Year, booingDate.Month, booingDate.Day, bookingTime.Hours, bookingTime.Minutes, bookingTime.Seconds);

            //calculate the end time of the new session
            TimeSpan duration = new TimeSpan(0, length, 0);
            DateTime endDateTime = startDateTime.Add(duration);

            var venues = db.findBookingVenuesFunc(startDateTime, endDateTime, "Discussion", campus);

            var venueList = venues.ToList();

            var newVenueList = (from a in venueList
                                where a.Building_ID.Equals(building)
                                select a).ToList() ;

            var jsonList = from a in newVenueList
                           select new
                           {
                               id = a.Venue_ID,
                               text = a.Venue_Name,
                           };

            var rows = jsonList.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }

        //GET: Update booking status
        public void updateStatus(string status)
        {
            var a = (BookingDetailsModel)Session["selectedBookingDetails"];

            var updatedBooking = (from b in db.Venue_Booking
                                  where b.Venue_Booking_Seq.Equals(a.booking_seq)
                                  select b).FirstOrDefault();

            var updatedPersonBooking = (from p in db.Venue_Booking_Person
                                        where p.Venue_Booking_Seq.Equals(a.booking_seq)
                                        select p).FirstOrDefault();

            updatedBooking.Booking_Status = status;
            updatedPersonBooking.Attendee_Status = status;

            db.Entry(updatedBooking).State = EntityState.Modified;
            db.Entry(updatedPersonBooking).State = EntityState.Modified;

            db.SaveChanges();
        }

        //GET: Filter venues
        public void filterVenues(IEnumerable<int> characteristics, int capacity)
        {

            //local list of all venues available before filtering
            var details = (DiscussionRoomBooking)Session["details"];
            var venues = db.findBookingVenuesFunc(details.date, details.endDate, "Discussion", details.campus_ID).ToList();

            if (characteristics.Any())
            {
                //get list of all venues venue_id
                var venueListId = (from a in venues
                                   select a.Venue_ID).ToList();

                //get all venue characteristic objects relating to the available venues
                var venueCharacteristics = (from v in db.Venue_Characteristic
                                            where venueListId.Contains(v.Venue_ID)
                                            select v).ToList();

                //get all venue characteristics that match the criteria
                var filteredCharacteristics = (from w in venueCharacteristics
                                               where characteristics.Contains(w.Characteristic_ID)
                                               select w.Venue_ID).ToList();

                //get new list of venues that match the characteristics
                venues = (from venue in db.Venues
                          where filteredCharacteristics.Contains(venue.Venue_ID)
                          select venue).ToList();

                var characteristicCounter = characteristics.Count();
                int loopCounter = 0;
                List<Venue> loopList = new List<Venue>();

                foreach (var item in venues)
                {
                    loopCounter = 0;
                    foreach (var charac in venueCharacteristics)
                    {
                        if (characteristics.Contains(charac.Characteristic_ID) && charac.Venue_ID.Equals(item.Venue_ID))
                        {
                            loopCounter = loopCounter + 1;
                        }
                    }
                    if (loopCounter.Equals(characteristicCounter))
                    {
                        loopList.Add(item);
                    }
                }

                venues = loopList;
            }

            if (capacity > 0)
            {
                venues = (from venue in venues
                          where venue.Capacity.Equals(capacity)
                          select venue).ToList();
            }
                 
            Session["venues"] = venues;
        }

        //GET: Update booking
        [HttpGet]
        public ActionResult updateBookingDetails()
        {
            var bookingDetails = (BookingDetailsModel)Session["selectedBookingDetails"];

            var booking = db.Venue_Booking.Where(b => b.Venue_Booking_Seq.Equals(bookingDetails.booking_seq)).FirstOrDefault();

            UpdateBookingModel a = new UpdateBookingModel
            {
                booking_seq = bookingDetails.booking_seq,
                building = bookingDetails.building,
                campus = bookingDetails.campus,
                date = bookingDetails.date,
                startDate = booking.DateTime_From,
                endDate = booking.DateTime_To,
                person_id = bookingDetails.person_id,
                time = booking.DateTime_From.TimeOfDay.ToString(),
                venue = bookingDetails.venue,
                campus_id = booking.Campus_ID,
                building_id = booking.Building_ID,
                building_floor_id = booking.Building_Floor_ID,
                venue_id = booking.Venue_ID,
                length = booking.DateTime_To.Subtract(booking.DateTime_From).TotalMinutes,
            };
            ViewBag.Campus = from c in db.Campus
                                select c;

            ViewBag.Building = from b in db.Buildings
                               where b.Campus_ID.Equals(booking.Campus_ID)
                               select b;

            var venues = db.findBookingVenuesFunc(booking.DateTime_From, booking.DateTime_To, "Discussion", booking.Campus_ID).ToList();
            var currentVenue = db.Venues.Where(v => v.Venue_ID.Equals(booking.Venue_ID)).FirstOrDefault();
            venues.Add(currentVenue);

            ViewBag.Venue = venues;

            ModelState.Clear();

            return View(a);
        }

        [HttpPost]
        public void updateBookingDetails(UpdateBookingModel model)
        {
            var todayDate = DateTime.Today;
            DateTime datecheck = new DateTime();
            datecheck = Convert.ToDateTime(model.date);
            if (datecheck > todayDate)
            {
                var bookingDetails = (BookingDetailsModel)Session["selectedBookingDetails"];
                var bookingUpdate = db.Venue_Booking.Where(b => b.Venue_Booking_Seq.Equals(bookingDetails.booking_seq)).FirstOrDefault();

                var test = bookingUpdate.Venue_Booking_Seq;

                //get the time and date components
                DateTime bookingTime = Convert.ToDateTime(model.time);
                var timeTimespan = bookingTime.TimeOfDay;
                var bookingDate = datecheck;

                //calculate the start time of the new session
                DateTime startDateTime = new DateTime(bookingDate.Year, bookingDate.Month, bookingDate.Day, timeTimespan.Hours, timeTimespan.Minutes, timeTimespan.Seconds);

                //calculate the end time of the new session
                TimeSpan duration = new TimeSpan(0, Convert.ToInt32(model.length), 0);
                DateTime endDateTime = startDateTime.Add(duration);

                var buildingFloorId = (from b in db.Building_Floor
                                       where b.Building_ID.Equals(model.building_id) && b.Campus_ID.Equals(model.campus_id)
                                       select b.Building_Floor_ID).FirstOrDefault();

                bookingUpdate.DateTime_From = startDateTime;
                bookingUpdate.DateTime_To = endDateTime;
                bookingUpdate.Campus_ID = model.campus_id;
                bookingUpdate.Building_ID = model.building_id;
                bookingUpdate.Building_Floor_ID = buildingFloorId;
                bookingUpdate.Venue_ID = model.venue_id;

                db.Entry(bookingUpdate).State = EntityState.Modified;
                db.SaveChanges();

            }
            else
            {
                var bookingDetails = (BookingDetailsModel)Session["selectedBookingDetails"];
                var bookingUpdate = db.Venue_Booking.Where(b => b.Venue_Booking_Seq.Equals(bookingDetails.booking_seq)).FirstOrDefault();

                TempData["Message"] = "Invalid date entry. Date is in the past";
                TempData["classStyle"] = "warning";
                ViewBag.Campus = from c in db.Campus
                                 select c;

                ViewBag.Building = from b in db.Buildings
                                   where b.Campus_ID.Equals(bookingUpdate.Campus_ID)
                                   select b;

                var venues = db.findBookingVenuesFunc(bookingUpdate.DateTime_From, bookingUpdate.DateTime_To, "Discussion", bookingUpdate.Campus_ID).ToList();
                var currentVenue = db.Venues.Where(v => v.Venue_ID.Equals(bookingUpdate.Venue_ID)).FirstOrDefault();
                venues.Add(currentVenue);

                ViewBag.Venue = venues;
                RedirectToAction("updateBookingDetails", "Booking", model);
            }
        }

        [HttpGet]
        public ActionResult bookTrainingSess()
        {
            var categories = (from c in db.Categories
                              select c);

            Session["categories"] = categories;

            return View();
        }

        [HttpGet]
        public PartialViewResult getTopics(int id)
        {
            var topics = db.Topic_Category.Where(t => t.Category_ID == id).Include(t => t.Topic);

            TempData["matchingTopics"] = topics;

            return PartialView();
        }

        [HttpGet]
        public PartialViewResult getAvailableTrainingSess(int id)
        {
            var trainingSessions = db.Venue_Booking.Where(b => b.Booking_Type.Booking_Type_Name == "Training" && b.Booking_Status == "Confirmed" && b.Topic_Seq == id).Include(v => v.Venue).ToList();

            var bookings = from a in trainingSessions
                           select new BookTrainingSessionModel
                           {
                               date = a.DateTime_From.ToShortDateString(),
                               timeslot = a.DateTime_From.ToShortTimeString() + " - " + a.DateTime_To.ToShortTimeString(),
                               campus = (from c in db.Campus
                                         where c.Campus_ID == a.Campus_ID
                                         select c.Campus_Name).FirstOrDefault(),
                               building = (from b in db.Buildings
                                           where b.Building_ID == a.Building_ID
                                           select b.Building_Name).FirstOrDefault(),
                               venue = a.Venue.Venue_Name,
                               id = a.Venue_Booking_Seq,
                           };

            TempData["trainingSessions"] = bookings;

            return PartialView();
        }

        [HttpGet]
        public void sessionSelect(int id)
        {
            Session["studentSelectedSess"] = id;
        }

        [HttpGet]
        public PartialViewResult confirmStudentTraining()
        {
            var id = (int)Session["studentSelectedSess"];

            var trainingSession = db.Venue_Booking.Where(b => b.Venue_Booking_Seq == id).ToList();

            var bookings = from a in trainingSession
                           select new BookTrainingSessionModel
                           {
                               date = a.DateTime_From.ToShortDateString(),
                               timeslot = a.DateTime_From.ToShortTimeString() + " - " + a.DateTime_To.ToShortTimeString(),
                               campus = (from c in db.Campus
                                         where c.Campus_ID == a.Campus_ID
                                         select c.Campus_Name).FirstOrDefault(),
                               building = (from b in db.Buildings
                                           where b.Building_ID == a.Building_ID
                                           select b.Building_Name).FirstOrDefault(),
                               venue = a.Venue.Venue_Name,
                               id = a.Venue_Booking_Seq,
                           };

            var booking = bookings.FirstOrDefault();

            TempData["booking"] = booking;

            return PartialView();
        }

        [HttpPost]
        public void captureStudentTraining()
        {
            var id = (int)Session["studentSelectedSess"];

            Venue_Booking_Person a = new Venue_Booking_Person
            {
                Person_ID = User.Identity.Name,
                Venue_Booking_Seq = id,
                Certificate_Ind = 0,
                Attendee_Type = "Student",
                Attendee_Status = "Active",
            };

            db.Venue_Booking_Person.Add(a);

            db.SaveChanges();
        }
    }
}