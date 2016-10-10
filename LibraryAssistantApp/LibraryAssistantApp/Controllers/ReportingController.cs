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
        [Authorize(Roles ="Admin")]
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