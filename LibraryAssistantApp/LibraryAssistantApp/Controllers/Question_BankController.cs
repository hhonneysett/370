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
    public class Question_BankController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Question_Bank
        public ActionResult Index(int? Topic, string Search)
        {
            //var question_Bank = db.Question_Bank.Include(q => q.Question_Topic).Include(q => q.Style_Type);

            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            return View(db.Question_Bank.Where(X => X.Question_Text.Contains(Search) && X.Topic_Seq == Topic || Search == null).ToList());

        }

        // GET: Question_Bank/Create
        public ActionResult Create()
        {
            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID");
            return View();
        }

        // POST: Question_Bank/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Question_Seq,Question_Text,Topic_Seq,Style_Type_ID")] Question_Bank question_Bank)
        {
            if (ModelState.IsValid)
            {
                db.Question_Bank.Add(question_Bank);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", question_Bank.Topic_Seq);
            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Description", question_Bank.Style_Type_ID);
            return View(question_Bank);
        }

        // GET: Question_Bank/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_Bank question_Bank = db.Question_Bank.Find(id);
            if (question_Bank == null)
            {
                return HttpNotFound();
            }
            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", question_Bank.Topic_Seq);
            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Description", question_Bank.Style_Type_ID);
            return View(question_Bank);
        }

        // POST: Question_Bank/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Question_Seq,Question_Text,Topic_Seq,Style_Type_ID")] Question_Bank question_Bank)
        {
            if (ModelState.IsValid)
            {
                db.Entry(question_Bank).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", question_Bank.Topic_Seq);
            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Description", question_Bank.Style_Type_ID);
            return View(question_Bank);
        }

        // GET: Question_Bank/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_Bank question_Bank = db.Question_Bank.Find(id);
            if (question_Bank == null)
            {
                return HttpNotFound();
            }
            return View(question_Bank);
        }

        // POST: Question_Bank/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question_Bank question_Bank = db.Question_Bank.Find(id);
            db.Question_Bank.Remove(question_Bank);
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
