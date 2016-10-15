using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class RepositoryController : Controller
    {
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Repository
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult ViewFile()
        {
            var files = from a in db.Document_Repository
                        where a.Document_Status.Equals("Active")
                        select a;


            return View(files);
        }

        // GET: Add file
        [Authorize(Roles="Admin, Employee")]
        public ActionResult AddFile()
        {
            ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
            ViewBag.Document_Type_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
            return View();
        }

        // POST: Add file
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFile(AddFileModel model)
        {
            //check if model state is valid
            if (ModelState.IsValid)
            {
                //get uploaded file filename
                var filename = Path.GetFileName(model.uploadFile.FileName);
                //get uploaded file path
                var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), filename);
                //determine the uploaded file extension
                var extension = Path.GetExtension(model.uploadFile.FileName);
                //check if the file uploaded is a valid extension
                if (System.IO.File.Exists(path))
                {
                    TempData["Message"] = "Uploaded file already exists";
                    TempData["classStyle"] = "warning";
                    model.uploadFile = null;
                    ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
                    ViewBag.Document_Type_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
                    return View(model);
                }
                else
                {
                    var validExtension = db.Document_Extension.Where(m => m.Extension_Type.Equals(extension)).FirstOrDefault();
                    if (validExtension != null)
                    {
                        //create a new instance of a document_repository object
                        Document_Repository a = new Document_Repository();
                        //add details of the file to a document_repository object
                        a.Document_Name = model.Document_Name;
                        a.Description = model.Description;
                        a.Category_ID = model.Category_ID;
                        a.Document_Type_ID = model.Document_Type_ID;
                        a.Directory_Path = path;
                        a.Document_Extension_ID = validExtension.Document_Extension_ID;
                        a.Document_Status = "Active";
                        db.Document_Repository.Add(a);
                        db.SaveChanges();

                        var sessionLog = db.Person_Session_Log.Where(p => p.Person_ID == User.Identity.Name).OrderByDescending(p => p.Login_DateTime).FirstOrDefault();

                        Document_Access_Log ac = new Document_Access_Log();
                        ac.Access_DateTime = DateTime.Now;
                        ac.Document_Seq = a.Document_Seq;
                        ac.Session_ID = sessionLog.Session_ID;

                        db.Document_Access_Log.Add(ac);
                        db.SaveChanges();

                        //record action
                        global.addAudit("Repository", "Repository: Add File", "Create", User.Identity.Name);

                        //save file to server
                        model.uploadFile.SaveAs(path);
                        TempData["Message"] = "File successfully uploaded";
                        TempData["classStyle"] = "success";
                        return RedirectToAction("ViewFile");
                    }
                    else
                    {
                        ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
                        ViewBag.Document_Type_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
                        return View();
                    }
                }
            }
            ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
            ViewBag.Document_Type_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
            return View();
        }

        // GET: Update file
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult UpdateFile(int id)
        {
            //get details of file to be updated
            var file = db.Document_Repository.Where(f => f.Document_Seq.Equals(id)).FirstOrDefault();
            //create the viewmodel to be passed
            UpdateFileModel a = new UpdateFileModel
            {
                Document_Name = file.Document_Name,
                Document_Type_ID = file.Document_Type_ID,
                Category_ID = file.Category_ID,
                Description = file.Description,
                Document_Seq = file.Document_Seq,
            };
            //populate select lists for drop downs
            ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
            ViewBag.Document_Type_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
            return View(a);
        }

        // Post: Update file
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult UpdateFile(UpdateFileModel model)
        {
            if (ModelState.IsValid)
            {
                //get instance of the file to be updated
                var updatedfile = db.Document_Repository.Where(f => f.Document_Seq.Equals(model.Document_Seq)).FirstOrDefault();
                //update the entries details
                updatedfile.Document_Name = model.Document_Name;
                updatedfile.Description = model.Description;
                updatedfile.Category_ID = model.Category_ID;
                updatedfile.Document_Type_ID = model.Document_Type_ID;
                //check if a new file has been uploaded
                if (model.uploadFile != null)
                {
                    //get uploaded file filename
                    var filename = Path.GetFileName(model.uploadFile.FileName);
                    //get uploaded file path
                    var path = Path.Combine(Server.MapPath("~/App_Data/Uploads"), filename);
                    //determine the uploaded file extension
                    var extension = Path.GetExtension(model.uploadFile.FileName);
                    //check if the file uploaded is a valid extension
                    if (System.IO.File.Exists(path))
                    {
                        TempData["Message"] = "Uploaded file already exists";
                        TempData["classStyle"] = "danger";
                        model.uploadFile = null;
                        ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
                        ViewBag.Document_Type_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
                        return View(model);
                    }
                    else
                    {
                        System.IO.File.Delete(updatedfile.Directory_Path);
                        model.uploadFile.SaveAs(path);
                        updatedfile.Directory_Path = path;
                    }
                }
                db.Entry(updatedfile).State = EntityState.Modified;
                db.SaveChanges();

                var sessionLog = db.Person_Session_Log.Where(p => p.Person_ID == User.Identity.Name).OrderByDescending(p => p.Login_DateTime).FirstOrDefault();

                Document_Access_Log ac = new Document_Access_Log();
                ac.Access_DateTime = DateTime.Now;
                ac.Document_Seq = updatedfile.Document_Seq;
                ac.Session_ID = sessionLog.Session_ID;

                db.Document_Access_Log.Add(ac);
                db.SaveChanges();

                //record action
                global.addAudit("Repository", "Repository: Update File", "Update", User.Identity.Name);

                TempData["Message"] = "File successfuly updated";
                TempData["classStyle"] = "success";
                return RedirectToAction("ViewFile");
            }
            else
            {
                ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
                ViewBag.Document_Type_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
                return View(model);
            }
        }

        // GET: Download file
        [Authorize]
        public FilePathResult DownloadFile(int id)
        {
            var file = db.Document_Repository.Where(f => f.Document_Seq.Equals(id)).FirstOrDefault();

            var sessionLog = db.Person_Session_Log.Where(p => p.Person_ID == User.Identity.Name).OrderByDescending(p => p.Login_DateTime).FirstOrDefault();

            Document_Access_Log ac = new Document_Access_Log();
            ac.Access_DateTime = DateTime.Now;
            ac.Document_Seq = file.Document_Seq;
            ac.Session_ID = sessionLog.Session_ID;

            db.Document_Access_Log.Add(ac);
            db.SaveChanges();

            var virtualDirectoryPath = file.Directory_Path;
            return File(virtualDirectoryPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualDirectoryPath));
        }

        // GET: Delete file
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult DeleteFile(int id)
        {
            var file = db.Document_Repository.Where(m => m.Document_Seq.Equals(id)).FirstOrDefault();

            //create an instance of a deletefilemodel
            DeleteFileModel a = new DeleteFileModel
            {
                //get details of file to be deleted
                Document_Seq = id,
                Document_Name = file.Document_Name,
                Description = file.Description,
                Category = (from b in db.Document_Category
                            where b.Category_ID.Equals(file.Category_ID)
                            select b.Category_Name).FirstOrDefault(),
                Document_Type_Name = (from c in db.Document_Type
                                      where c.Document_Type_ID.Equals(file.Document_Type_ID)
                                      select c.Document_Type_Name).FirstOrDefault(),
            };
            Session["FileDelete"] = a;
            return View(a);
        }

        // POST: Delete file
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult DeleteFile()
        {
            DeleteFileModel tempdatafile = (DeleteFileModel)Session["FileDelete"];
            var deleteFile = db.Document_Repository.Where(f => f.Document_Seq.Equals(tempdatafile.Document_Seq)).FirstOrDefault();

            var sessionLog = db.Person_Session_Log.Where(p => p.Person_ID == User.Identity.Name).OrderByDescending(p => p.Login_DateTime).FirstOrDefault();

            Document_Access_Log ac = new Document_Access_Log();
            ac.Access_DateTime = DateTime.Now;
            ac.Document_Seq = deleteFile.Document_Seq;
            ac.Session_ID = sessionLog.Session_ID;

            db.Document_Access_Log.Add(ac);
            db.SaveChanges();

            //record action
            global.addAudit("Repository", "Repository: Delete File", "Delete", User.Identity.Name);

            deleteFile.Document_Status = "Deleted";
            var virtualDirectoryPath = deleteFile.Directory_Path;
            if (System.IO.File.Exists(virtualDirectoryPath))
            {
                System.IO.File.Delete(virtualDirectoryPath);
            }
            db.Entry(deleteFile).State = EntityState.Modified;
            db.SaveChanges();
            TempData["Message"] = "File '" + deleteFile.Document_Name + "' successfuly deleted";
            TempData["classStyle"] = "success";
            Session.Remove("FileDelete");
            return RedirectToAction("ViewFile");
        }

        // GET: View file type
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult ViewFileType()
        {
            var fileType = (from a in db.Document_Type
                            select a);
            return View(fileType);
        }

        // GET: Add file type
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult AddFileType()
        {
            return View();
        }

        // POST: Add file tpye
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult AddFileType(AddFileTypeModel model)
        {
            if (ModelState.IsValid)
            {
                Document_Type a = new Document_Type
                {
                    Document_Type_Name = model.Type_Name,
                    Description = model.Description,
                };
                db.Document_Type.Add(a);
                db.SaveChanges();

                //record action
                global.addAudit("Repository", "Repository: Add File Type", "Create", User.Identity.Name);

                TempData["classStyle"] = "success";
                TempData["Message"] = "File type successfully added";
                return RedirectToAction("ViewFileType");
            }
            else
            {
                return View(model);
            }
        }

        // GET: Update file type
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult UpdateFileType(int id)
        {
            var fileType = db.Document_Type.Where(m => m.Document_Type_ID.Equals(id)).FirstOrDefault();
            UpdateFileType a = new UpdateFileType
            {
                Type_Name = fileType.Document_Type_Name,
                Description = fileType.Description,
            };
            Session["updateFileType"] = fileType;
            return View(a);
        }

        // POST: Update file type
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin, Employee")]
        public ActionResult UpdateFileType(UpdateFileType model)
        {
            if (ModelState.IsValid)
            {
                var oldFileType = (Document_Type)Session["updateFileType"];
                var types = db.Document_Type.ToList();
                var typeExists = types.Any(m => m.Document_Type_Name.ToLower() == model.Type_Name.ToLower() && m.Document_Type_ID != oldFileType.Document_Type_ID);
                if (typeExists)
                {
                    TempData["classStyle"] = "warning";
                    TempData["Message"] = "File type already exists";
                    return View(model);
                }
                else
                {
                    var type = db.Document_Type.Where(d => d.Document_Type_ID == oldFileType.Document_Type_ID).FirstOrDefault();
                    type.Document_Type_Name = model.Type_Name;
                    type.Description = model.Description;
                    db.Entry(type).State = EntityState.Modified;
                    db.SaveChanges();

                    //record action
                    global.addAudit("Repository", "Repository: Update File Type", "Update", User.Identity.Name);

                    TempData["classStyle"] = "success";
                    TempData["Message"] = "File type successfuly updated";
                    return RedirectToAction("ViewFileType");
                }
            }
            else
            {
                return View(model);
            }
        }
        
        //delete file type - get
        public ActionResult DeleteFileType(int id)
        {
            Session["typeID"] = id;
            var type = db.Document_Type.Where(t => t.Document_Type_ID == id).FirstOrDefault();
            return View(type);
        }

        //delete file type = post
        [HttpPost]
        public ActionResult DeleteFileType()
        {
            int id = (int)Session["typeID"];
            var type = db.Document_Type.Where(t => t.Document_Type_ID == id).FirstOrDefault();
            var check = db.Document_Repository.Where(d => d.Document_Type_ID == id);
            if (check.Any())
            {
                TempData["Message"] = "Unable to delete, file type has existing dependencies!";
                TempData["classStyle"] = "danger";
                return View(type);
            }
            else
            {
                db.Document_Type.Attach(type);
                db.Document_Type.Remove(type);
                db.SaveChanges();

                //record action
                global.addAudit("Repository", "Repository: Delete File Type", "Delete", User.Identity.Name);

                TempData["Message"] = "Successfully deleted file type!";
                TempData["classStyle"] = "success";
                return RedirectToAction("ViewFileType");

            }
        }

        //view file categories
        public ActionResult viewFileCategories()
        {
            var categories = db.Document_Category.ToList();
            return View(categories);
        }

        //add file category - get
        public ActionResult addFileCategory()
        {
            return View();
        }

        //add file category - post
        [HttpPost]
        public ActionResult addFileCategory(AddFileCategory model)
        {
            if (ModelState.IsValid)
            {
                Document_Category category = new Document_Category
                {
                    Category_Name = model.name,
                    Description = model.description,
                };

                db.Document_Category.Add(category);
                db.SaveChanges();

                //record action
                global.addAudit("Repository", "Repository: Add File Category", "Create", User.Identity.Name);

                TempData["Message"] = "Document category successfully added!";
                TempData["classStyle"] = "success";
                return RedirectToAction("viewFileCategories");
            }
            else
            {
                TempData["Message"] = "Invalid category details!";
                TempData["classStyle"] = "danger";
                return View(model);
            }
        }

        //update file category - get
        public ActionResult updateFileCategory(int id)
        {
            var category = db.Document_Category.Where(c => c.Category_ID == id).FirstOrDefault();
            var update = new UpdateFileCategory
            {
                name = category.Category_Name,
                description = category.Description,
                id = category.Category_ID,
            };
            return View(update);
        }

        //update file category - post
        [HttpPost]
        public ActionResult updateFileCategory(UpdateFileCategory model)
        {
            var category = db.Document_Category.Where(c => c.Category_ID == model.id).FirstOrDefault();
            var categories = db.Document_Category.ToList();
            var check = categories.Any(c => c.Category_Name.ToLower() == model.name.ToLower() && c.Category_ID != model.id);
            if (!check)
            {
                category.Category_Name = model.name;
                category.Description = model.description;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();

                //record action
                global.addAudit("Repository", "Repository: Update File Category", "Update", User.Identity.Name);

                return RedirectToAction("viewFileCategories");
            }
            else
            {
                TempData["Message"] = "Categeory already exists!";
                TempData["classStyle"] = "danger";
                return View(model);
            }
            
        }

        //delete file category - get
        public ActionResult deleteFileCategory(int id)
        {
            Session["catID"] = id;
            var category = db.Document_Category.Where(c => c.Category_ID == id).FirstOrDefault();
            return View(category);
        }

        //delete file category - post
        [HttpPost]
        public ActionResult deleteFileCategory()
        {
            var id = (int)Session["catID"];
            var category = db.Document_Category.Where(c => c.Category_ID == id).FirstOrDefault();
            var check = db.Document_Repository.Where(d => d.Category_ID == id);
            if (check.Any())
            {
                TempData["Message"] = "File category has existing dependencies!";
                TempData["classStyle"] = "danger";
                return View(category);
            }
            else
            {
                db.Document_Category.Attach(category);
                db.Document_Category.Remove(category);
                db.SaveChanges();

                //record action
                global.addAudit("Repository", "Repository: Delete File Category", "Delete", User.Identity.Name);

                TempData["Message"] = "File category successfully deleted!";
                TempData["classStyle"] = "success";
                return RedirectToAction("viewFileCategories");
            }
        }

        // GET: View library map file
        [Authorize]
        public FilePathResult ViewMap()
        {
            var file = db.Document_Repository.Where(f => f.Document_Name.Equals("Library Map")).FirstOrDefault();
            var virtualDirectoryPath = file.Directory_Path;
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + Path.GetFileName(virtualDirectoryPath));
            return File(virtualDirectoryPath, mimeType);
        }

        // GET: View tutorials
        [Authorize]
        public ActionResult ViewTutorials()
        {
            return View();
        }

        // GET: Get document types
        [HttpGet]
        [Authorize]
        public PartialViewResult GetTypes()
        {
            ViewBag.Category_ID = new SelectList(db.Document_Type, "Document_Type_ID", "Document_Type_Name");
            return PartialView();
        }

        // POST: Get tutorials from selected drop down list
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult GetTypes(GetTypesModel model)
        {
            if (ModelState.IsValid)
            {
                var typeID = (from a in db.Document_Type
                              where a.Document_Type_Name.Equals("Tutorial")
                              select a.Document_Type_ID).FirstOrDefault();

                var catName = (from b in db.Document_Category
                               where b.Category_ID.Equals(model.Category_ID)
                               select b.Category_Name).FirstOrDefault();

                Session["catSelection"] = model.Category_ID;
                Session["typeID"] = typeID;
                Session["catName"] = catName;
                return RedirectToAction("GetTutorials");
            }
            else return View("ViewTutorials");
        }

        // GET: Get tutorials
        [HttpGet]
        [Authorize]
        public ActionResult GetTutorials()
        {
            string sSessionCat;
            string sSessionType;
            int catSelection = 0;
            int typeID = 0;

            try
            {
                sSessionCat = (string)(Session["catSelection"]) ?? "empty";
                sSessionType = (string)(Session["typeID"]) ?? "empty";
                if (sSessionCat.Equals("empty") && sSessionType.Equals("empty"))
                {
                    catSelection = 0;
                    typeID = 0;
                }
            }
            catch
            {
                catSelection = (int)Session["catSelection"];
                typeID = (int)Session["typeID"];
            }

            var matchingTutorials = db.Document_Repository.Where(f => f.Document_Type_ID.Equals(typeID) && f.Category_ID.Equals(catSelection));
            if (matchingTutorials.Any())
            {
                IEnumerable<Document_Repository> model = matchingTutorials;
                return View(model);
            }
            else
            {
                TempData["classStyle"] = "warning";
                TempData["Message"] = "No tutorials exist for the selected category";
                return View("ViewTutorials");
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}