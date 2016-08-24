using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryAssistantApp.Models;


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

        public ActionResult Create()
        {
            var viewModel = new EmployeeAddModel();
            viewModel.current_up_person = db.Current_UP_Person;

            ViewBag.PersonTitle = new SelectList(db.Person_Title, "Person_Title1", "Person_Title1");
            ViewBag.PersonType = new SelectList(db.Person_Type, "Person_Type1", "Person_Type1", "Employee");

            return View(viewModel);
        }

        public PartialViewResult GetRoles()
        {
            var viewModel = new EmployeeAddModel();

            viewModel.role = db.Roles
                .Include(i => i.Role_Action.Select(x => x.Action));

            return PartialView(viewModel);
        }

        public PartialViewResult GetActions(int? id)
        {
            var viewModel = new EmployeeAddModel();
            if (id != null)
            {
                viewModel.role_action = db.Roles.Single(
                    r => r.Role_ID == id.Value).Role_Action;
                return PartialView(viewModel);
            }
            return PartialView();
        }

        [HttpPost]
        public ActionResult Create(EmployeeAddModel viewModel)
        {
            if (ModelState.IsValid)
            {
                TempData["SuccessMsg"] = "New employee created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            return View();
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
