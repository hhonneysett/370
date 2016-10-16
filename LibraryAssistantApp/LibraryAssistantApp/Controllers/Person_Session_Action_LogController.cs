using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryAssistantApp.Models;

namespace LibraryAssistantApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class Person_Session_Action_LogController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Person_Session_Action_Log
        public ActionResult Index()
        {
            List<AuditLog> log = new List<AuditLog>();
            List < Person_Session_Action_Log > logwithdata= db.Person_Session_Action_Log.ToList();
            int count = db.Person_Session_Action_Log.Count();

            for (int i = 0; i < count; i++)
            {                
                AuditLog mylog = new AuditLog();
                mylog.Action_Performed = logwithdata[i].Action_Performed;
                mylog.TimePerformed = logwithdata[i].Action_DateTime;

                int Session_ID = logwithdata[i].Session_ID;
                string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();
                int Action_ID = logwithdata[i].Action_ID;
                mylog.Username = PersonID;
                mylog.Session_ID = Session_ID;
                mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                log.Add(mylog);
            }

             //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
            ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
            return View(log);
        }

      
        public ActionResult Search (string Names, int? Areas , DateTime  From, string Crud_Operation) 
        {

            if (Crud_Operation != "")
            {
                if (Names == "" && Areas == null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.Where(X => X.Crud_Operation == Crud_Operation).ToList();
                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index",log);
                }
                else if (Names != "" && Areas == null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession && X.Crud_Operation == Crud_Operation).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index", log);
                }
                else if (Names == "" && Areas != null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.Where(X => X.Action_ID == Areas && X.Crud_Operation == Crud_Operation).ToList();
                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index", log);
                }
                else if (Names == "" && Areas == null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.Where(X => X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year && X.Crud_Operation == Crud_Operation).ToList();
                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    ViewBag.DateSearch = From.ToString("D");
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index", log);
                }
                else if (Names == "" && Areas != null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.Where(X => X.Action_ID == Areas && X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year && X.Crud_Operation == Crud_Operation).ToList();
                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    ViewBag.DateSearch = From.ToString("D");
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index", log);
                }
                else if (Names != "" && Areas == null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession && X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year && X.Crud_Operation == Crud_Operation).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    ViewBag.DateSearch = From.ToString("D");
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index", log);
                }
                else if (Names != "" && Areas != null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession && X.Action_ID == Areas && X.Crud_Operation == Crud_Operation).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index", log);
                }
                else if (Names != "" && Areas != null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession && X.Action_ID == Areas && X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year && X.Crud_Operation == Crud_Operation).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    ViewBag.DateSearch = From.ToString("D");
                    ViewBag.Crud_Operation = Crud_Operation;
                    return View("Index", log);
                }

            }
            else
            {
                if (Names == "" && Areas == null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.ToList();
                    int count = db.Person_Session_Action_Log.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    return View("Index", log);
                }
                else if (Names != "" && Areas == null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    return View("Index", log);
                }
                else if (Names == "" && Areas != null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.Where(X => X.Action_ID == Areas).ToList();
                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    return View("Index", log);
                }
                else if (Names == "" && Areas == null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.Where(X => X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year).ToList();
                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    ViewBag.DateSearch = From.ToString("D");
                    return View("Index", log);
                }
                else if (Names == "" && Areas != null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = db.Person_Session_Action_Log.Where(X => X.Action_ID == Areas && X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year).ToList();
                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    //ViewBag.Names = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    ViewBag.DateSearch = From.ToString("D");
                    return View("Index", log);
                }
                else if (Names != "" && Areas == null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession && X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name");
                    ViewBag.DateSearch = From.ToString("D");
                    return View("Index", log);
                }
                else if (Names != "" && Areas != null && From == Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession && X.Action_ID == Areas).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    return View("Index", log);
                }
                else if (Names != "" && Areas != null && From != Convert.ToDateTime("11/11/1111"))
                {
                    List<AuditLog> log = new List<AuditLog>();
                    List<Person_Session_Action_Log> logwithdata = new List<Person_Session_Action_Log>();
                    List<Person_Session_Log> sessions = db.Person_Session_Log.Where(X => X.Person_ID.ToLower().Contains(Names.ToLower())).ToList();

                    for (int i = 0; i < sessions.Count(); i++)
                    {
                        int CurrentSession = sessions[i].Session_ID;
                        List<Person_Session_Action_Log> c = db.Person_Session_Action_Log.Where(X => X.Session_ID == CurrentSession && X.Action_ID == Areas && X.Action_DateTime.Day == From.Day && X.Action_DateTime.Month == From.Month && X.Action_DateTime.Year == From.Year).ToList();
                        logwithdata.AddRange(c);
                    }

                    int count = logwithdata.Count();

                    for (int i = 0; i < count; i++)
                    {
                        AuditLog mylog = new AuditLog();
                        mylog.Action_Performed = logwithdata[i].Action_Performed;
                        mylog.TimePerformed = logwithdata[i].Action_DateTime;

                        int Session_ID = logwithdata[i].Session_ID;
                        string PersonID = db.Person_Session_Log.Where(X => X.Session_ID == Session_ID).Select(Y => Y.Person_ID).Single();

                        mylog.Person_Name = db.Registered_Person.Where(X => X.Person_ID == PersonID).Select(Y => Y.Person_Name).Single();

                        int Action_ID = logwithdata[i].Action_ID;
                        mylog.Area = db.Actions.Where(X => X.Action_ID == Action_ID).Select(Y => Y.Action_Name).Single();
                        mylog.Crud_Operation = logwithdata[i].Crud_Operation;
                        log.Add(mylog);
                    }

                    ViewBag.Names = Names;
                    ViewBag.Areas = new SelectList(db.Actions, "Action_ID", "Action_Name", Areas);
                    ViewBag.DateSearch = From.ToString("D");
                    return View("Index", log);
                }
            }
            return View("Index");
        }
    }
}
