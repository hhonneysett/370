using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using LibraryAssistantApp.Models;

namespace LibraryAssistantApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CampusController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Campus
        public ActionResult Index()
        {
            return View(db.Campus.ToList());
        }

        // GET: Campus/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campus campus = db.Campus.Where(X => X.Campus_ID == id).Single();
            if (campus == null)
            {
                return HttpNotFound();
            }
            return View(campus);
        }

        // GET: Campus/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Campus/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Campus_Name")] Campus campus)
        {
            if (ModelState.IsValid)
            {
                if (db.Campus.Any(X => X.Campus_Name == campus.Campus_Name))
                {
                    ModelState.AddModelError("Campus_Name", "A campus with this name already exists. Please choose another name");
                    return View(campus);
                }
                else
                {
                    int maxId = this.db.Campus.Max(table => table.Campus_ID);
                    campus.Campus_ID = maxId + 1;

                    db.Campus.Add(campus);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }

            return View(campus);
        }

        // GET: Campus/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campus campus = db.Campus.Where(X => X.Campus_ID == id).Single();
            if (campus == null)
            {
                return HttpNotFound();
            }
            return View(campus);
        }

        // POST: Campus/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Campus_ID,Campus_Name")] Campus campus)
        {
            if (ModelState.IsValid)
            {
                db.Entry(campus).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(campus);
        }

        // GET: Campus/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Campus campus = db.Campus.Where(X => X.Campus_ID == id).Single();
            if (campus == null)
            {
                return HttpNotFound();
            }


            return View(campus);
        }

        // POST: Campus/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            
            if (db.Buildings.Any(X => X.Campus_ID == id))
            {
                ModelState.AddModelError("Campus_Name", "This campus cannot be deleted as it has buildings assigned to it");
                Campus campus = db.Campus.Where(X => X.Campus_ID == id).Single();
                return View("Delete",campus);
            }
            else
            {
                Campus campus = db.Campus.Where(X => X.Campus_ID == id).Single();
                db.Campus.Remove(campus);
                db.SaveChanges();
                return RedirectToAction("Index");

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
