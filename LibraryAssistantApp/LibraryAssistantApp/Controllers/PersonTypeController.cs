using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LibraryAssistantApp.Models;
using System.Data.Entity;

namespace LibraryAssistantApp.Controllers
{
    public class PersonTypeController : Controller
    {
        LibraryAssistantEntities db = new LibraryAssistantEntities();
        public ActionResult Index(string search)
        {
            TempData["ErrorMsg"] = "";
            var model = new PersonTypeModel();
            model.person_types = (from t in db.Person_Type
                     select t);

            if (search != null)
            {
                var personType = (from pt in db.Person_Type
                                   where pt.Person_Type1.ToLower().StartsWith(search.ToLower())
                                   select pt);
                model.person_types = personType;
                if (model.person_types.Count() == 0)
                {
                    TempData["ErrorMsg"] = "No person types match search criteria";
                }
            }           
            return View(model);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PersonTypeAddModel model)
        {
            ViewBag.ErrorMsg = "";
            TempData["SuccessMsg"] = "";
            if (ModelState.IsValid)
            {
                var query = (from p in db.Person_Type
                             where (p.Person_Type1.ToLower() == model.person_type.ToLower())
                             select p);
                if (query.Count() != 0)
                {
                    ViewBag.ErrorMsg = "Person Type exists, please provide a different Person Type";
                    return View(model);
                }
                Person_Type persontype = new Person_Type();
                persontype.Person_Type1 = model.person_type;
                db.Person_Type.Add(persontype);
                db.SaveChanges();
                TempData["SuccessMsg"] = "Person Type was added successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMsg = "";
                return View();
            }
        }

        public ActionResult Edit(int? id)
        {
            TempData["ErrorMsg"] = "";
            if (id == null)
            {
                TempData["ErrorMsg"] = "Please select a person type";
                return RedirectToAction("Index");
            }
                PersonTypeEditModel model = new PersonTypeEditModel();
                model.person_type = db.Person_Type.Find(id);
                if (model.person_type == null)
                {
                    return HttpNotFound();
                }
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(int id, PersonTypeEditModel model)
        {
            TempData["ErrorMsg"] = "";
            TempData["SuccessMsg"] = "";
            if (ModelState.IsValid)
            {
                var query = (from pt in db.Person_Type
                             where pt.Person_Type1.ToLower() == model.person_type.Person_Type1.ToLower()
                             select pt);
                if (query.Count() == 1)
                {
                    ViewBag.ErrorMsg = "The Person Type exists, please provide a differernt Person Type";
                    return View(model);
                }
                Person_Type person_type = db.Person_Type.Find(id);
                person_type.Person_Type1 = model.person_type.Person_Type1;
                db.Entry(person_type).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMsg"] = "The Person Type was updated successfully";
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ErrorMsg = "Person Type invalid";
                return View(model);
            }
        }

        public ActionResult Delete(int? id)
        {
            TempData["ErrorMsg"] = "";
            TempData["Disabled"] = false;
            ViewBag.ErrorMsg = "Are you sure you want to delete?";
            if (id == null)
            {
                TempData["ErrorMsg"] = "Please select a person type";
                return RedirectToAction("Index");
            } 

            PersonTypeEditModel deletemodel = new PersonTypeEditModel();
            deletemodel.person_type = db.Person_Type.Find(id);
            if (deletemodel.person_type == null)
            {
                return HttpNotFound();
            }
            var query = from q in db.Registered_Person
                        where q.Person_Type_ID == id
                        select q;
            if (query.Count() != 0)
            {
                ViewBag.ErrorMsg = "Person Type cannot be deleted because it relates to existing registered persons";
                TempData["Disabled"] = true;
                return View(deletemodel);
            }

            return View(deletemodel);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, PersonTypeEditModel model)
        {
            TempData["SuccessMsg"] = "";
            TempData["Disabled"] = false;
            ViewBag.ErrorMsg = "Are you sure you want to delete?";
            PersonTypeEditModel pt = new PersonTypeEditModel();
            pt.person_type = db.Person_Type.Find(id);
            if (pt.person_type == null)
            {
                return HttpNotFound();
            }
            var query = from q in db.Registered_Person
                        where q.Person_Type_ID == id
                        select q;
            if (query.Count() != 0)
            {
                ViewBag.ErrorMsg = "Person Type cannot be deleted because it relates to existing registered persons";
                TempData["Disabled"] = true;
                return View(model);
            }
            db.Person_Type.Remove(pt.person_type);
            db.SaveChanges();
            TempData["SuccessMsg"] = "Person Type was successfully deleted";
            return RedirectToAction("Index");
        }
    }
}
