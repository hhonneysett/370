using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryAssistantApp.Controllers
{
    public class RegisteredPersonController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: RegisteredPerson/Details/5
        [Authorize]
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
            var pt = (from t in db.Person_Topic
                      where t.Person_ID.Equals(User.Identity.Name)
                      select t).Include(t => t.Topic);

            var c = (from d in db.Categories
                      select d);

            TempData["categories"] = c;

            TempData["personTopic"] = pt;

            return View(registered_Person);
        }

        // GET: RegisteredPerson/RegisterStudent
        [AllowAnonymous]
        public ActionResult RegisterStudent()
        {
            if (Request.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }
        
        // POST: RegisteredPerson/RegisterStudent
        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult RegisterStudent(CreatePersonModel b)
        {
            if (ModelState.IsValid)
            {
                Registered_Person a = new Registered_Person();

                //hash password
                var hashPassword = FormsAuthentication.HashPasswordForStoringInConfigFile(b.Person_Password, "MD5");

                a.Person_ID = b.Person_ID;
                a.Person_Name = b.Person_Name;
                a.Person_Surname = b.Person_Surname;
                a.Person_Email = b.Person_Email;
                a.Person_Password = hashPassword;
                a.Person_Type = "Student";
                a.Person_Registration_DateTime = DateTime.Now;

                Session["newStudent"] = a;

                return RedirectToAction("oneTimePin");
            }

            return View(b);
        }

        //send one time pin to student 
        [HttpGet]
        [AllowAnonymous]
        public ActionResult oneTimePin()
        {
            var newStudent = (Registered_Person)Session["newStudent"];

            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            //generate one time pin
            int _min = 1000;
            int _max = 9999;
            Random _rdm = new Random();
            var OTP = _rdm.Next(_min, _max);

            //capture one time pin to session
            Session["OTP"] = OTP;

            message.From = new MailAddress("uplibraryassistant@gmail.com");
            message.To.Add(newStudent.Person_Email);
            message.Subject = "Account Activation";
            message.Body = "Hi, your one time pin for registration is: <b>" + OTP + "</b>";
            message.IsBodyHtml = true;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
            client.Send(message);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult oneTimePin(OneTimePin model)
        {
            if (ModelState.IsValid)
            {
                var newStudent = (Registered_Person)Session["newStudent"];
                var personRole = new Person_Role();

                personRole.Person_ID = newStudent.Person_ID;
                personRole.Role_ID = (from r in db.Roles
                                      where r.Role_Name == "Student"
                                      select r.Role_ID).FirstOrDefault();

                db.Registered_Person.Add(newStudent);
                db.Person_Role.Add(personRole);
                db.SaveChanges();

                Session.Remove("newStudent");
                Session.Remove("OTP");

                TempData["Message"] = "Succesfully created an account!";
                TempData["classStyle"] = "success";

                return RedirectToAction("Login", "Account");
            }
            else return View();
        }

        // GET: RegisteredPerson/Edit/5
        [Authorize]
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
            b.Person_Name = registered_Person.Person_Name;
            b.Person_Surname = registered_Person.Person_Surname;
            b.Person_Email = registered_Person.Person_Email;

            return View(b);
        }

        // POST: RegisteredPerson/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
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


                    db.Entry(registered_Person).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["Message"] = "Details successfully updated!";
                    TempData["classStyle"] = "success";
                    return RedirectToAction("Details");
                }
                else
                {
                    TempData["Message"] = "Email address already exists on the system";
                    return View(model);
                }              
            }

            return View(model);
        }

        [Authorize]
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
        [Authorize]
        public ActionResult UpdatePassword(UpdatePasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var identity = User.Identity.Name;

                var registered_person = (from a in db.Registered_Person
                                         where a.Person_ID.Equals(identity)
                                         select a).FirstOrDefault();

                var hashedPass = FormsAuthentication.HashPasswordForStoringInConfigFile(model.CurrentPassword, "MD5");

                if (registered_person.Person_Password.Equals(hashedPass))
                {
                    var newHashed = FormsAuthentication.HashPasswordForStoringInConfigFile(model.NewPassword, "MD5");

                    registered_person.Person_Password = newHashed;
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

        [HttpGet]
        [Authorize]
        public JsonResult getTopics(int? category)
        {
            if (category == null)
            {
                var allTopics = db.Person_Topic.Where(t => t.Person_ID.Equals(User.Identity.Name)).Include(c => c.Topic);

                var allTopicsList = from b in allTopics
                                 select new
                                 {
                                     id = b.Topic_Seq,
                                     text = b.Topic.Topic_Name
                                 };

                var allRows = allTopicsList.ToArray();

                return Json(allRows, JsonRequestBehavior.AllowGet);
            }

            var personTopics = (from i in db.Person_Topic
                                where i.Person_ID == User.Identity.Name
                                select i.Topic_Seq);

            var topics = db.Topic_Category.Where(c => c.Category_ID == category && personTopics.Contains(c.Topic_Seq)).Include(t => t.Topic);


            var topicsList = from b in topics
                             select new
                             {
                                 id = b.Topic_Seq,
                                 text = b.Topic.Topic_Name,
                             };

            var rows = topicsList.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
            
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpGet]
        [Authorize]
        public ActionResult updateFavTopics()
        {
            //get list of topics student already favourites
            var favTopics = db.Person_Topic.Where(t => t.Person_ID == User.Identity.Name).Include(t => t.Topic).ToList();


            //get list of available topics not favourited
            var ts = (from t in favTopics
                      select t.Topic_Seq).ToList();

            var availableTopics = (from t in db.Topics
                                   where !ts.Contains(t.Topic_Seq)
                                   select t).ToList();

            //get list of categories
            var categories = (from c in db.Categories
                              select c);

            //assing the variables to session
            Session["favTopics"] = favTopics;
            Session["availTopics"] = availableTopics;
            Session["categories"] = categories;

            return View();
        }

        [HttpGet]
        [Authorize]
        public JsonResult getAvailTopics(int? category)
        {
            var availTopics = (IEnumerable<Topic>)Session["availTopics"];

            if (category == null)
            {
                var topicList = from a in availTopics
                                select new
                                {
                                    id = a.Topic_Seq,
                                    text = a.Topic_Name,
                                };
                var rows = topicList.ToArray();

                return Json(rows, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var topSeq = (from a in db.Topic_Category
                              where a.Category_ID == category
                              select a.Topic_Seq);

                var filteredTop = from a in availTopics
                                  where topSeq.Contains(a.Topic_Seq)
                                  select a;

                var topList = from b in filteredTop
                           select new
                           {
                               id = b.Topic_Seq,
                               text = b.Topic_Name,
                           };

                var rows = topList.ToArray();

                return Json(rows, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        [Authorize]
        public void addTopic(int id)
        {
            Person_Topic newTopic = new Person_Topic();

            var topic = db.Topics.Where(t => t.Topic_Seq == id).FirstOrDefault();

            newTopic.Person_ID = User.Identity.Name;
            newTopic.Topic_Seq = id;

            db.Person_Topic.Add(newTopic);
            db.SaveChanges();

            //get list of topics student already favourites
            var favTopics = db.Person_Topic.Where(t => t.Person_ID == User.Identity.Name).Include(t => t.Topic).ToList();


            //get list of available topics not favourited
            var ts = (from t in favTopics
                      select t.Topic_Seq).ToList();

            var availableTopics = (from t in db.Topics
                                   where !ts.Contains(t.Topic_Seq)
                                   select t).ToList();

            Session["favTopics"] = favTopics;
            Session["availTopics"] = availableTopics;
        }

        [HttpGet]
        [Authorize]
        public void removeTopic(int id)
        {
            var removeTop = db.Person_Topic.Where(t => t.Person_ID == User.Identity.Name && t.Topic_Seq == id).FirstOrDefault();

            db.Person_Topic.Remove(removeTop);
            db.SaveChanges();

            //get list of topics student already favourites
            var favTopics = db.Person_Topic.Where(t => t.Person_ID == User.Identity.Name).Include(t => t.Topic).ToList();


            //get list of available topics not favourited
            var ts = (from t in favTopics
                      select t.Topic_Seq).ToList();

            var availableTopics = (from t in db.Topics
                                   where !ts.Contains(t.Topic_Seq)
                                   select t).ToList();

            Session["favTopics"] = favTopics;
            Session["availTopics"] = availableTopics;

        }
    }
}
