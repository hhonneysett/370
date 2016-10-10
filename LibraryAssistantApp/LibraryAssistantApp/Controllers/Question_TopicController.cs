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
    public class Question_TopicController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Question_Topic
        public ActionResult Index()
        {
            return View(db.Question_Topic.ToList());
        }
        public ActionResult Finished(string TopicName)
        {
            ViewBag.CreateComplete = "Yes";
            @ViewBag.CreateCompleteMessage = "Question topic '"+TopicName+"' successfully created";
            return View("Index",db.Question_Topic.ToList());
        }
        
            public ActionResult FinishEdit(int Topic)
        {
            string TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic).Select(Y => Y.Topic_Name).Single();
            ViewBag.EditComplete = "Yes";
            @ViewBag.EditCompleteMessage = "Question topic '"+ TopicName +"' successfully edited";
            return View("Index", db.Question_Topic.ToList());
        }
        // GET: Question_Topic/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_Topic question_Topic = db.Question_Topic.Find(id);
            if (question_Topic == null)
            {
                return HttpNotFound();
            }
            return View(question_Topic);
        }

        // GET: Question_Topic/Create
        public ActionResult Create()
        {
            return View();
        }

        public ActionResult Create_Back_From_Create_Question(string Topic_Name, string Topic_Des)
        {

            Question_Topic question_Topic = db.Question_Topic.Where(X => X.Topic_Name == Topic_Name).Single();

            //// -------------------------------Action Log ----------------------------------------//
            //string name = db.Question_Topic.Where(X => X.Topic_Seq == question_Topic.Topic_Seq).Select(Y => Y.Topic_Name).Single();


            //db.Person_Session_Action_Log.Remove(db.Person_Session_Action_Log.Where(X => X.Action_Performed == "Created question topic: " + name).Single());
            //db.SaveChanges();
            //// -------------------------------Action Log ----------------------------------------//

            
            db.Question_Topic.Remove(question_Topic);
            db.SaveChanges();

            
            ViewBag.Topic_Name = Topic_Name;
            ViewBag.Topic_Des = Topic_Des;
            return View("Create");
        }

        // POST: Question_Topic/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Topic_Seq,Topic_Name,Description")] Question_Topic question_Topic)
        {
            if (ModelState.IsValid)
            {
                if (AreThereDuplicatesTopics(question_Topic.Topic_Name))
                {
                    ModelState.AddModelError("Topic_Name", "This topic already exists. Please choose another topic name.");
                    ViewBag.Topic_Name = question_Topic.Topic_Name;
                    ViewBag.Description = question_Topic.Description;
                    return View("Create");
                }
                else
                {
                    db.Question_Topic.Add(question_Topic);
                    db.SaveChanges();


                    // -------------------------------Action Log ----------------------------------------//
                    string name = db.Question_Topic.Where(X => X.Topic_Seq == question_Topic.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                    Person_Session_Action_Log psal = new Person_Session_Action_Log();
                    psal.Action_DateTime = DateTime.Now;
                    psal.Action_ID = 11;
                    psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                    psal.Action_Performed = "Question topic: " + name;
                    psal.Crud_Operation = "Create";
                    db.Person_Session_Action_Log.Add(psal);
                    db.SaveChanges();
                    // -------------------------------Action Log ----------------------------------------//

                    string NewTopic = "YES";
                    return RedirectToAction("Add_Question_To_New_Topic", "Question_Bank", new { question_Topic.Topic_Seq, NewTopic, question_Topic.Topic_Name, question_Topic.Description });
                }
            }

            return View(question_Topic);
        }

        public bool AreThereDuplicatesTopics(string Topic_Name)
        {
            if (db.Question_Topic.Any(o => o.Topic_Name == Topic_Name))
            {
                return true;
            }
            else
            {
                return false;
            }


        }

        // GET: Question_Topic/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_Topic question_Topic = db.Question_Topic.Find(id);
            if (question_Topic == null)
            {
                return HttpNotFound();
            }
            return View(question_Topic);
        }

        // POST: Question_Topic/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Topic_Seq,Topic_Name,Description")] Question_Topic question_Topic)
        {
            if (ModelState.IsValid)
            {
                if (AreThereDuplicatesTopics(question_Topic.Topic_Name))
                {                    
                    if (db.Question_Topic.Where(X => X.Topic_Seq == question_Topic.Topic_Seq).Select(Y => Y.Topic_Name).Single() == question_Topic.Topic_Name)
                    {
                        db.Entry(question_Topic).State = EntityState.Modified;
                        db.SaveChanges();

                        // -------------------------------Action Log ----------------------------------------//
                        string name = db.Question_Topic.Where(X => X.Topic_Seq == question_Topic.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                        Person_Session_Action_Log psal = new Person_Session_Action_Log();
                        psal.Action_DateTime = DateTime.Now;
                        psal.Action_ID = 11;
                        psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();                        
                        psal.Action_Performed = "Question topic: " + name;
                        psal.Crud_Operation = "Edit";
                        db.Person_Session_Action_Log.Add(psal);
                        db.SaveChanges();
                        // -------------------------------Action Log ----------------------------------------//


                        ViewBag.Topic = question_Topic.Topic_Seq;
                        return View("Edit_Some_More");
                    }
                    else
                    {
                        ModelState.AddModelError("Topic_Name", "This topic already exists. Please choose another topic name.");
                        ViewBag.Topic_Name = question_Topic.Topic_Name;
                        ViewBag.Description = question_Topic.Description;
                        return View("Edit");
                    }
                }
                else
                {
                    db.Entry(question_Topic).State = EntityState.Modified;
                    db.SaveChanges();


                    // -------------------------------Action Log ----------------------------------------//
                    string name = db.Question_Topic.Where(X => X.Topic_Seq == question_Topic.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                    Person_Session_Action_Log psal = new Person_Session_Action_Log();
                    psal.Action_DateTime = DateTime.Now;
                    psal.Action_ID = 11;
                    psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                    psal.Crud_Operation = "Edit";
                    psal.Action_Performed = "Question topic: " + name;
                    db.Person_Session_Action_Log.Add(psal);
                    db.SaveChanges();
                    // -------------------------------Action Log ----------------------------------------//

                    ViewBag.Topic = question_Topic.Topic_Seq;
                    return View("Edit_Some_More");
                }                             
            }
            return View(question_Topic);
        }

        // GET: Question_Topic/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Question_Topic question_Topic = db.Question_Topic.Find(id);
            if (question_Topic == null)
            {
                return HttpNotFound();
            }            

            int _Count = 0;
            _Count = db.Questionnaires.Where(X => X.Topic_Seq == id).Count();

            if (_Count != 0)
            {
                ViewBag.Error = "This topic cannot be deleted as it is being used in a questionnaire";
               }


            return View(question_Topic);
        }

        // POST: Question_Topic/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Question_Topic question_Topic = db.Question_Topic.Find(id);

            int _Count = 0;
            _Count = db.Questionnaires.Where(X => X.Topic_Seq == id).Count();

            if (_Count != 0)
            {
                ViewBag.Error = "This topic cannot be deleted as it is being used in a questionnaire";
                Question_Topic q_Topic = db.Question_Topic.Find(id);
                return View(q_Topic);
            }
            else
            {


                // -------------------------------Action Log ----------------------------------------//
                string name = db.Question_Topic.Where(X => X.Topic_Seq == question_Topic.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                psal.Action_DateTime = DateTime.Now;
                psal.Action_ID = 11;
                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                psal.Crud_Operation = "Delete";
                psal.Action_Performed = "Question topic: " + name;

                db.Person_Session_Action_Log.Add(psal);
                db.SaveChanges();
                // -------------------------------Action Log ----------------------------------------//

                List<Question_Bank> question_Bank = db.Question_Bank.Where(X => X.Topic_Seq == question_Topic.Topic_Seq).ToList();
                @ViewBag.DeleteCompleteMessage = "Question topic '" + question_Topic.Topic_Name + "' successfully deleted.";
                db.Question_Bank.RemoveRange(question_Bank);

                db.Question_Topic.Remove(question_Topic);

                db.SaveChanges();
                ViewBag.DeleteComplete = "Yes";
                
                return View("Index",db.Question_Topic.ToList());
               
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
