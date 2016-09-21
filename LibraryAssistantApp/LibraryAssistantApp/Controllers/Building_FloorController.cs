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
    public class Building_FloorController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Building_Floor
        public ActionResult Index()
        {
            var building_Floor = db.Building_Floor.Include(b => b.Building);
            return View(building_Floor.ToList());
        }

        // GET: Building_Floor/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building_Floor building_Floor = db.Building_Floor.Where(X => X.Building_Floor_ID == id).Single();
            if (building_Floor == null)
            {
                return HttpNotFound();
            }
            return View(building_Floor);
        }

        // GET: Building_Floor/Create
        public ActionResult Create(int Campus_ID, int Building_ID)
        {
            ViewBag.Campus_ID = Campus_ID;
            ViewBag.Building_ID = Building_ID;
          
          
            return View();
        }

        // POST: Building_Floor/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Campus_ID,Building_ID,Floor_Name")] Building_Floor building_Floor)
        {
            if (ModelState.IsValid)
            {   
                int maxId = this.db.Building_Floor.Max(table => table.Building_Floor_ID);
                building_Floor.Building_Floor_ID = maxId + 1;

                db.Building_Floor.Add(building_Floor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Campus_ID = building_Floor.Campus_ID;
            ViewBag.Building_ID = building_Floor.Building_ID;
            return View(building_Floor);
        }

        // GET: Building_Floor/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building_Floor building_Floor = db.Building_Floor.Where(X => X.Building_Floor_ID == id).Single();
            if (building_Floor == null)
            {
                return HttpNotFound();
            }
            ViewBag.Campus_ID = new SelectList(db.Buildings, "Campus_ID", "Building_Name", building_Floor.Campus_ID);
            return View(building_Floor);
        }

        // POST: Building_Floor/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Campus_ID,Building_ID,Building_Floor_ID,Floor_Name")] Building_Floor building_Floor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(building_Floor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Campus_ID = building_Floor.Campus_ID;
            ViewBag.Building_ID = building_Floor.Building_ID;
            return View(building_Floor);
        }

        // GET: Building_Floor/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Building_Floor building_Floor = db.Building_Floor.Where(X => X.Building_Floor_ID == id).Single();
            if (building_Floor == null)
            {
                return HttpNotFound();
            }
            return View(building_Floor);
        }

        // POST: Building_Floor/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (db.Venues.Any(X => X.Venue_ID == id))
            {
                ModelState.AddModelError("Building_Floor_ID", "This building floor cannot be deleted as it has venues assigned to it");
                Building_Floor building_Floor_ = db.Building_Floor.Where(X => X.Building_Floor_ID == id).Single();
                return View("Delete", building_Floor_);
            }
            else
            {
            }


                Building_Floor building_Floor = db.Building_Floor.Where(X => X.Building_Floor_ID == id).Single();
            db.Building_Floor.Remove(building_Floor);
            db.SaveChanges();
            return RedirectToAction("Index");
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
