using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    [Authorize(Roles = "Admin, Employee")]
    public class ReportingController : Controller
    {
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        [HttpGet]
        public ActionResult personSession()
        {
            var persons = (from a in db.Person_Session_Log
                           select a.Registered_Person).Distinct().ToList();

            var sessions = (from b in db.Person_Session_Log
                            select b).ToList();

            List<PersonSessionReportModel> reportList = new List<PersonSessionReportModel>();

            foreach (var person in persons)
            {
                PersonSessionReportModel reportPerson = new PersonSessionReportModel();

                reportPerson.person = person;
                reportPerson.count = 0;

                foreach (var session in sessions)
                {
                    if(session.Person_ID == person.Person_ID)
                    {
                        TimeSpan time = session.Login_DateTime.TimeOfDay;
                        TimeSpan end = session.Logout_DateTime.TimeOfDay;
                        TimeSpan duration = end - time;
                        reportPerson.totalTime = reportPerson.totalTime + duration;
                        reportPerson.count = reportPerson.count + 1;
                    }
                }

                reportList.Add(reportPerson);
            }

            return View(reportList);
        }

        [HttpGet]
        public ActionResult documentAccess()
        {
            var documentsSeq = (from d in db.Document_Access_Log
                             select d.Document_Seq).Distinct().ToList();

            var documents = (from doc in db.Document_Repository
                             where documentsSeq.Contains(doc.Document_Seq)
                             select doc).ToList();

            var sessions = (from s in db.Document_Access_Log
                            select s).ToList();

            var documentList = new List<DocumentAccessReportModel>();

            foreach (var document in documents)
            {
                DocumentAccessReportModel reportItem = new DocumentAccessReportModel();
                reportItem.document = document;
                reportItem.access = 0;

                foreach(var session in sessions)
                {
                    if (session.Document_Seq == document.Document_Seq)
                    {
                        reportItem.access = reportItem.access + 1;
                    }
                }

                documentList.Add(reportItem);
            }

            return View(documentList);
        }

        public ActionResult documentAccessReport()
        {
            var viewModel = new DocumentAccess();
            var doc_acc = new List<Document_Access_Log>();
            var typeslist = new List<DocType>();
            var doclist = new List<Doc>();
            var personlist = new List<DocPerson>();
            var accesslist = new List<DocAcc>();
            var runcount1 = 0;
            var runcount2 = 0;
            foreach (var item in db.Document_Repository)
            {
                if (db.Document_Access_Log.Any(x => x.Document_Seq == item.Document_Seq))
                {
                    var type = db.Document_Type.Find(item.Document_Type_ID);
                    if (!typeslist.Any(x => x.doc_type == type.Document_Type_Name))
                    {
                        var doctype = new DocType();
                        doctype.doc_type = type.Document_Type_Name;
                        var doctype_ = (db.Document_Access_Log.Where(x => x.Document_Repository.Document_Type_ID == type.Document_Type_ID));
                        var unique = (doctype_.GroupBy(x => x.Document_Seq)).Count();
                        runcount1 = runcount1 + unique;
                        doctype.doc_count = runcount1;
                        typeslist.Add(doctype);
                    }
                    var doc = new Doc();
                    doc.doc_id = item.Document_Seq;
                    doc.doc_name = db.Document_Repository.Find(item.Document_Seq).Document_Name;
                    var ext = (from d in db.Document_Extension
                               where d.Document_Extension_ID == item.Document_Extension_ID
                               select d.Extension_Type).Single();
                    doc.doc_ext = ext;
                    var count = (db.Document_Access_Log.Where(x => x.Document_Seq == doc.doc_id)).Count();
                    runcount2 = runcount2 + count;
                    doc.count = runcount2;
                    var doc_access = (db.Document_Access_Log.Where(x => x.Document_Seq == item.Document_Seq)).ToList();
                    //var test = (db.Document_Access_Log.Where(x => x.Document_Seq == item.Document_Seq).Select(x => x.Person_Session_Log).Distinct()).ToList();
                    foreach (var da in doc_access)
                    {
                        //attempted code at creating the 3rd control break at person

                        //var id = (from b in db.Person_Session_Log
                        //          where b.Session_ID == da.Session_ID
                        //          select b.Person_ID).Single();
                        //var person_count = (from f in db.Person_Session_Log
                        //                    where f.Session_ID == da.Session_ID
                        //                    select f.Person_ID).Distinct().Count();
                        //doc.person_count = person_count;
                        //var docperson = new DocPerson();
                        //var person = (from b in db.Person_Session_Log
                        //              where b.Session_ID == da.Session_ID
                        //              select b.Person_ID).Single();
                        //docperson.person_id = person;
                        //var pName = (from c in db.Registered_Person
                        //             where c.Person_ID == person
                        //             select c.Person_Name).Single();
                        //docperson.person_name = pName;
                        //var access = (from g in db.Person_Session_Log
                        //              where g.Session_ID == da.Session_ID
                        //              select g);
                        //int i = 0;
                        //foreach (var p in access)
                        //{
                        //    if (p.Person_ID == person)
                        //    {
                        //        i = i + 1;
                        //    }
                        //}
                        //docperson.access_count = i;
                        //var acc = new DocAcc();
                        //acc.accessed = da.Access_DateTime;
                        //personlist.Add(docperson);
                        //accesslist.Add(acc);
                        doc_acc.Add(da);
                    }
                    doclist.Add(doc);

                }
            }
            viewModel.doc_types = typeslist;
            viewModel.docs = doclist;
            viewModel.doc_access = doc_acc;
            //viewModel.doc_persons = personlist;
            //viewModel.doc_acc = accesslist;
            return View(viewModel);
        }

        [HttpGet]
        public ActionResult personSessionAction()
        {
            var actions = (from a in db.Person_Session_Action_Log
                           select a.Action).Distinct().ToList();

            var sessions = (from s in db.Person_Session_Action_Log
                           select s);

            var reportList = new List<ActionAccessModel>();

            foreach (var action in actions)
            {
                ActionAccessModel report = new ActionAccessModel();

                report.action = action;
                report.access = 0;

                foreach (var session in sessions)
                {
                    if (session.Action_ID == action.Action_ID)
                    {
                        report.access = report.access + 1;
                    }
                }

                reportList.Add(report);
            }

            return View(reportList);
        }

        [HttpGet]
        public ActionResult venueUsage()
        {
            var venueIds = (from v in db.Venue_Booking
                          select v.Venue_ID).Distinct().ToList();

            var venues = (from v in db.Venues
                          where venueIds.Contains(v.Venue_ID)
                          select v).ToList();

            var sessions = (from s in db.Venue_Booking
                           select s).ToList();

            var campuses = db.Campus;

            var buildings = db.Buildings;

            var reportList = new List<VenueUsageModel>();

            foreach (var venue in venues)
            {
                VenueUsageModel report = new VenueUsageModel();
                report.venue = venue;
                report.count = 0;
                report.campus = (from c in campuses
                                 where c.Campus_ID == venue.Campus_ID
                                 select c.Campus_Name).FirstOrDefault();
                report.building = (from c in buildings
                                   where c.Building_ID == venue.Building_ID
                                   select c.Building_Name).FirstOrDefault();


                foreach (var session in sessions)
                {
                    if (session.Venue_ID == venue.Venue_ID)
                    {
                        report.count = report.count + 1;
                    }
                }

                reportList.Add(report);
            }

            return View(reportList);
        }

        [HttpGet]
        public ActionResult trainingAttendance()
        {
            var trainingSeq = (from t in db.Venue_Booking
                        where t.Booking_Type.Booking_Type_Name == "Training" && t.Booking_Status == "Complete"
                        select t.Venue_Booking_Seq).ToList();

            var trainings = (from t in db.Venue_Booking
                            where t.Booking_Type.Booking_Type_Name == "Training" && t.Booking_Status == "Complete"
                            select t).ToList();

            var attendees = (from a in db.Venue_Booking_Person
                             where trainingSeq.Contains(a.Venue_Booking_Seq) && a.Attendee_Type == "Student"
                             select a).ToList();

            var reportList = new List<TrainingSessionAttendance>();

            foreach (var training in trainings)
            {
                var report = new TrainingSessionAttendance();
                report.training = training;
                report.attended = 0;

                foreach(var attendent in attendees)
                {
                    if (attendent.Venue_Booking_Seq == training.Venue_Booking_Seq && attendent.Attendee_Status == "Attended")
                    {
                        report.attended = report.attended + 1;
                    }
                }

                reportList.Add(report);
            }

            return View(reportList);
        }

        [HttpGet]
        public ActionResult BookingReport(int? year)
        {
            if (year == null)
            {
                year = DateTime.Now.Year;
            }
            var viewModel = new BookingsReport();
            var bookingslist = new List<Venue_Booking>();
            bookingslist = (from a in db.Venue_Booking
                           orderby a.DateTime_From descending
                           select a).ToList();
            var groupedDate = from b in bookingslist
                              where b.DateTime_From.Year == year
                           group b by new { month = b.DateTime_From.Month, year = b.DateTime_From.Year } into d
                           select new { dt = string.Format("{0}/{1}", d.Key.month, d.Key.year), dcount = d.Count(), d.Key.month };
            var monthYear = "";
            var bookings = new List<BookingR>();
            foreach (var item in bookingslist)
            {
                var monthYear2 = string.Format("{0}/{1}", item.DateTime_From.Month, item.DateTime_From.Year);
                if (monthYear2 != monthYear)
                {
                    monthYear = string.Format("{0}/{1}", item.DateTime_From.Month, item.DateTime_From.Year);
                    if (groupedDate.Any(x => x.dt == monthYear))
                    {
                        var booking = new BookingR();
                        booking.monthname = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(item.DateTime_From.Month);
                        booking.monthyear = monthYear;
                        var singlemonth = groupedDate.Where(x => x.dt == monthYear);
                        foreach (var m in singlemonth)
                        {
                            booking.total = booking.total + m.dcount;
                            booking.month = m.month;
                        }
                        var type = from b in bookingslist
                                   where string.Format("{0}/{1}", b.DateTime_From.Month, b.DateTime_From.Year) == monthYear
                                   select b;
                        foreach (var t in type)
                        {
                            if (t.Booking_Type_Seq == 1)
                            {
                                booking.discussionCount += 1;
                            }
                            if (t.Booking_Type_Seq == 2)
                            {
                                booking.trainingCount += 1;
                            }
                            if (t.Booking_Status == "Active")
                            {
                                booking.activeCount += 1;
                            }
                            if (t.Booking_Status == "Cancelled")
                            {
                                booking.cancelledCount += 1;
                            }
                            if (t.Booking_Status == "Complete")
                            {
                                booking.completeCount += 1;
                            }
                            if (t.Booking_Status == "Confirmed")
                            {
                                booking.confirmedCount += 1;
                            }
                            if (t.Booking_Status == "Tenative")
                            {
                                booking.tenativeCount += 1;
                            }
                            var persons = (from p in db.Venue_Booking_Person
                                          where p.Venue_Booking_Seq == t.Venue_Booking_Seq
                                          select p.Attendee_Type).ToList();
                            foreach (var at in persons)
                            {
                                if (at == "Trainer")
                                {
                                    booking.trainerCount += 1;
                                }
                                if (at == "Student")
                                {
                                    booking.studentCount += 1;
                                }
                            }
                        }
                        bookings.Add(booking);
                    }
                }  
            }
            var bookTotal = new BookingTotal();
            foreach (var item in bookings)
            {
                bookTotal.activeTotal += item.activeCount;
                bookTotal.bookingTotal += item.total;
                bookTotal.cancelledTotal += item.cancelledCount;
                bookTotal.completeTotal += item.completeCount;
                bookTotal.confirmedTotal += item.confirmedCount;
                bookTotal.discussionTotal += item.discussionCount;
                bookTotal.studentTotal += item.studentCount;
                bookTotal.tenativeTotal += item.tenativeCount;
                bookTotal.trainerTotal += item.trainerCount;
                bookTotal.trainingTotal += item.trainingCount;
            }
            viewModel.Total = bookTotal;
            viewModel.bookingsreportlist = (from b in bookings
                                           orderby b.month ascending
                                           select b).ToList();

            var groupedYear = (from b in bookingslist
                               where b.DateTime_From.Year != year
                              group b by new { year = b.DateTime_From.Year } into d
                              select new { groupYear = string.Format("{0}", d.Key.year) }).ToList();
            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (var item in groupedYear)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = item.groupYear,
                    Value = item.groupYear
                };
                selectListItems.Add(selectListItem);
            }

            ViewBag.YearSelected = year;
            ViewBag.Year = selectListItems;

            return View(viewModel);
        }       
    }
}