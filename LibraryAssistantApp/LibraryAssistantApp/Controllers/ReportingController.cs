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
    }
}