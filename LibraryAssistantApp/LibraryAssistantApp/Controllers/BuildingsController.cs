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
    public class BuildingsController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Buildings
        public ActionResult Index()
        {
            var buildings = db.Buildings.Include(b => b.Campu);
            return View(buildings.ToList());
        }

        // GET: Buildings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Where(X => X.Building_ID == id).Single();
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // GET: Buildings/Create
        public ActionResult Create(int Campus_ID)
        { 

            ViewBag.Campus_ID = Campus_ID;
             return View();
        }

        // POST: Buildings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Campus_ID,Building_Name")] Building building)
        {
            if (ModelState.IsValid)
            {
                if (db.Buildings.Any(X => X.Building_Name == building.Building_Name))
                {
                    ModelState.AddModelError("Building_Name", "A building with this name already exists. Please choose another name");
                    ViewBag.Campus_ID = building.Campus_ID;
                    return View(building);
                }
                else
                {
                    int maxId = this.db.Buildings.Max(table => table.Building_ID);
                    building.Building_ID = maxId + 1;

                    db.Buildings.Add(building);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }
            ViewBag.Campus_ID = building.Campus_ID;
            return View(building);
        }

        // GET: Buildings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Where(X => X.Building_ID == id).Single();
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Campus_ID,Building_ID,Building_Name")] Building building)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Campus_ID = building.Campus_ID;
            return View(building);
        }

        // GET: Buildings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building building = db.Buildings.Where(X => X.Building_ID == id).Single();
            if (building == null)
            {
                return HttpNotFound();
            }
            return View(building);
        }

        // POST: Buildings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (db.Building_Floor.Any(X => X.Building_ID == id))
            {
                ModelState.AddModelError("Building_Name", "This building cannot be deleted as it has building floors assigned to it");
                Building building = db.Buildings.Where(X => X.Building_ID == id).Single();
                return View("Delete", building);
            }
            else
            {
                Building building = db.Buildings.Where(X => X.Building_ID == id).Single();
                db.Buildings.Remove(building);
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
