using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryAssistantApp.Models;
using System.Web.Security;
using System.Net.Mail;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Json;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace LibraryAssistantApp.Controllers
{
    public class MemberController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();
        // GET: Member
        public ActionResult Index()
        {
            try
            {
                if ((bool)TempData["Show"] == true)
                {
                    TempData["Hidden"] = "";
                    TempData["Show"] = false;
                }
                else
                {
                    TempData["Hidden"] = "Hidden";
                    TempData["Show"] = false;
                }
            }
            catch
            {
                TempData["Hidden"] = "Hidden";
                TempData["Show"] = false;
            }

            var viewModel = new MemberIndexVM();
            viewModel.registered_person = db.Registered_Person
                    .Where(t => t.Person_Type.ToLower() == "student");

            return View("Index", viewModel);
        }
        public PartialViewResult Members(string username, string name, string surname, string email, string roleid)
        {
            var viewModel = new MemberIndexVM();
            viewModel.registered_person = db.Registered_Person
                    .Where(t => t.Person_Type.ToLower() == "student");

            viewModel.registered_person = from q in viewModel.registered_person
                                          where ((string.IsNullOrEmpty(username) ? true : q.Person_ID.ToLower().StartsWith(username.ToLower())) &&
                                                  (string.IsNullOrEmpty(name) ? true : q.Person_Name.ToLower().StartsWith(name.ToLower())) &&
                                                  (string.IsNullOrEmpty(surname) ? true : q.Person_Surname.ToLower().StartsWith(surname.ToLower())) &&
                                                  (string.IsNullOrEmpty(email) ? true : q.Person_Email.ToLower().StartsWith(email.ToLower())))
                                          select q;
            return PartialView("Members", viewModel);
        }

        public PartialViewResult MemberDetails(string id)
        {
            var personDetails = new MemberIndexVM();
            if (id != null)
            {
                var selectPerson = db.Registered_Person.Find(id);
                personDetails.person_id = selectPerson.Person_ID;
                personDetails.person_name = selectPerson.Person_Name;
                personDetails.person_surname = selectPerson.Person_Surname;
                personDetails.person_email = selectPerson.Person_Email;
            }
            return PartialView("MemberDetails", personDetails);
        }

        // GET: Member/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Member/Create
        public ActionResult Create()
        {
            var viewModel = new MemberCreateVM();
            return View(viewModel);
        }
        public JsonResult UserExists(string person_id)
        {
            return Json(!(db.Registered_Person.Any(x => x.Person_ID.ToLower() == person_id.ToLower())), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmailExists(string person_email)
        {
            return Json(!(db.Registered_Person.Any(x => x.Person_Email.ToLower() == person_email.ToLower())), JsonRequestBehavior.AllowGet);
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult Create(MemberCreateVM viewModel)
        {
            //Role id has been hardcoded to represent student (4)
            TempData["Show"] = false;
            if (db.Registered_Person.Any(x => x.Person_ID == viewModel.person_id))
            {
                ModelState.AddModelError("person_id", "Username is already registered");
            }
            if (!db.Registered_Person.Any(x => x.Person_ID.StartsWith("p")))
            {
                ModelState.AddModelError("person_id", "Username must start with a 'p' and follow with 8 digits");
            }
            if (ModelState.IsValid)
            {
                string password = Membership.GeneratePassword(5, 1);
                var hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
                var mem = new Registered_Person();
                mem.Person_ID = viewModel.person_id;
                mem.Person_Name = viewModel.person_name;
                mem.Person_Surname = viewModel.person_surname;
                mem.Person_Type = "Student";
                mem.Person_Password = hashed;
                mem.Person_Registration_DateTime = DateTime.Now;
                mem.Person_Email = viewModel.person_email;
                db.Registered_Person.Add(mem);
                var pRole = new Person_Role();
                pRole.Person_ID = viewModel.person_id;
                pRole.Role_ID = 4;

                db.Person_Role.Add(pRole);

                //Email start
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                message.From = new MailAddress("uplibraryassistant@gmail.com");
                message.To.Add(viewModel.person_email);
                message.Subject = "Member Registerstration";
                message.Body = "Hi, " + viewModel.person_id + " you have been registered to UP Library Assistant by an Admin, use your UP username to login, your password is: " + password;
                message.IsBodyHtml = true;
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
                client.Send(message);
                //Email end

                db.SaveChanges();
                TempData["Msg"] = "New member created successfully.";
                TempData["Show"] = true;
                TempData["color"] = "alert-success";
                return RedirectToAction("Index");
            }
            TempData["Show"] = true;
            TempData["color"] = "alert-warning";
            TempData["Msg"] = "Something went wrong.";
            return View(viewModel);
        }

        // GET: Member/Edit/5
        public ActionResult Edit(string id)
        {
            //employee details
            if (id == null)
            {
                TempData["Msg"] = "Please select an employee before selecting update.";
                TempData["Show"] = true;
                TempData["color"] = "alert-warning";
                ModelState.AddModelError("person_id", "Please select a member before selecting update.");
                return RedirectToAction("Index");
            }
            var viewModel = new MemberEditVM();
            viewModel.person_id = db.Registered_Person.Find(id).Person_ID;
            viewModel.person_name = db.Registered_Person.Find(id).Person_Name;
            viewModel.person_surname = db.Registered_Person.Find(id).Person_Surname;
            viewModel.person_email = db.Registered_Person.Find(id).Person_Email;
            TempData["Show"] = false;
            return View(viewModel);
        }

        public ActionResult ResetPassword(string id, string _email)
        {
            string password = Membership.GeneratePassword(5, 1);
            var hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
            Registered_Person rp = db.Registered_Person.Find(id);
            rp.Person_Password = hashed;

            //Email start
            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            message.From = new MailAddress("uplibraryassistant@gmail.com");
            message.To.Add(_email);
            message.Subject = "UP Library Assistant - Password Reset";
            message.Body = "Hi " + id + ", your password for UP Library Assistant has been reset by an Admin. </br> Your new password is: " + password;
            message.IsBodyHtml = true;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
            client.Send(message);
            //Email end

            return RedirectToAction("Edit", id);
        }

        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult Edit(string id, MemberEditVM viewModel)
        {
            TempData["Show"] = false;
            if (ModelState.IsValid)
            {
                Registered_Person rp = db.Registered_Person.Find(id);
                rp.Person_ID = id;
                rp.Person_Name = viewModel.person_name;
                rp.Person_Surname = viewModel.person_surname;
                rp.Person_Email = viewModel.person_email;
                rp.Person_Type = "Student";
                db.Entry(rp).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Msg"] = "Member updated successfully.";
                TempData["Show"] = true;
                TempData["color"] = "alert-success";
            }
            return RedirectToAction("Index");
        }

        // GET: Member/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                TempData["Msg"] = "Please select a member before selecting delete.";
                TempData["Show"] = true;
                TempData["color"] = "alert-warning";
                return RedirectToAction("Index");
            }

            var viewModel = new MemberDeleteVM();
            viewModel.registered_person = db.Registered_Person.Find(id);
            viewModel.person_role = db.Person_Role.Where(x => x.Person_ID == id);

            if (db.Person_Role.Where(x => x.Person_ID == id).Where(x => x.Role_ID == 5).Any())
            {
                ViewBag.ErrorMsg = "Administrators cannot be deleted.";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }
            if (db.Person_Questionnaire.Any(x => x.Person_ID == id))
            {
                ViewBag.ErrorMsg = "Member cannot be deleted because they have been active on the system (relating to questionnaires).";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }
            if (db.Person_Questionnaire_Result.Any(x => x.Person_ID == id))
            {
                ViewBag.ErrorMsg = "Member cannot be deleted because they have been active on the system (relating to questionnaire results).";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }
            if (db.Person_Session_Log.Any(x => x.Person_ID == id))
            {
                ViewBag.ErrorMsg = "Member cannot be deleted because they have been active on the system (relating to session log).";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }

            ViewBag.ErrorMsg = "Are you sure you want to delete?";
            TempData["Disabled"] = "";
            TempData["Show"] = false;

            return View(viewModel);
        }

        // POST: Member/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var deleteMem = db.Registered_Person.Find(id);
            var deletePersonRoles = db.Person_Role.Where(x => x.Person_ID == id);
            foreach (var item in deletePersonRoles)
            {
                db.Person_Role.Remove(item);
            }
            var deleteTopics = db.Person_Topic.Where(x => x.Person_ID == id);
            foreach (var item in deleteTopics)
            {
                db.Person_Topic.Remove(item);
            }
            db.Registered_Person.Remove(deleteMem);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
