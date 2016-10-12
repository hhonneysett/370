using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class ReportingController : Controller
    {
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        [HttpGet]
        [Authorize(Roles = "Admin")]
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
                reportPerson.totalTime = 0;
                reportPerson.count = 0;

                foreach (var session in sessions)
                {
                    if(session.Person_ID == person.Person_ID)
                    {
                        TimeSpan time = session.Login_DateTime.TimeOfDay;
                        TimeSpan end = session.Logout_DateTime.TimeOfDay;
                        TimeSpan duration = end - time;
                        double dDuration = duration.TotalHours;
                        double round = Math.Round(dDuration, 2);
                        reportPerson.totalTime = reportPerson.totalTime + round;
                        reportPerson.count = reportPerson.count + 1;
                    }
                }

                reportList.Add(reportPerson);
            }

            return View(reportList);
        }

        [HttpGet]
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles ="Admin")]
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
        [Authorize(Roles ="Admin")]
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
       
    }
}