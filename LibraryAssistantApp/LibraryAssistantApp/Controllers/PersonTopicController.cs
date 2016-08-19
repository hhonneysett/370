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
    public class PersonTopicController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        public PartialViewResult StudentTopic()
        {
            var topicList = from a in db.Person_Topic
                            where a.Person_ID.Equals(User.Identity.Name)
                            select a;
            return PartialView("StudentTopic", topicList);
        }

        public ActionResult UpdatePersonTopic()
        {
            return View();
        }

        // GET: PersonTopic/Add
        public PartialViewResult AddPersonTopic()
        {
            var topicList = from a in db.Topics
                            join b in db.Person_Topic on a.Topic_Seq equals b.Topic_Seq
                            where b.Person_ID.Equals(User.Identity.Name)
                            select a;

            var peopleDifference =  from person2 in db.Topics
                                    where !(
                                    from person1 in topicList
                                    select person1.Topic_Seq
                                    ).Contains(person2.Topic_Seq)
                                    select person2;
            if (peopleDifference.Count()==0)
            {
                ViewBag.Topic_Seq = null;
                return PartialView();
            }
            else
            {
                ViewBag.Topic_Seq = new SelectList(peopleDifference, "Topic_Seq", "Topic_Name");
                return PartialView();
            }            
        }

        // POST: PersonTopic/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult AddPersonTopic(AddPersonTopicModel model)
        {
            var topicList = from a in db.Topics
                            join b in db.Person_Topic on a.Topic_Seq equals b.Topic_Seq
                            where b.Person_ID.Equals(User.Identity.Name)
                            select a;

            var peopleDifference = from person2 in db.Topics
                                   where !(
                                   from person1 in topicList
                                   select person1.Topic_Seq
                                   ).Contains(person2.Topic_Seq)
                                   select person2;

            if (ModelState.IsValid)
            {
                Person_Topic a = new Person_Topic();
                a.Topic_Seq = model.Topic_Seq;
                a.Person_ID = User.Identity.Name;
                db.Person_Topic.Add(a);
                db.SaveChanges();
                TempData["Message"] = "Topic Added";
                if (peopleDifference.Count() == 0)
                {
                    ViewBag.Topic_Seq = null;
                    return PartialView();
                }
                else
                {
                    ViewBag.Topic_Seq = new SelectList((IEnumerable<Topic>)peopleDifference, "Topic_Seq", "Topic_Name");
                    return PartialView();
                }
            }
            else
            {
                ViewBag.Topic_Seq = new SelectList((IEnumerable<Topic>)peopleDifference, "Topic_Seq", "Topic_Name");
                return PartialView();
            }
        }

        // GET: PersonTopic/Delete
        public PartialViewResult DeletePersonTopic()
        {
            var topicList = from a in db.Topics
                            join b in db.Person_Topic on a.Topic_Seq equals b.Topic_Seq
                            where b.Person_ID.Equals(User.Identity.Name)
                            select a;

            ViewBag.Topic_Sequence = new SelectList(topicList, "Topic_Seq", "Topic_Name");

            return PartialView();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public PartialViewResult DeletePersonTopic(DeletePersonTopicModel model)
        {
            Person_Topic person_Topic = db.Person_Topic.Find(User.Identity.Name, model.Topic_Sequence);
            db.Person_Topic.Remove(person_Topic);
            db.SaveChanges();
            var topicList = from a in db.Topics
                            join b in db.Person_Topic on a.Topic_Seq equals b.Topic_Seq
                            where b.Person_ID.Equals(User.Identity.Name)
                            select a;

            ViewBag.Topic_Sequence = new SelectList(topicList, "Topic_Seq", "Topic_Name");
            return PartialView();
        }

        // GET: PersonTopic
        public ActionResult Index()
        {
            var person_Topic = db.Person_Topic.Include(p => p.Registered_Person).Include(p => p.Topic);
            return View(person_Topic.ToList());
        }

        // GET: PersonTopic/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person_Topic person_Topic = db.Person_Topic.Find(id);
            if (person_Topic == null)
            {
                return HttpNotFound();
            }
            return View(person_Topic);
        }

        // GET: PersonTopic/Create
        public ActionResult Create()
        {
            ViewBag.Person_ID = new SelectList(db.Registered_Person, "Person_ID", "Person_Name");
            ViewBag.Topic_Seq = new SelectList(db.Topics, "Topic_Seq", "Topic_Name");
            return View();
        }

        // POST: PersonTopic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Person_ID,Topic_Seq")] Person_Topic person_Topic)
        {
            if (ModelState.IsValid)
            {
                db.Person_Topic.Add(person_Topic);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Person_ID = new SelectList(db.Registered_Person, "Person_ID", "Person_Name", person_Topic.Person_ID);
            ViewBag.Topic_Seq = new SelectList(db.Topics, "Topic_Seq", "Topic_Name", person_Topic.Topic_Seq);
            return View(person_Topic);
        }

        // GET: PersonTopic/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person_Topic person_Topic = db.Person_Topic.Find(id);
            if (person_Topic == null)
            {
                return HttpNotFound();
            }
            ViewBag.Person_ID = new SelectList(db.Registered_Person, "Person_ID", "Person_Name", person_Topic.Person_ID);
            ViewBag.Topic_Seq = new SelectList(db.Topics, "Topic_Seq", "Topic_Name", person_Topic.Topic_Seq);
            return View(person_Topic);
        }

        // POST: PersonTopic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Person_ID,Topic_Seq")] Person_Topic person_Topic)
        {
            if (ModelState.IsValid)
            {
                db.Entry(person_Topic).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Person_ID = new SelectList(db.Registered_Person, "Person_ID", "Person_Name", person_Topic.Person_ID);
            ViewBag.Topic_Seq = new SelectList(db.Topics, "Topic_Seq", "Topic_Name", person_Topic.Topic_Seq);
            return View(person_Topic);
        }

        // GET: PersonTopic/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person_Topic person_Topic = db.Person_Topic.Find(id);
            if (person_Topic == null)
            {
                return HttpNotFound();
            }
            return View(person_Topic);
        }

        // POST: PersonTopic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Person_Topic person_Topic = db.Person_Topic.Find(id);
            db.Person_Topic.Remove(person_Topic);
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
