using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryAssistantApp.Models;
using System.Web.Security;

namespace LibraryAssistantApp.Controllers
{
    public class EmployeeController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();
        public ActionResult Index(string username, string name, string surname, string email, string title)
        {
            var viewModel = new EmployeeIndexModel();
            viewModel.registered_person = db.Registered_Person
                .Include(i => i.Person_Title).Include(t => t.Person_Type)
                    .Where(t => t.Person_Type.Person_Type1.ToLower() == "employee");
            ViewBag.personTitle = db.Person_Title;
            viewModel.registered_person = from q in viewModel.registered_person
                            where ((string.IsNullOrEmpty(username) ? true : q.Person_ID.ToLower().StartsWith(username.ToLower())) &&
                                    (string.IsNullOrEmpty(name) ? true : q.Person_Name.ToLower().StartsWith(name.ToLower())) &&
                                    (string.IsNullOrEmpty(surname) ? true : q.Person_Surname.ToLower().StartsWith(surname.ToLower())) &&
                                    (string.IsNullOrEmpty(email) ? true : q.Person_Email.ToLower().StartsWith(email.ToLower())) &&
                                    (string.IsNullOrEmpty(title) ? true : q.Person_Title.Person_Title1.ToLower().StartsWith(title.ToLower())))
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
            ViewBag.Person_Title = new SelectList(db.Person_Title, "Title_ID", "Person_Title1");
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type_ID", "Person_Type1", 2);
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
            return Json(db.Current_UP_Person.Any(x => x.Person_ID.ToLower() == person_id.ToLower()), JsonRequestBehavior.AllowGet);
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
                    emp.Title_ID = viewModel.Person_Title;
                    emp.Person_Type_ID = viewModel.Person_Type;
                    emp.Person_Password = password;
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
            ViewBag.Person_Title = new SelectList(db.Person_Title, "Title_ID", "Person_Title1", viewModel.Person_Title);
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type_ID", "Person_Type1", 2);
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

            foreach (var item in viewModel.person_role)
            {
                foreach (var a in db.Roles)
                {
                    if (a.Role_ID )
                    {

                    }
                }
                var rolecheck = new RoleCheck();
                var test = db.Roles.Selec                
            }

            viewModel.person_role = db.Person_Role.Where(
                    i => i.Person_ID == id).ToList();
            

            viewModel.role = (db.Roles
                .Include(i => i.Role_Action.Select(x => x.Action))).ToList();

            var trainercheck = db.Trainer_Topic.Any(x => x.Person_ID == id);
            if (trainercheck)
            {
                viewModel.trainer_topic = db.Trainer_Topic.Where(t => t.Person_ID == id);
            }
            ViewBag.Person_Title = new SelectList(db.Person_Title, "Title_ID", "Person_Title1");
            ViewBag.Person_Type = new SelectList(db.Person_Type, "Person_Type_ID", "Person_Type1", 2);
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
