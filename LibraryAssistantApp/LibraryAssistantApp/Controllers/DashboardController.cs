using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryAssistantApp.Controllers
{
    public class DashboardController : Controller
    {
        //instance of db
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Dashboard
        public ActionResult Index()
        {
            var bookings = db.Venue_Booking.ToList();
            var today = bookings.Where(b => b.DateTime_From.Date == DateTime.Today.Date).Count();

            var members = db.Registered_Person.ToList();
            var membersYear = members.Where(r => r.Person_Registration_DateTime.Year == DateTime.Now.Year).Count();

            var attended = db.Venue_Booking_Person.Where(b => b.Attendee_Status == "Attended").Count();
            var absent = db.Venue_Booking_Person.Where(b => b.Attendee_Status == "Absent").Count();
            var totalTraining = attended + absent;
            var percAtt = "";
            if (totalTraining != 0 && attended != 0)
            {
                var attendance = Convert.ToDecimal(attended) / Convert.ToDecimal(totalTraining) * 100m;
                percAtt = Math.Round(attendance).ToString() + "%";
            }
            else
            {
                percAtt = "N/A";
            }

            var startDate = db.Person_Session_Log.OrderBy(c => c.Login_DateTime).FirstOrDefault();
            var endDate = db.Person_Session_Log.OrderByDescending(c => c.Login_DateTime).FirstOrDefault();
            var days = (endDate.Login_DateTime - startDate.Login_DateTime).Days;
            if (days == 0)
            {
                days = 1;
            }
            var sessions = db.Person_Session_Log.Count();
            var avg = Math.Round((Convert.ToDecimal(sessions) / Convert.ToDecimal(days))).ToString();

            HttpContext.Application.Lock();
            var online = (int)HttpContext.Application["OnlineUsers"];
            HttpContext.Application.UnLock();

            var problems = db.Venue_Problem.Where(p => p.Status == "Closed").Count();

            ViewBag.Bookings = today;
            ViewBag.Members = membersYear;
            ViewBag.Attendance = percAtt;
            ViewBag.Daily = avg;
            ViewBag.Online = online;
            ViewBag.Problems = problems;

            return View();
        }

        //get usage graph data
        public JsonResult getUsage()
        {
            var sessions = db.Person_Session_Log.ToList();
            var yearSessions = sessions.Where(s => s.Login_DateTime.Year == DateTime.Now.Year).ToList();
            var startDate = yearSessions.OrderBy(c => c.Login_DateTime).FirstOrDefault();
            var endDate = yearSessions.OrderByDescending(c => c.Login_DateTime).FirstOrDefault();

            var list = new List<monthList>();

            foreach (DateTime obj in EachDay(startDate.Login_DateTime, endDate.Login_DateTime))
            {
                var month = new monthList();
                month.month = obj.ToString("yyyy-MM");
                var counter = 0;

                foreach(var session in yearSessions)
                {
                    if (session.Login_DateTime.Month == obj.Month)
                    {
                        counter = counter + 1;
                    }
                }

                month.count = counter;
                list.Add(month);
            }

            var jsonObj = list.ToArray();
            return Json(jsonObj, JsonRequestBehavior.AllowGet);

        }

        //get booking types
        public JsonResult getBookingTypes()
        {
            var tId = db.Booking_Type.Where(b => b.Booking_Type_Name == "Training").FirstOrDefault();
            var dId = db.Booking_Type.Where(b => b.Booking_Type_Name == "Discussion Room").FirstOrDefault();
            var training = db.Venue_Booking.Where(b => b.Booking_Type_Seq == tId.Booking_Type_Seq).Count();
            var discussion = db.Venue_Booking.Where(b => b.Booking_Type_Seq == dId.Booking_Type_Seq).Count();
            List<typeList> list = new List<typeList>();

            var trainingOb = new typeList
            {
                type = "Training Session Bookings",
                count = training,
            };

            var discussionOb = new typeList
            {
                type = "Discussion Room Bookings",
                count = discussion,
            };

            list.Add(trainingOb);
            list.Add(discussionOb);

            return Json(list.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //run through each month
        public IEnumerable<DateTime> EachDay(DateTime from, DateTime thru)
        {
            for (var month = from.Date; month.Date <= thru.Date; month = month.AddMonths(1))
                yield return month;
        }
    }
}