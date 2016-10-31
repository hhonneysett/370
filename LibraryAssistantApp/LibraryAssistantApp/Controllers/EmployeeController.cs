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
    [Authorize(Roles = "Admin")]
    public class EmployeeController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();
        public ActionResult Index(string username, string name, string surname, string email, string roleid)
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
            var viewModel = new EmployeeIndexModel();
            viewModel.registered_person = db.Registered_Person
                    .Where(t => t.Person_Type.ToLower() == "employee");

            List < SelectListItem > selectListItems = new List<SelectListItem>();
            foreach (Role role in db.Roles)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = role.Role_Name,
                    Value = role.Role_ID.ToString()
                };
                selectListItems.Add(selectListItem);
            }
            ViewBag.Roles = selectListItems;

            return View("Index", viewModel);
        }

        public PartialViewResult Employees(string username, string name, string surname, string email, string roleid)
        {
            var viewModel = new EmployeeIndexModel();
            viewModel.registered_person = db.Registered_Person
                    .Where(t => t.Person_Type.ToLower() == "employee");

            var pRoles = (from r in db.Person_Role
                          where (string.IsNullOrEmpty(roleid) ? true : r.Role_ID.ToString() == roleid)
                          select r.Person_ID).Distinct().ToList();
            viewModel.registered_person = from q in viewModel.registered_person
                                          where ((string.IsNullOrEmpty(username) ? true : q.Person_ID.ToLower().StartsWith(username.ToLower())) &&
                                                  (string.IsNullOrEmpty(name) ? true : q.Person_Name.ToLower().StartsWith(name.ToLower())) &&
                                                  (string.IsNullOrEmpty(surname) ? true : q.Person_Surname.ToLower().StartsWith(surname.ToLower())) &&
                                                  (string.IsNullOrEmpty(email) ? true : q.Person_Email.ToLower().StartsWith(email.ToLower())))
                                          select q;
            if (pRoles != null)
            {
                viewModel.registered_person = viewModel.registered_person.Where(p => pRoles.Contains(p.Person_ID));
            }
            return PartialView("Employees", viewModel);
        }

        public PartialViewResult EmployeeDetails(string id)
        {
            var personDetails = new EmployeeIndexModel();
            if (id != null)
            {
                personDetails.person_role = db.Person_Role
                    .Where(x => x.Person_ID == id)
                        .Include(pr => pr.Role);

                personDetails.trainer_topic = db.Trainer_Topic
                    .Where(x => x.Person_ID == id)
                        .Include(pr => pr.Topic).ToList();

                TempData["Check"] = false;
                if (personDetails.trainer_topic.Any())
                {
                    TempData["Check"] = true;
                }

                var selectPerson = db.Registered_Person.Find(id);
                personDetails.person_id = selectPerson.Person_ID;
                personDetails.person_name = selectPerson.Person_Name;
                personDetails.person_surname = selectPerson.Person_Surname;
                personDetails.person_email = selectPerson.Person_Email;
            }
            return PartialView("EmployeeDetails", personDetails);
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

        public ActionResult Create()
        {
            TempData["_Categories"] = from c in db.Categories
                                      select c;
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", 2);
            ViewBag.Check1 = false;

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
        public PartialViewResult Categories(string cat_id, string arr)
        {
            //deserialize myArray
            JavaScriptSerializer js = new JavaScriptSerializer();
            var myojb = (List<TopicCheck>)js.Deserialize(arr, typeof(List<TopicCheck>));
            var topicchecklist = (List<TopicCheck>)Session["Topic_Checked"];
            //var topics = new List<TopicCheck>();

            foreach (var item in myojb)
            {
                if (topicchecklist.Any(tc => item.topic_seq == tc.topic_seq))
                {
                    var remove = topicchecklist.Where(x => x.topic_seq == item.topic_seq).Single();
                    topicchecklist.Remove(remove);
                    var topic = new TopicCheck();
                    topic.topic_seq = item.topic_seq;
                    topic.category_id = db.Topic_Category.Where(x => x.Topic_Seq == item.topic_seq).Select(x => x.Category_ID).FirstOrDefault().ToString();
                    topic.topic_name = db.Topics.Find(item.topic_seq).Topic_Name;
                    topic.topic_ind = item.topic_ind;
                    topicchecklist.Add(topic);
                }
            }

            var viewModel = new EmployeeAddModel();
            viewModel.topic_check = topicchecklist;
            if (cat_id != null)
            {
                var id = (string)js.Deserialize(cat_id, typeof(string));
                viewModel.topic_check = (from tc in viewModel.topic_check
                                            where tc.category_id == id
                                            select tc).ToList();
            }

            return PartialView("Categories", viewModel);
        }
        public PartialViewResult Topics()
        {
            Session["Topic_Checked"] = new List<TopicCheck>();

            var topicchecklist = (List<TopicCheck>)Session["Topic_Checked"];

            var viewModel = new EmployeeAddModel();

            foreach (var item in db.Topic_Category)
            {
                var t = new TopicCheck();
                t.topic_seq = item.Topic_Seq;
                t.topic_name = db.Topics.Find(item.Topic_Seq).Topic_Name;
                t.category_id = item.Category_ID.ToString();
                topicchecklist.Add(t);
            }
            viewModel.topic_check = topicchecklist;

            viewModel.topic_category = db.Topic_Category.Include(x => x.Topic).ToList();

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Category category in db.Categories)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = category.Category_Name,
                    Value = category.Category_ID.ToString()
                };
                selectListItems.Add(selectListItem);
            }
            ViewBag.Categories = selectListItems;

            return PartialView("Topics", viewModel);
        }

        public ActionResult saveEmployee(string id, string name, string surname, string email, string arr, string rolearr)
        {
            var viewModel = new EmployeeAddModel();
            var topicchecklist = (List<TopicCheck>)Session["Topic_Checked"];

            JavaScriptSerializer js = new JavaScriptSerializer();
            var myojb = (List<TopicCheck>)js.Deserialize(arr, typeof(List<TopicCheck>));
            var roleArr = (List<RoleCheck>)js.Deserialize(rolearr, typeof(List<RoleCheck>));

            var _id = (string)js.Deserialize(id, typeof(string));
            var _name = (string)js.Deserialize(name, typeof(string));
            var _surname = (string)js.Deserialize(surname, typeof(string));
            var _email = (string)js.Deserialize(email, typeof(string));

            foreach (var item in myojb)
            {
                if (topicchecklist.Any(tc => item.topic_seq == tc.topic_seq))
                {
                    var remove = topicchecklist.Where(x => x.topic_seq == item.topic_seq).Single();
                    topicchecklist.Remove(remove);
                    var topic = new TopicCheck();
                    topic.topic_seq = item.topic_seq;
                    topic.category_id = db.Topic_Category.Where(x => x.Topic_Seq == item.topic_seq).Select(x => x.Category_ID).FirstOrDefault().ToString();
                    topic.topic_name = db.Topics.Find(item.topic_seq).Topic_Name;
                    topic.topic_ind = item.topic_ind;
                    topicchecklist.Add(topic);
                }
            }
            viewModel.topic_check = topicchecklist;
            viewModel.role_check = roleArr;

            viewModel.person_id = _id;
            viewModel.person_name = _name;
            viewModel.person_surname = _surname;
            viewModel.person_email = _email;

            return Create(viewModel);
        }

        [HttpPost]
        public ActionResult Create(EmployeeAddModel viewModel)
        {
            TempData["Show"] = false;
            var topicchecklist = (List<TopicCheck>)Session["Topic_Checked"];
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
                    foreach (var item in topicchecklist)
                    {
                        var trainertopic = new Trainer_Topic();
                        if (item.topic_ind)
                        {
                            trainertopic.Person_ID = emp.Person_ID;
                            trainertopic.Topic_Seq = item.topic_seq;
                            db.Trainer_Topic.Add(trainertopic);
                        }
                    }
                }
                catch
                {

                }

                //Email start
                MailMessage message = new MailMessage();
                SmtpClient client = new SmtpClient();
                client.Host = "smtp.gmail.com";
                client.Port = 587;

                message.From = new MailAddress("uplibraryassistant@gmail.com");
                message.To.Add(viewModel.person_email);
                message.Subject = "Employee Registration";
                message.Body = "Hi, " + viewModel.person_id + " you have been registered to UP Library Assistant by an Admin, use your UP username to login, your password is: " + password;
                message.IsBodyHtml = true;
                client.EnableSsl = true;
                client.UseDefaultCredentials = true;
                client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
                client.Send(message);
                //Email end

                db.SaveChanges();

                global.addAudit("Employees", "Employees: Create Employee", "Create", User.Identity.Name);

                TempData["Msg"] = "New employee created successfully.";
                TempData["Show"] = true;
                TempData["color"] = "alert-success";
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
            TempData["Msg"] = "Something went wrong.";
            TempData["Show"] = true;
            TempData["color"] = "alert-success";
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

        public ActionResult Edit(string id)
        {
            //employee details
            if (id == null)
            {
                TempData["Msg"] = "Please select an employee before selecting update.";
                TempData["Show"] = true;
                TempData["color"] = "alert-warning";
                ModelState.AddModelError("person_id", "Please select an employee before selecting update.");
                return RedirectToAction("Index");
            }
            var viewModel = new EmployeeEditModel();
            viewModel.emprolecheckeditlist = new List<EmpRoleCheckEdit>();

            viewModel.person_id = db.Registered_Person.Find(id).Person_ID;
            viewModel.person_name = db.Registered_Person.Find(id).Person_Name;
            viewModel.person_surname = db.Registered_Person.Find(id).Person_Surname;
            viewModel.person_email = db.Registered_Person.Find(id).Person_Email;
            
            //roles
            var p_role = db.Person_Role.Where(
                i => i.Person_ID == id).Include(x => x.Role).ToList();

            foreach (var item in db.Roles)
            {
            var rolecheck = new EmpRoleCheckEdit();
            rolecheck.role_id = item.Role_ID;
                rolecheck.role_name = item.Role_Name;
                foreach (var r in p_role)
                {
                    if (item.Role_ID == r.Role_ID)
                    {
                        rolecheck.role_ind = true;
                    }
                }
                viewModel.emprolecheckeditlist.Add(rolecheck);
            }

            //topics

            Session["TopicChecked"] = new List<TopicCheck>();
            var topicchecks = (List<TopicCheck>)Session["TopicChecked"];

            List<SelectListItem> selectListItems = new List<SelectListItem>();
            foreach (Category category in db.Categories)
            {
                SelectListItem selectListItem = new SelectListItem
                {
                    Text = category.Category_Name,
                    Value = category.Category_ID.ToString()
                };
                selectListItems.Add(selectListItem);
            }
            ViewBag.Categories = selectListItems;

            TempData["Check1"] = false;
                var trainer_topic = db.Trainer_Topic
                    .Where(i => i.Person_ID == id).ToList();
                viewModel.topicchecks = new List<TopicCheck>();

                foreach (var item in db.Topics)
                {
                    var topiccheck = new TopicCheck();
                    topiccheck.topic_seq = item.Topic_Seq;
                    topiccheck.topic_name = item.Topic_Name;
                topiccheck.category_id = db.Topic_Category.Where(x => x.Topic_Seq == item.Topic_Seq).Select(x => x.Category_ID).FirstOrDefault().ToString();
                foreach (var tt in trainer_topic)
                    {
                       if (item.Topic_Seq == tt.Topic_Seq)
                        {
                            topiccheck.topic_ind = true;
                        }
                    }
                    topicchecks.Add(topiccheck);
                }

                if (!viewModel.emprolecheckeditlist.Where(x => x.role_id == 7).Where(y => y.role_ind == true).Any())
                {
                    TempData["Check1"] = "hidden";
                }
            viewModel.topicchecks = topicchecks;

            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", 2);
            TempData["Show"] = false;
            return View(viewModel);
        }
        public PartialViewResult Categories_edit(string cat_id, string arr)
        {
            //deserialize myArray
            JavaScriptSerializer js = new JavaScriptSerializer();
            var myojb = (List<TopicCheck>)js.Deserialize(arr, typeof(List<TopicCheck>));
            var topicchecks = (List<TopicCheck>)Session["TopicChecked"];

            foreach (var item in myojb)
            {
                if (topicchecks.Any(tc => item.topic_seq == tc.topic_seq))
                {
                    var remove = topicchecks.Where(x => x.topic_seq == item.topic_seq).Single();
                    topicchecks.Remove(remove);
                    var topic = new TopicCheck();
                    topic.topic_seq = item.topic_seq;
                    topic.category_id = db.Topic_Category.Where(x => x.Topic_Seq == item.topic_seq).Select(x => x.Category_ID).FirstOrDefault().ToString();
                    topic.topic_name = db.Topics.Find(item.topic_seq).Topic_Name;
                    topic.topic_ind = item.topic_ind;
                    topicchecks.Add(topic);
                }
            }

            var viewModel = new EmployeeEditModel();
            viewModel.topicchecks = topicchecks;
            if (cat_id != null)
            {
                var id = (string)js.Deserialize(cat_id, typeof(string));
                viewModel.topicchecks = (from tc in viewModel.topicchecks
                                         where tc.category_id == id
                                         select tc).ToList();
            }

            return PartialView("Categories_edit", viewModel);
        }

        public ActionResult saveTopics(string id, string name, string surname, string email, string arr, string rolearr)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            var myojb = (List<TopicCheck>)js.Deserialize(arr, typeof(List<TopicCheck>));
            var roleArr = (List<EmpRoleCheckEdit>)js.Deserialize(rolearr, typeof(List<EmpRoleCheckEdit>));

            var _id = (string)js.Deserialize(id, typeof(string));
            var _name = (string)js.Deserialize(name, typeof(string));
            var _surname = (string)js.Deserialize(surname, typeof(string));
            var _email = (string)js.Deserialize(email, typeof(string));
            var topicchecks = (List<TopicCheck>)Session["TopicChecked"];

            foreach (var item in myojb)
            {
                if (topicchecks.Any(tc => item.topic_seq == tc.topic_seq))
                {
                    var remove = topicchecks.Where(x => x.topic_seq == item.topic_seq).Single();
                    topicchecks.Remove(remove);
                    var topic = new TopicCheck();
                    topic.topic_seq = item.topic_seq;
                    topic.category_id = db.Topic_Category.Where(x => x.Topic_Seq == item.topic_seq).Select(x => x.Category_ID).FirstOrDefault().ToString();
                    topic.topic_name = db.Topics.Find(item.topic_seq).Topic_Name;
                    topic.topic_ind = item.topic_ind;
                    topicchecks.Add(topic);
                }
            }
            var viewModel = new EmployeeEditModel();
            viewModel.topicchecks = topicchecks;
            viewModel.emprolecheckeditlist = roleArr;

            viewModel.person_id = _id;
            viewModel.person_name = _name;
            viewModel.person_surname = _surname;
            viewModel.person_email = _email;

            return Edit(_id, viewModel);
        }

        public ActionResult ResetPassword(string id, string _email)
        {
            string password = Membership.GeneratePassword(5, 1);
            var hashed = FormsAuthentication.HashPasswordForStoringInConfigFile(password, "MD5");
            Registered_Person rp = db.Registered_Person.Find(id);
            rp.Person_Password = hashed;
            db.Entry(rp).State = EntityState.Modified;
            db.SaveChanges();
            global.addAudit("Employees", "Employees: Reset Password", "Update", User.Identity.Name);
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

        [HttpPost]
        public ActionResult Edit(string id, EmployeeEditModel viewModel)
        {
            var topicchecks = (List<TopicCheck>)Session["TopicChecked"];
            //TODO: validate to make sure at least one role is selected
            if (ModelState.IsValid)
            {
                Registered_Person rp = db.Registered_Person.Find(id);
                rp.Person_ID = id;

                rp.Person_Name = viewModel.person_name;
                rp.Person_Surname = viewModel.person_surname;
                rp.Person_Email = viewModel.person_email;

                rp.Person_Type = "Employee";
                db.Entry(rp).State = EntityState.Modified;
                var roleRemove = db.Person_Role.Where(x => x.Person_ID == id);
                foreach (var item in roleRemove)
                {
                        db.Person_Role.Remove(item);
                }
                foreach (var item in viewModel.emprolecheckeditlist)
                {
                    if (item.role_ind == true)
                    {
                        var pRole = new Person_Role();
                        pRole.Person_ID = id;
                        pRole.Role_ID = item.role_id;
                        db.Person_Role.Add(pRole);
                    }
                }
                var trainerRemove = db.Trainer_Topic.Where(x => x.Person_ID == id);
                foreach (var item in trainerRemove)
                {
                    db.Trainer_Topic.Remove(item);
                }

                if (viewModel.emprolecheckeditlist.Where(x => x.role_id == 7).Where(y => y.role_ind == true ).Any())
                {
                    foreach (var item in topicchecks)
                    {
                        if (item.topic_ind == true)
                        {
                            var tTopic = new Trainer_Topic();
                            tTopic.Person_ID = id;
                            tTopic.Topic_Seq = item.topic_seq;
                            db.Trainer_Topic.Add(tTopic);
                        }
                    }
                }
                db.SaveChanges();
                global.addAudit("Employees", "Employees: Update Employee", "Update", User.Identity.Name);
            }
            TempData["Check2"] = false;
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", 2);
            return RedirectToAction("Index");
        }

        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                TempData["Msg"] = "Please select an employee before selecting delete.";
                TempData["Show"] = true;
                TempData["color"] = "alert-warning";
                return RedirectToAction("Index");
            }

            var viewModel = new EmployeeDeleteModel();
            viewModel.registered_person = db.Registered_Person.Find(id);
            viewModel.person_role = db.Person_Role.Where(x => x.Person_ID == id);
            viewModel.trainer_topic = db.Trainer_Topic
                .Where(x => x.Person_ID == id)
                    .Include(pr => pr.Topic).ToList();

            TempData["Check"] = false;
            if (viewModel.trainer_topic.Any())
            {

                TempData["Check"] = true;
            }

            viewModel.trainer_topic = db.Trainer_Topic
                .Where(x => x.Person_ID == id)
                    .Include(pr => pr.Topic).ToList();

            if (db.Person_Role.Where(x => x.Person_ID == id).Where(x => x.Role_ID == 5).Any())
            {
                ViewBag.ErrorMsg = "Administrators cannot be deleted.";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }
            if (db.Person_Questionnaire.Any(x => x.Person_ID == id))
            {
                ViewBag.ErrorMsg = "Employee cannot be deleted because they have been active on the system (relating to questionnaires).";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }
            if (db.Person_Questionnaire_Result.Any(x => x.Person_ID == id))
            {
                ViewBag.ErrorMsg = "Employee cannot be deleted because they have been active on the system (relating to questionnaire results).";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }
            if (db.Person_Session_Log.Any(x => x.Person_ID == id))
            {
                ViewBag.ErrorMsg = "Employee cannot be deleted because they have been active on the system (relating to session log).";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }
            /*Check venue booking person table*/
            if (db.Venue_Booking_Person.Any(x => x.Person_ID == id))
            {
                ViewBag.ErrorMsg = "Employee cannot be deleted because they have been active on the system (relating to venue bookings).";
                TempData["Disabled"] = "Disabled";
                return View(viewModel);
            }

            ViewBag.ErrorMsg = "Are you sure you want to delete?";
            TempData["Show"] = false;
            TempData["Disabled"] = "";

            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirm(string id)
        {
            if (id == null)
            {
                return HttpNotFound();
            }
            var deleteEmp = db.Registered_Person.Find(id);
            var deletePersonRoles = db.Person_Role.Where(x => x.Person_ID == id);
            foreach (var item in deletePersonRoles)
            {
                db.Person_Role.Remove(item);
            }
            db.Registered_Person.Remove(deleteEmp);
            db.SaveChanges();

            global.addAudit("Employees", "Employees: Delete Employee", "Delete", User.Identity.Name);

            return RedirectToAction ("Index");
        }
    }
}