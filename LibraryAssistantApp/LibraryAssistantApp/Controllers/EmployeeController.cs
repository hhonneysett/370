using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryAssistantApp.Models;
using System.Web.Security;
using System.Net.Mail;

namespace LibraryAssistantApp.Controllers
{
    [Authorize(Roles="Admin")]
    public class EmployeeController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();
        public ActionResult Index(string username, string name, string surname, string email, string title)
        {
            var viewModel = new EmployeeIndexModel();
            viewModel.registered_person = db.Registered_Person
                    .Where(t => t.Person_Type.ToLower() == "employee");
            viewModel.registered_person = from q in viewModel.registered_person
                            where ((string.IsNullOrEmpty(username) ? true : q.Person_ID.ToLower().StartsWith(username.ToLower())) &&
                                    (string.IsNullOrEmpty(name) ? true : q.Person_Name.ToLower().StartsWith(name.ToLower())) &&
                                    (string.IsNullOrEmpty(surname) ? true : q.Person_Surname.ToLower().StartsWith(surname.ToLower())) &&
                                    (string.IsNullOrEmpty(email) ? true : q.Person_Email.ToLower().StartsWith(email.ToLower())))
                            select q;
            return View(viewModel);
        }

        public PartialViewResult EmployeeDetails(string id)
        {
            var personRoles = new EmployeeIndexModel();
            if (id != null)
            {
                personRoles.person_role = db.Person_Role
                    .Where(x => x.Person_ID == id)
                        .Include(pr => pr.Role);
            }
            return PartialView("EmployeeDetails", personRoles);
        }
        public PartialViewResult RoleDetails(int? id)
        {
            var roleAction = new EmployeeIndexModel();
            if (id != null)
            {
                roleAction.role_action = db.Role_Action
                    .Where(ra => ra.Role_ID == id).Include(a => a.Action);
            }
            return PartialView("RoleDetails", roleAction);
        }

        public ActionResult Create(string person_id, string person_email, string person_name, string person_surname)
        {
            TempData["_Categories"] = from c in db.Categories
                                      select c;
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", 2);
            ViewBag.Check1 = false;
            if (!db.Registered_Person.Any(x => x.Person_ID.StartsWith("p")))
            {
                ModelState.AddModelError("person_id", "Username must start with a 'p' and follow with 8 digits");
                return View();
            }
            if (db.Registered_Person.Any(x => x.Person_ID == person_id))
            {
                ModelState.AddModelError("person_id", "Username is already registered");
                return View();
            }
            if (person_email != null)
            {
                var person_address = db.Registered_Person.Any(e => e.Person_Email == person_email);
                if (!person_address)
                {
                    ViewBag.Check1 = true;
                    var viewModel = new EmployeeAddModel();
                    viewModel.role = (db.Roles
                        .Include(i => i.Role_Action.Select(x => x.Action))).ToList();
                    var rolechecklist = new List<RoleCheck>();
                    for (int i = 0; i < viewModel.role.Count(); i++)
                    {
                        var roleCheck = new RoleCheck();
                        roleCheck.role_id = viewModel.role[i].Role_ID;
                        rolechecklist.Add(roleCheck);
                    }
                    viewModel.role_check = rolechecklist;
                    return View(viewModel);
                }
            }
            return View();
        }

        public JsonResult UserExists(string person_id)
        {
            return Json(db.Registered_Person.Any(x => x.Person_ID.ToLower() == person_id.ToLower()), JsonRequestBehavior.AllowGet);
        }

        public JsonResult EmailExists(string person_email)
        {
            return Json(!(db.Registered_Person.Any(x => x.Person_Email.ToLower() == person_email.ToLower())), JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult GetActions(int? id)
        {
            if (id != null)
            {
                var viewModel = new EmployeeAddModel();
                viewModel.role_action = db.Roles.Single(
                    r => r.Role_ID == id.Value).Role_Action;

                return PartialView(viewModel);
            }
            return PartialView();
        }
        public PartialViewResult Topics()
        {
            var viewModel = new EmployeeAddModel();
                viewModel.topic_category = db.Topic_Category.Include(x => x.Topic).ToList();
            var topicchecklist = new List<TopicCheck>();
            foreach (var item in viewModel.topic_category)
            {
                var t = new TopicCheck();
                t.topic_sec = item.Topic_Seq;
                topicchecklist.Add(t);
            }
            viewModel.topic_check = topicchecklist;
            return PartialView("Topics", viewModel);
            //if (id == null)
            //{
            //    viewModel.topic_category = db.Topic_Category.Include(x => x.Topic).ToList();
            //}
            //if (id != null)
            //{
            //    viewModel.topic_category = (db.Topic_Category
            //        .Include(x => x.Topic).Where(i => i.Category_ID == id)).ToList();
            //        var tcList = new List<TopicCheck>();
            //        foreach (var item in viewModel.topic_category)
            //        {
            //            var tc = new TopicCheck();
            //        if (viewModel.topic_check.Any())
            //        {
            //            foreach (var tc1 in viewModel.topic_check)
            //            {
            //                bool myBool = false;
            //                int count = viewModel.topic_check.Count();
            //                while (myBool != true)
            //                {
            //                    for (int i = 0; i < count; i++)
            //                    {
            //                        if (tc1.topic_sec == item.Topic_Seq)
            //                        {
            //                            tc.topic_sec = item.Topic_Seq;
            //                            tc.topic_ind = tc1.topic_ind;
            //                            tcList.Add(tc);
            //                            myBool = true;
            //                        }
            //                        if (i == count)
            //                        {
            //                            tc.topic_sec = item.Topic_Seq;
            //                            tcList.Add(tc);
            //                            myBool = true;
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //        else 
            //        {
            //            tc.topic_sec = item.Topic_Seq;
            //            tcList.Add(tc);
            //        }

            //        }
            //        viewModel.topic_check = tcList;
            //}
            //return PartialView("Topics", viewModel);
        }

        [HttpPost]
        public ActionResult Create(EmployeeAddModel viewModel)
        {
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
                string password = Membership.GeneratePassword(8, 1);
                var hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");                
                var emp = new Registered_Person();
                    emp.Person_ID = viewModel.person_id;
                    emp.Person_Name = viewModel.person_name;
                    emp.Person_Surname = viewModel.person_surname;
                    emp.Person_Type = "Employee";
                    emp.Person_Password = hashed;
                    emp.Person_Registration_DateTime = DateTime.Now;
                    emp.Person_Email = viewModel.person_email;
                    db.Registered_Person.Add(emp);
                    foreach (var item in viewModel.role_check)
                    {
                        var prole = new Person_Role();
                        if (item.role_ind)
                        {
                            prole.Role_ID = item.role_id;
                            prole.Person_ID = emp.Person_ID;
                            db.Person_Role.Add(prole);
                        }
                    }
                try
                {
                    foreach (var item in viewModel.topic_check)
                    {
                        var trainertopic = new Trainer_Topic();
                        if (item.topic_ind)
                        {
                            trainertopic.Person_ID = emp.Person_ID;
                            trainertopic.Topic_Seq = item.topic_sec;
                            db.Trainer_Topic.Add(trainertopic);
                        }
                    }
                }
                catch
                {

                }

                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                message.From = new MailAddress("uplibraryassistant@gmail.com");
                message.To.Add(viewModel.person_email);
                message.Subject = "Employee Register";
                message.Body = "Hi, you have been registered to UP Library Assistant by an Admin, use your UP username to login, your password is: " + password;
                message.IsBodyHtml = true;
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
                client.Send(message);

                db.SaveChanges();
                    TempData["SuccessMsg"] = "New employee created successfully";
                    return RedirectToAction("Index");
                }
            ViewBag.Check1 = true;
            ViewBag.Check2 = true;
            viewModel.role = (db.Roles
                .Include(i => i.Role_Action.Select(x => x.Action))).ToList();
            var rolechecklist = new List<RoleCheck>();
            for (int i = 0; i < viewModel.role.Count(); i++)
            {
                var roleCheck = new RoleCheck();
                roleCheck.role_id = viewModel.role[i].Role_ID;
                rolechecklist.Add(roleCheck);
            }
            viewModel.role_check = rolechecklist;
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", 2);
            return View(viewModel);
        }

        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                TempData["ErrorMsg"] = "Please seect an employee before selecting update";
                ModelState.AddModelError("person_id", "Please select an employee before selecting update");
                return RedirectToAction("Index");
            }
            var viewModel = new EmployeeEditModel();

            viewModel.registered_person = db.Registered_Person.Find(id);

            viewModel.person_role = db.Person_Role.Where(
                i => i.Person_ID == id).Include(x => x.Role).ToList();

            var rolechecklist = new List<EmpRoleCheckEdit>();
            foreach (var item in db.Roles)
            {
            var rolecheck = new EmpRoleCheckEdit();
            rolecheck.role_id = item.Role_ID;
            rolecheck.role_name = item.Role_Name;
                rolecheck.person_ID = id;
                foreach (var r in viewModel.person_role)
                {
                    if (item.Role_ID == r.Role_ID)
                    {
                        rolecheck.role_ind = true;
                    }
                }
            rolechecklist.Add(rolecheck);
            }
            viewModel.emprolecheckeditlist = (from r in rolechecklist
                                      select r).ToList();

            TempData["Check1"] = false;
            var trainercheck = db.Trainer_Topic.Any(x => x.Person_ID == id);
            if (trainercheck)
            {
                viewModel.trainer_topic = db.Trainer_Topic
                    .Where(i => i.Person_ID == id).ToList();
                var trainertopicchecklist = new List<TrainerTopicCheck>();
                foreach (var item in db.Topics)
                {
                    var topiccheck = new TrainerTopicCheck();
                    topiccheck.topic_seq = item.Topic_Seq;
                    topiccheck.topic_name = item.Topic_Name;
                    topiccheck.topic_description = item.Description;
                    foreach (var tt in viewModel.trainer_topic)
                    {
                       topiccheck.personid = id;
                       if (item.Topic_Seq == tt.Topic_Seq)
                        {
                            topiccheck.topic_ind = true;
                        }
                    }
                    trainertopicchecklist.Add(topiccheck);
                }
                viewModel.trainertopiccheck = (from t in trainertopicchecklist
                                              select t).ToList();
                TempData["Check1"] = true;
            }
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", 2);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(string id, EmployeeEditModel viewModel, string Password_, string Person_Title)
        {

            TempData["ErrorMsg"] = "Not enough permissionb to update employee roles";

            if (ModelState.IsValid)
            {
                Registered_Person rp = db.Registered_Person.Find(id);
                rp.Person_ID = id;
                rp.Person_Name = viewModel.registered_person.Person_Name;
                rp.Person_Surname = viewModel.registered_person.Person_Surname;
                rp.Person_Email = viewModel.registered_person.Person_Email;
                rp.Person_Type = "Employee";
                if (Password_.Equals("true"))
                {
                    string password = Membership.GeneratePassword(8, 1);
                    var hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
                    rp.Person_Password = hashed;
                }
                db.Entry(rp).State = EntityState.Modified;
                //foreach (var item in viewModel.emprolecheckeditlist)
                //{ 
                //    if (item.role_ind == true)
                //    {
                //        if (!db.Person_Role.Any(x => x.Person_ID == id))
                //        {
                //            var pRole = new Person_Role();
                //            pRole.Person_ID = item.person_ID;
                //            pRole.Role_ID = item.role_id;                            
                //            db.Person_Role.Add(pRole);                             
                //        }  
                //    }
                //    if (item.role_ind == false)
                //    {
                //        if (db.Person_Role.Any(x => x.Role_ID.Equals(item.role_id)))
                //        {
                //            Person_Role remove = db.Person_Role.Find(item.role_id);
                //            db.Person_Role.Remove(remove);
                //        }
                //    }
                //}
                db.SaveChanges();
            }
            TempData["Check2"] = false;
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", 2);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                TempData["Error"] = "Please select an employee before selecting delete";
                return RedirectToAction("Index");
            }
            ViewBag.ErrorMsg = "Are you sure you want to delete?";
            TempData["Disabled"] = false;

            var viewModel = new EmployeeDeleteModel();
            viewModel.registered_person = db.Registered_Person.Find(id);

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(string id, EmployeeDeleteModel viewModel)
        {
            ViewBag.ErrorMsg = "Are you sure you want to delete?";
            TempData["Disabled"] = false;
            try
            {
                if (db.Venue_Booking_Person.Any(x => x.Person_ID.ToLower() == id.ToLower()))
                {
                    ModelState.AddModelError("person_id", "Employee cannot be deleted because they have been active on the system (relating to venue bookings)");
                    TempData["Disabled"] = true;
                }
                if (db.Person_Questionnaire.Any(x => x.Person_ID.ToLower() == id.ToLower()))
                {
                    ModelState.AddModelError("person_id", "Employee cannot be deleted because they have been active on the system (relating to person questionnaire)");
                    TempData["Disabled"] = true;
                }
                if (db.Person_Questionnaire_Result.Any(x => x.Person_ID.ToLower() == id.ToLower()))
                {
                    ModelState.AddModelError("person_id", "Employee cannot be deleted because they have been active on the system (person questionnaire results)");
                    TempData["Disabled"] = true;
                }
                if (db.Person_Session_Log.Any(x => x.Person_ID.ToLower() == id.ToLower()))
                {
                    ModelState.AddModelError("person_id", "The employee cannot be deleted because they have been active on the system (relating to person session log)");
                    TempData["Disabled"] = true;
                }
                var admin = from p in db.Person_Role
                                  where p.Person_ID == id && p.Role_ID == 5
                                  select p;
                if (admin != null)
                {
                    ModelState.AddModelError("person_id", "Admin cannot be deleted");
                    TempData["Disabled"] = true;
                }

                var superadmin = from s in db.Person_Role
                                  where s.Person_ID == id && s.Role_ID == 5
                                  select s;

                if (superadmin != null)
                {
                    ModelState.AddModelError("person_id", "Super admin cannot be deleted");
                    TempData["Disabled"] = true;
                }

                Registered_Person rp = db.Registered_Person.Find(id);
                if (rp == null)
                {
                    return HttpNotFound();
                }

                foreach (var o in db.Person_Role)
                {
                    Person_Role person = db.Person_Role.Find(o.Person_Role_ID);
                    db.Person_Role.Remove(person);
                }
                foreach (var p in db.Person_Topic)
                {
                    Person_Topic topic = db.Person_Topic.Find(p.Person_ID);
                    db.Person_Topic.Remove(topic);
                }
                foreach (var a in db.Trainer_Topic)
                {
                    Trainer_Topic trainer = db.Trainer_Topic.Find(a.Trainer_Topic_ID);
                    db.Trainer_Topic.Remove(trainer);
                }
                    Registered_Person regperson = db.Registered_Person.Find(id);
                    db.Registered_Person.Remove(regperson);

                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(viewModel);
            }
        }
    }
}
