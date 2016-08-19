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
        public ActionResult ViewFile()
        {
            var files = from a in db.Document_Repository
                        where a.Document_Status.Equals("Active")
                        select a;
            return View(files);
        }

        // GET: Add file
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
                TempData["Message"] = "File successfuly updated";
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
        public FilePathResult DownloadFile(int id)
        {
            var file = db.Document_Repository.Where(f => f.Document_Seq.Equals(id)).FirstOrDefault();
            var virtualDirectoryPath = file.Directory_Path;
            return File(virtualDirectoryPath, System.Net.Mime.MediaTypeNames.Application.Octet, Path.GetFileName(virtualDirectoryPath));
        }

        // GET: Delete file
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
            TempData["FileDelete"] = a;
            return View(a);
        }

        // POST: Delete file
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteFile()
        {
            DeleteFileModel tempdatafile = (DeleteFileModel)TempData["FileDelete"];
            var deleteFile = db.Document_Repository.Where(f => f.Document_Seq.Equals(tempdatafile.Document_Seq)).FirstOrDefault();
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
            return RedirectToAction("ViewFile");
        }

        // GET: View file type
        public ActionResult ViewFileType()
        {
            var fileType = (from a in db.Document_Type
                            select a);
            return View(fileType);
        }

        // GET: Add file type
        public ActionResult AddFileType()
        {
            return View();
        }

        // POST: Add file tpye
        [HttpPost]
        [ValidateAntiForgeryToken]
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
                TempData["classStyle"] = "success";
                TempData["Message"] = "File type successfully added";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View(model);
            }
        }

        // GET: Update file type
        public ActionResult UpdateFileType(int id)
        {
            var fileType = db.Document_Type.Where(m => m.Document_Type_ID.Equals(id)).FirstOrDefault();
            AddFileTypeModel a = new AddFileTypeModel
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
        public ActionResult UpdateFileType(AddFileTypeModel model)
        {
            if (ModelState.IsValid)
            {
                var oldFileType = (Document_Type)Session["updateFileType"];
                var typeExists = db.Document_Type.Any(m => m.Document_Type_Name.Equals(model.Type_Name) && m.Document_Type_ID != oldFileType.Document_Type_ID);
                if (typeExists)
                {
                    TempData["classStyle"] = "warning";
                    TempData["Message"] = "File type already exists";
                    model.Type_Name = "TEST";
                    return View(model);
                }
                else
                {
                    Document_Type a = new Document_Type
                    {
                        Document_Type_ID = oldFileType.Document_Type_ID,
                        Document_Type_Name = model.Type_Name,
                        Description = model.Description,
                    };
                    db.Entry(a).State = EntityState.Modified;
                    db.SaveChanges();
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

        // GET: View library map file
        public FilePathResult ViewMap()
        {
            var file = db.Document_Repository.Where(f => f.Document_Name.Equals("Library Map")).FirstOrDefault();
            var virtualDirectoryPath = file.Directory_Path;
            string mimeType = "application/pdf";
            Response.AppendHeader("Content-Disposition", "inline; filename=" + Path.GetFileName(virtualDirectoryPath));
            return File(virtualDirectoryPath, mimeType);
        }

        // GET: View tutorials
        public ActionResult ViewTutorials()
        {
            return View();
        }

        // GET: Get document types
        [HttpGet]
        public PartialViewResult GetTypes()
        {
            ViewBag.Category_ID = new SelectList(db.Document_Category, "Category_ID", "Category_Name");
            return PartialView();
        }

        // POST: Get tutorials from selected drop down list
        [HttpPost]
        [ValidateAntiForgeryToken]
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