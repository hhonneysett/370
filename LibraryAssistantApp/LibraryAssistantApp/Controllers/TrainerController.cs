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

        [HttpGet]
        public ActionResult updateTopic(int id)
        {
            //get list of existing categories
            var existingCategories = from c in db.Categories
                                     select c;

            ViewBag.Category = existingCategories;

            var currentTopCat = (from t in db.Topics
                                   join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                   where tc.Topic_Seq.Equals(id)
                                   select tc).FirstOrDefault();

            var currentCat = (from c in db.Categories
                              where c.Category_ID.Equals(currentTopCat.Category_ID)
                              select c.Category_ID).FirstOrDefault().ToString();

            ViewBag.currentCat = currentCat;

            var model = (from t in db.Topics
                         where t.Topic_Seq.Equals(id)
                         select t).FirstOrDefault();

            return View(model);
        }

        [HttpPost]
        public ActionResult updateTopic(Topic model, string submitButton, int category)
        {
            if (ModelState.IsValid)
            {
                switch (submitButton)
                {

                    //check if save button was clicked
                    case "Save":

                        //check if any topics with the same name already exist
                        var check = (from t in db.Topics
                                     where t.Topic_Name.Equals(model.Topic_Name) && t.Topic_Seq != model.Topic_Seq
                                     select t);
                        
                        //if topic with the same name already exists return the form and display an error message
                        if (check.Any())
                        {
                            //get list of existing categories
                            var existCat = from c in db.Categories
                                                     select c;

                            //get current topic category
                            ViewBag.Category = existCat;

                            //get the current topic category object for the selected category
                            var currentTC = (from t in db.Topics
                                                 join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                                 where tc.Topic_Seq.Equals(model.Topic_Seq)
                                                 select tc).FirstOrDefault();

                            //get the current category for the selected topic
                            var currentC = (from c in db.Categories
                                              where c.Category_ID.Equals(currentTC.Category_ID)
                                              select c.Category_ID).FirstOrDefault().ToString();

                            ViewBag.currentCat = currentC;

                            //display error message
                            TempData["Message"] = "Topic already exists with the same name";
                            TempData["classStyle"] = "warning";

                            //return form
                            return View(model);
                        }
                        else
                        {

                            //get current topic category object
                            var currentTC = (from tc in db.Topic_Category
                                                 where tc.Topic_Seq.Equals(model.Topic_Seq)
                                                 select tc).FirstOrDefault();

                            //check if the category for the topic has changed
                            if (currentTC.Category_ID.Equals(category))
                            {

                                //if category hasnt changed only update the topic object
                                db.Entry(model).State = EntityState.Modified;
                                db.SaveChanges();


                                //display success message
                                TempData["Message"] = "Topic successfully updated";
                                TempData["classStyle"] = "success";

                                return RedirectToAction("viewTopic");
                            }
                            else
                            {
                                //topic category has been changed so must remove old topic category object
                                db.Topic_Category.Remove(currentTC);

                                //add details for the new topic category details
                                Topic_Category a = new Topic_Category
                                {
                                    Topic_Seq = model.Topic_Seq,
                                    Category_ID = category,
                                };

                                //capture the new topic category object
                                db.Topic_Category.Add(a);

                                //capture the updated topic details
                                db.Entry(model).State = EntityState.Modified;

                                //save changes to the database
                                db.SaveChanges();


                                //display message of success
                                TempData["Message"] = "Topic successfully updated";
                                TempData["classStyle"] = "success";

                                return RedirectToAction("viewTopic");
                            }
                        }
                    
                        //delete button was pressed go to delete action
                    case "Delete Topic":
                        return RedirectToAction("deleteTopic", "Trainer", new { id = model.Topic_Seq });

                    default:
                        //get list of existing categories
                        var existingCategories = from c in db.Categories
                                                 select c;

                        //pass list of existing categories to viewbag for select list
                        ViewBag.Category = existingCategories;

                        //get the topic category obkect for the selected topic
                        var currentTopCat = (from t in db.Topics
                                             join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                             where tc.Topic_Seq.Equals(model.Topic_Seq)
                                             select tc).FirstOrDefault();


                        //get the category for the selected topic
                        var currentCat = (from c in db.Categories
                                          where c.Category_ID.Equals(currentTopCat.Category_ID)
                                          select c.Category_ID).FirstOrDefault().ToString();


                        //assign the current category to a viewbag
                        ViewBag.currentCat = currentCat;


                        //return the view
                        return View(model);
                }               
            }
            else
            {
                //get list of existing categories
                var existingCategories = from c in db.Categories
                                         select c;

                //assign selected category to a viewbag
                ViewBag.Category = existingCategories;

                //get the selected topic topic category object
                var currentTopCat = (from t in db.Topics
                                     join tc in db.Topic_Category on t.Topic_Seq equals tc.Topic_Seq
                                     where tc.Topic_Seq.Equals(model.Topic_Seq)
                                     select tc).FirstOrDefault();


                //get the selected topic curremt category
                var currentCat = (from c in db.Categories
                                  where c.Category_ID.Equals(currentTopCat.Category_ID)
                                  select c.Category_ID).FirstOrDefault().ToString();

                //assing the current category to a viewbag
                ViewBag.currentCat = currentCat;

                //return view
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult addTopic()
        {
            //get list of current categories
            var categories = from c in db.Categories
                             select c;

            //assign current categories to a viewbag
            ViewBag.Categories = categories;

            //return the view
            return View();
        }

        [HttpPost]
        public ActionResult addTopic(Topic model, int category)
        {
            if (ModelState.IsValid)
            {
                //check if a topic with the same name already exists
                var check = from t in db.Topics
                            where t.Topic_Name.ToLower().Equals(model.Topic_Name)
                            select t;

                if (check.Any())
                {
                    //display error message
                    TempData["Message"] = "Topic with the same name already exists";
                    TempData["classStyle"] = "warning";

                    //return form
                    return View(model);
                }
                else
                {
                    //add new instance of topic to database
                    db.Topics.Add(model);
                    db.SaveChanges();

                    var newTopicSeq = (from t in db.Topics
                                       where t.Topic_Name.Equals(model.Topic_Name)
                                       select t.Topic_Seq).FirstOrDefault();
                    //create new instance of topic category
                    Topic_Category a = new Topic_Category
                    {
                        Topic_Seq = newTopicSeq,
                        Category_ID = category,
                    };

                    //add new instance of topic category to the database
                    db.Topic_Category.Add(a);

                    //save changes to the database
                    db.SaveChanges();

                    //display success message
                    TempData["Message"] = "Topic successfully added";
                    TempData["classStyle"] = "success";

                    //return view
                    return RedirectToAction("viewTopic");
                }
            }
            else
            {
                //get list of current categories
                var categories = from c in db.Categories
                                 select c;

                //assign current categories to a viewbag
                ViewBag.Categories = categories;

                return View(model);
            }                   
        }
            
    }
}