using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Models
{
    [Authorize(Roles ="Admin")]
    public class VenuesController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Venues
        public ActionResult Index()
        {           
            var venues = db.Venues.Include(v => v.Building_Floor).Include(v => v.Venue_Type1);
            return View(venues.ToList());
        }

        // GET: Venues/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = db.Venues.Where(X => X.Venue_ID == id).Single();
            if (venue == null)
            {
                return HttpNotFound();
            }
            return View(venue);
        }

        // GET: Venues/Create
        public ActionResult Create(int Campus_ID, int Building_ID, int Building_Floor_ID)
        {
            ViewBag.Campus_ID = Campus_ID;
            ViewBag.Building_ID = Building_ID;
            ViewBag.Building_Floor_ID = Building_Floor_ID;
            ViewBag.Venue_Type = new SelectList(db.Venue_Type, "Venue_Type1", "Venue_Type1");

            return View();
        }

        // POST: Venues/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Campus_ID,Building_ID,Building_Floor_ID,Venue_Name,Venue_Type,Capacity")] Venue venue)
        {
            if (ModelState.IsValid)
            {

                if (db.Venues.Any(X => X.Venue_Name == venue.Venue_Name))
                {
                    ModelState.AddModelError("Venue_Name", "A campus with this name already exists. Please choose another name");
                    ViewBag.Campus_ID = venue.Campus_ID;
                    ViewBag.Building_ID = venue.Building_ID;
                    ViewBag.Building_Floor_ID = venue.Building_Floor_ID;
                    ViewBag.Venue_Type = new SelectList(db.Venue_Type, "Venue_Type1", "Venue_Type1");


                    return View(venue);
                }
                else
                {
                    int maxId = this.db.Venues.Max(table => table.Venue_ID);
                    venue.Venue_ID = maxId + 1;
                    db.Venues.Add(venue);
                    db.SaveChanges();
                    return RedirectToAction("Index");

                }
            }

            ViewBag.Campus_ID = venue.Campus_ID;
            ViewBag.Building_ID = venue.Building_ID;
            ViewBag.Building_Floor_ID = venue.Building_Floor_ID;
            
            ViewBag.Venue_Type = new SelectList(db.Venue_Type, "Venue_Type1", "Venue_Type1", venue.Venue_Type);
            return View(venue);
        }

        // GET: Venues/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = db.Venues.Where(X => X.Venue_ID == id).Single();
            if (venue == null)
            {
                return HttpNotFound();
            }
            ViewBag.Campus_ID = new SelectList(db.Building_Floor, "Campus_ID", "Campus_ID", venue.Campus_ID);
            ViewBag.Venue_Type = new SelectList(db.Venue_Type, "Venue_Type1", "Venue_Type1", venue.Venue_Type);
            return View(venue);
        }

        // POST: Venues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Campus_ID,Building_ID,Building_Floor_ID,Venue_ID,Venue_Name,Venue_Type,Capacity")] Venue venue)
        {
            if (ModelState.IsValid)
            {
                db.Entry(venue).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Campus_ID = venue.Campus_ID;
            ViewBag.Building_ID = venue.Building_ID;
            ViewBag.Building_Floor_ID = venue.Building_Floor_ID;
            ViewBag.Venue_Type = new SelectList(db.Venue_Type, "Venue_Type1", "Venue_Type1", venue.Venue_Type);
            return View(venue);
        }

        // GET: Venues/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Venue venue = db.Venues.Where(X => X.Venue_ID == id).Single();
            if (venue == null)
            {
                return HttpNotFound();
            }
            return View(venue);
        }

        // POST: Venues/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (db.Venue_Booking.Any(X => X.Venue_ID == id))
            {
                ModelState.AddModelError("Venue_Name", "This venue cannot be deleted as it has bookings assigned to it");

                Venue venue = db.Venues.Where(X => X.Venue_ID == id).Single();
                return View("Delete", venue);
            }
            else
            {
                Venue venue = db.Venues.Where(X => X.Venue_ID == id).Single();
                db.Venues.Remove(venue);
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
