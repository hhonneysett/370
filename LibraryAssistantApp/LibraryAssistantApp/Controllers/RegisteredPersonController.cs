using LibraryAssistantApp.Models;
using System;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class RegisteredPersonController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: RegisteredPerson
        public ActionResult Index()
        {
            var registered_Person = db.Registered_Person.Include(r => r.Person_Level).Include(r => r.Person_Title1).Include(r => r.Person_Type1);
            return View(registered_Person.ToList());
        }

        // GET: RegisteredPerson/Details/5
        public ActionResult Details()
        {
            var id = User.Identity.Name;

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Registered_Person registered_Person = db.Registered_Person.Find(id);
            if (registered_Person == null)
            {
                return HttpNotFound();
            }
            var personTopicList = from a in db.Person_Topic
                                  where a.Person_ID.Equals(User.Identity.Name)
                                  select a;

            return View(registered_Person);
        }

        // GET: RegisteredPerson/ValidatePerson
        public ActionResult ValidatePerson()
        {
            return View();
        }

        // POST: RegisteredPerson/ValidatePerson
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ValidatePerson([Bind(Include ="Person_ID")]Registered_Person registered_person)
        {
                var exists = db.Registered_Person.Any(p => p.Person_ID.Equals(registered_person.Person_ID));

                if (exists)
                {
                     TempData["Message"] = "You are already a registered user.";
                    return RedirectToAction("Login", "MyAccount");
                }
                else
                {
                    TempData["tempPerson"] = registered_person;
                    return RedirectToAction("RegisterStudent");
                }
                     
        }

        // GET: RegisteredPerson/RegisterStudent
        public ActionResult RegisterStudent()
        {
            ViewBag.Level_ID = new SelectList(db.Person_Level, "Level_ID", "Level_Name");
            ViewBag.Person_Title = new SelectList(db.Person_Title, "Person_Title1", "Person_Title1");
            return View();
        }
        
        // POST: RegisteredPerson/RegisterStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult RegisterStudent(CreatePersonModel b)
        {
            if (ModelState.IsValid)
            {
                Registered_Person a = new Registered_Person();

                var c = (Registered_Person)TempData["tempPerson"];

                a.Person_ID = c.Person_ID;
                a.Person_Name = b.Person_Name;
                a.Person_Surname = b.Person_Surname;
                a.Person_Email = b.Person_Email;
                a.Person_Password = b.Person_Password;
                a.Level_ID = b.Level_ID;
                a.Person_Title = b.Person_Title;
                a.Person_Type = "Student";
                a.Person_Registration_DateTime = DateTime.Now;
                a.Person_Registration_Status = "Pending";

                db.Registered_Person.Add(a);
                db.SaveChanges();
                SendEmail(a);   

                return RedirectToAction("Index");
            }

            ViewBag.Level_ID = new SelectList(db.Person_Level, "Level_ID", "Level_Name", b.Level_ID);
            ViewBag.Person_Title = new SelectList(db.Person_Title, "Person_Title1", "Person_Title1", b.Person_Title);
            return View(b);
        }

        //send email to registered person
        public void SendEmail(Registered_Person registered_Person)
        {
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            string personActivation = "";

            message.From = new MailAddress("uplibraryassistant@gmail.com");
            message.To.Add(registered_Person.Person_Email);
            message.Subject = "Account Activation";
            message.Body = "Hi, " + registered_Person.Person_Name + " your email activation link is here </b>" + personActivation;
            message.IsBodyHtml = true;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
            client.Send(message);
        }

        // GET: RegisteredPerson/Edit/5
        public ActionResult UpdateDetails()
        {
            var identity = User.Identity.Name;
            if (identity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var registered_Person = (from a in db.Registered_Person
                                                  where a.Person_ID == identity
                                                  select a).FirstOrDefault();
            if (registered_Person == null)
            {
                return HttpNotFound();
            }

            UpdatePersonModel b = new UpdatePersonModel();
            b.Person_Title = registered_Person.Person_Title;
            b.Person_Name = registered_Person.Person_Name;
            b.Person_Surname = registered_Person.Person_Surname;
            b.Person_Email = registered_Person.Person_Email;
            b.Level_ID = registered_Person.Level_ID;

            ViewBag.Level = new SelectList(db.Person_Level, "Level_ID", "Level_Name", registered_Person.Level_ID);
            ViewBag.Title = new SelectList(db.Person_Title, "Person_Title1", "Person_Title1", registered_Person.Person_Title);
            return View(b);
        }

        // POST: RegisteredPerson/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdateDetails(UpdatePersonModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = User.Identity.Name;
                var count = db.Registered_Person.Count(me => me.Person_Email == model.Person_Email && me.Person_ID != identity);
                
                if (count == 0)
                {
                    var registered_Person = (from a in db.Registered_Person
                                             where a.Person_ID == identity
                                             select a).FirstOrDefault();

                    registered_Person.Person_Name = model.Person_Name;
                    registered_Person.Person_Surname = model.Person_Surname;
                    registered_Person.Person_Email = model.Person_Email;
                    registered_Person.Level_ID = model.Level_ID;
                    registered_Person.Person_Title = model.Person_Title;

                    db.Entry(registered_Person).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Details");
                }
                else
                {
                    ViewBag.Level = new SelectList(db.Person_Level, "Level_ID", "Level_Name", model.Level_ID);
                    ViewBag.Title = new SelectList(db.Person_Title, "Person_Title1", "Person_Title1", model.Person_Title);
                    TempData["Message"] = "Email address already exists on the system";
                    return View(model);
                }              
            }
            ViewBag.Level = new SelectList(db.Person_Level, "Level_ID", "Level_Name", model.Level_ID);
            ViewBag.Title = new SelectList(db.Person_Title, "Person_Title1", "Person_Title1", model.Person_Title);
            return View(model);
        }

        public ActionResult UpdatePassword()
        {
            var identity = User.Identity.Name;

            if (identity == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var registered_Person = (from a in db.Registered_Person
                                     where a.Person_ID == identity
                                     select a).FirstOrDefault();

            if (registered_Person == null)
            {
                return HttpNotFound();
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UpdatePassword(UpdatePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = User.Identity.Name;

                var registered_person = (from a in db.Registered_Person
                                         where a.Person_ID.Equals(identity)
                                         select a).FirstOrDefault();
                if (registered_person.Person_Password.Equals(model.CurrentPassword))
                {
                    registered_person.Person_Password = model.NewPassword;
                    db.Entry(registered_person).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Password Updated";
                    return RedirectToAction("Details");
                }
                else
                {
                    TempData["Message"] = "Invalid Password";
                    return View();
                }
                
            }
            model.ConfirmNewPassword = null;          
            return View(model);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult oneTimePin()
        {
            return PartialView();
        }
    }
}
