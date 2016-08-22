using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class TrainerController : Controller
    {
        //initiate instance of database
        LibraryAssistantEntities db = new LibraryAssistantEntities();
        
        //display all existing categories
        [HttpGet]
        public ActionResult viewCategory ()
        {

            //get list of existing categories
            var existingCategories = from c in db.Categories
                                     select c;

            //populate a select list from list of categories
            ViewBag.Category_ID = new SelectList(existingCategories, "Category_ID", "Category_Name");
            return View();
        }

        [HttpGet]
        public ActionResult addCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult addCategory([Bind(Exclude ="categoryId")]CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                //check if a category with the same name already exists
                var categoryCheck = from a in db.Categories
                                    where a.Category_Name.ToLower().Equals(model.categoryName)
                                    select a;

                if (categoryCheck.Any())
                {
                    TempData["Message"] = "Category already exists with the same name";
                    TempData["classStyle"] = "warning";
                    return View(model);
                }
                else
                {
                    //create new instanc of a category object and assing model values to object
                    Category a = new Category
                    {
                        Category_Name = model.categoryName,
                        Description = model.description,
                    };

                    //add the new category to the database and save
                    db.Categories.Add(a);
                    db.SaveChanges();


                    TempData["Message"] = "Category successfully added";
                    TempData["classStyle"] = "success";
                    return RedirectToAction("viewCategory");
                }                               
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult updateCategory(int id)
        {
            //get selected category
            var model = (from c in db.Categories
                         where c.Category_ID.Equals(id)
                         select c).FirstOrDefault();

            //assign selected category to tempdata
            TempData["selectedCat"] = model;

            return View(model);
        }

        [HttpPost]
        public ActionResult updateCategory(Category model, string submitButton)
        {           
            if(ModelState.IsValid)
            {
                switch(submitButton)
                {
                    //check if update button was pressed
                    case "Update Category":
                        //check if changed category name already exists
                        var check = from c in db.Categories
                                    where c.Category_Name.Equals(model.Category_Name) && c.Category_ID != model.Category_ID
                                    select c;

                        if (check.Any())
                        {
                            //name already exists, dislpay error message and go back
                            TempData["Message"] = "Category with the provided name already exists";
                            TempData["classStyle"] = "warning";
                            return View(model);
                        }
                        else
                        {
                            //capture the updated category
                            db.Entry(model).State = EntityState.Modified;
                            db.SaveChanges();

                            //display success message
                            TempData["Message"] = "Category successfully updated";
                            TempData["classStyle"] = "success";
                            return RedirectToAction("viewCategory");
                        };

                    //check if delete button was pressed
                    case "Delete Category":
                        return RedirectToAction("deleteCategory", "Trainer", new { id = model.Category_ID });

                    //if no button was pressed return form
                    default:
                        return View(model);
                }               
            }
            else
            {
                return View(model);
            }                   
        }

        [HttpGet]
        public ActionResult deleteCategory(int id)
        {
            //get selected category
            var model = (from c in db.Categories
                         where c.Category_ID.Equals(id)
                         select c).FirstOrDefault();

            //assign selected category to tempdata
            TempData["selectedCat"] = model;

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult deleteCategory()
        {
            var model = (Category)TempData["selectedCat"];

            //check if category is in use in category topic table
            var check = (from ct in db.Topic_Category
                         where ct.Category_ID.Equals(model.Category_ID)
                         select ct).ToList() ;

            if (check.Any())
            {
                TempData["Message"] = "Unable to delete category. Category is in use";
                TempData["classStyle"] = "warning";
                return View(model);
            }
            else
            {
                //get the seleceted category
                Category selectedCat = db.Categories.Find(model.Category_ID);

                //delete the selected category from the database
                db.Categories.Remove(selectedCat);
                db.SaveChanges();

                //alert successful deletion
                TempData["Message"] = "Category successfully deleted";
                TempData["classStyle"] = "success";

                //return to the view categories page
                return RedirectToAction("ViewCategory");
            }
        }

        [HttpGet]
        public ActionResult viewTopic()
        {
            //get list of existing categories
            var existingCategories = from c in db.Categories
                                     select c;

            ViewBag.Category = existingCategories;
            return View();
        }

        [HttpGet]
        public JsonResult getCatTopic(int id)
        {
            var Topics = (from t in db.Topics
                          join c in db.Topic_Category on t.Topic_Seq equals c.Topic_Seq
                          where c.Category_ID.Equals(id)
                          select t).ToList();

            var jsonList = from b in Topics
                           select new
                           {
                               id = b.Topic_Seq,
                               text = b.Topic_Name,
                           };

            var rows = jsonList.ToArray();

            return Json(rows, JsonRequestBehavior.AllowGet);
        }
            
    }
}