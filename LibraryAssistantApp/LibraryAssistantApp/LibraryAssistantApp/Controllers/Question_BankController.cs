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
    public class Question_BankController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Question_Bank
        public ActionResult Index(int? Topic, string Search)
        {
            //var question_Bank = db.Question_Bank.Include(q => q.Question_Topic).Include(q => q.Style_Type);

            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");

            if (Topic == null)
            {
                return View(db.Question_Bank.Where(X => X.Question_Text.Contains(Search) || Search == null).ToList());
            }
            else
            {
                return View(db.Question_Bank.Where(X => X.Question_Text.Contains(Search) && X.Topic_Seq == Topic || Search == null).ToList());
            }



        }
        public ActionResult Add_Question_To_New_Topic(int Topic_Seq, string NewTopic, string Topic_Name, string Description)
        {
            ViewBag.NewTopic = NewTopic;
            ViewBag.Topic_Name = Topic_Name;
            ViewBag.Topic_Des = Description;
            ViewBag.Description = null;
            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID");
            return View("Create");
        }


        // GET: Question_Bank/Create
        public ActionResult Create(string Name, int? Topic)
        {
            if (Topic == null)
            {
                ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            }
            else
            {
                ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);
            }

            ViewBag.Question_Text = Name;
            ViewBag.Style_Type_IDName = "None";

            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID");
            return View();
        }

        public ActionResult GetDescription(string Style_Type_ID, string Question_Text, int? Topic_Seq, string NewTopic, string Topic_Name, string Topic_Des)
        {
            ViewBag.NewTopic = NewTopic;
            ViewBag.Topic_Name = Topic_Name;
            ViewBag.Topic_Des = Topic_Des;
            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID");
            ViewBag.Description = db.Style_Type.Where(X => X.Style_Type_ID == Style_Type_ID).Select(Y => Y.Description).Single();
            ViewBag.Image = db.Style_Type.Where(X => X.Style_Type_ID == Style_Type_ID).Select(Y => Y.Preview).Single();
            ViewBag.Question_Text = Question_Text;
            ViewBag.Style_Type_IDName = Style_Type_ID;
            return View("Create");
        }

        public ActionResult Possible_Answer(string Style_Type_ID, int Question_Seq, string Question_Text, int Topic_Seq, string Topic_Name, string Topic_Des, string NewTopic)
        {
            ViewBag.Question_Text = Question_Text;
            ViewBag.Style_Type_ID = Style_Type_ID;
            ViewBag.Question_Seq = Question_Seq;
            ViewBag.Topic_Seq = Topic_Seq;
            ViewBag.Topic_Name = Topic_Name;
            ViewBag.Topic_Des = Topic_Des;
            ViewBag.NewTopic = NewTopic;
            ViewBag.EditNewPossibleAnswer = "No";
            //db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq).ToList();
            return View();
        }

        public ActionResult New_Possible_Answer_From_Edit(string Style_Type_ID, int Question_Seq, string Question_Text, int Topic_Seq)
        {
            ViewBag.Question_Text = Question_Text;
            ViewBag.Style_Type_ID = Style_Type_ID;
            ViewBag.Question_Seq = Question_Seq;
            ViewBag.Topic_Seq = Topic_Seq;
            ViewBag.EditNewPossibleAnswer = "Yes";
            return View("Possible_Answer");
        }


        public ActionResult Possible_Answer_Back(string Style_Type_ID, int Question_Seq, string Question_Text, int Topic_Seq, string Topic_Name, string Topic_Des)
        {

            int Count = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).Count();

            if (Count <= 1)
            {
                ViewBag.NewTopic = "YES";                
                string TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Seq).Select(Y => Y.Topic_Name).Single();
                ViewBag.TopicName = TopicName;

            }
            Question_Bank question_Bank = db.Question_Bank.Find(Question_Seq);
            db.Question_Bank.Remove(question_Bank);
            db.SaveChanges();

            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID", Style_Type_ID);

            ViewBag.Description = db.Style_Type.Where(X => X.Style_Type_ID == Style_Type_ID).Select(Y => Y.Description).Single();
            ViewBag.Image = db.Style_Type.Where(X => X.Style_Type_ID == Style_Type_ID).Select(Y => Y.Preview).Single();

            ViewBag.Topic_Name = Topic_Name;
            ViewBag.Topic_Des = Topic_Des;



            ViewBag.Question_Text = Question_Text;
            ViewBag.Question_Seq = Question_Seq;

            return View("Create");
        }

        public ActionResult Possible_Answer_Create_2(int Question_Seq, int Display_Order1, int Display_Order2, string Answer_Text1, string Answer_Text2, string NewTopic, string EditNewPossibleAnswer)
        {
            Possible_Answer Answer1 = new Models.Possible_Answer();
            Answer1.Question_Seq = Question_Seq;
            Answer1.Display_Order = Display_Order1;
            Answer1.Answer_Text = Answer_Text1;
            db.Possible_Answer.Add(Answer1);

            Possible_Answer Answer2 = new Models.Possible_Answer();
            Answer2.Question_Seq = Question_Seq;
            Answer2.Display_Order = Display_Order2;
            Answer2.Answer_Text = Answer_Text2;
            db.Possible_Answer.Add(Answer2);

            db.SaveChanges();

            int Topic_Name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Topic_Seq).Single();
            ViewBag.NewTopic = NewTopic;

            if (EditNewPossibleAnswer == "Yes")
            {
                ViewBag.EditComplete = "Yes";
                string QuestionName = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                ViewBag.EditCompleteMessage = "Question '" + QuestionName + "' successfully edited";
                // -------------------------------Action Log ----------------------------------------//
                string name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                psal.Action_DateTime = DateTime.Now;
                psal.Action_ID = 11;
                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                //psal.Ac = "Delete";
                psal.Crud_Operation = "Edit";
                psal.Action_Performed = "Question: " + name;
                db.Person_Session_Action_Log.Add(psal);
                db.SaveChanges();
                // -------------------------------Action Log ----------------------------------------//
            }
            else
            {
                
                ViewBag.CreateComplete = "Yes";
                string QuestionName = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                ViewBag.CreateCompleteMessage = "Question '" + QuestionName + "' successfully created";
               
                string TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Name).Select(Y => Y.Topic_Name).Single();
                ViewBag.TopicName = TopicName;
                // -------------------------------Action Log ----------------------------------------//
                string name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                psal.Action_DateTime = DateTime.Now;
                psal.Action_ID = 11;
                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                //psal.Ac = "Delete";
                psal.Crud_Operation = "Create";
                psal.Action_Performed = "Question: " + name;
                db.Person_Session_Action_Log.Add(psal);
                db.SaveChanges();
                // -------------------------------Action Log ----------------------------------------//

            }
            if (ViewBag.NewTopic == "YES")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Name);
                return View("Index", db.Question_Bank.Where(X => X.Topic_Seq == Topic_Name).ToList());
            }
            else
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                return View("Index", db.Question_Bank.ToList());
            }            
        }
        public ActionResult Possible_Answer_Create_5(int Question_Seq, int Display_Order1, int Display_Order2, int Display_Order3, int Display_Order4, int Display_Order5, string Answer_Text1, string Answer_Text2, string Answer_Text3, string Answer_Text4, string Answer_Text5, string NewTopic, string EditNewPossibleAnswer)
        {
            Possible_Answer Answer1 = new Models.Possible_Answer();
            Answer1.Question_Seq = Question_Seq;
            Answer1.Display_Order = Display_Order1;
            Answer1.Answer_Text = Answer_Text1;
            db.Possible_Answer.Add(Answer1);

            Possible_Answer Answer2 = new Models.Possible_Answer();
            Answer2.Question_Seq = Question_Seq;
            Answer2.Display_Order = Display_Order2;
            Answer2.Answer_Text = Answer_Text2;
            db.Possible_Answer.Add(Answer2);

            if (Answer_Text3 != "")
            {
                Possible_Answer Answer3 = new Models.Possible_Answer();
                Answer3.Question_Seq = Question_Seq;
                Answer3.Display_Order = Display_Order3;
                Answer3.Answer_Text = Answer_Text3;
                db.Possible_Answer.Add(Answer3);
            }

            if (Answer_Text4 != "")
            {
                Possible_Answer Answer4 = new Models.Possible_Answer();
                Answer4.Question_Seq = Question_Seq;
                Answer4.Display_Order = Display_Order4;
                Answer4.Answer_Text = Answer_Text4;
                db.Possible_Answer.Add(Answer4);
            }

            if (Answer_Text5 != "")
            {
                Possible_Answer Answer5 = new Models.Possible_Answer();
                Answer5.Question_Seq = Question_Seq;
                Answer5.Display_Order = Display_Order5;
                Answer5.Answer_Text = Answer_Text5;
                db.Possible_Answer.Add(Answer5);
            }

            db.SaveChanges();
            int Topic_Name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Topic_Seq).Single();
            
            ViewBag.NewTopic = NewTopic;
            if (EditNewPossibleAnswer == "Yes")
            {
                ViewBag.EditComplete = "Yes";
                string QuestionName = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                @ViewBag.EditCompleteMessage = "Question '" + QuestionName + "' successfully edited";
                // -------------------------------Action Log ----------------------------------------//
                string name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                psal.Action_DateTime = DateTime.Now;
                psal.Action_ID = 11;
                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                //psal.Ac = "Delete";
                psal.Crud_Operation = "Edit";
                psal.Action_Performed = "Question: " + name;
                db.Person_Session_Action_Log.Add(psal);
                db.SaveChanges();
                // -------------------------------Action Log ----------------------------------------//
            }
            else
            {
                ViewBag.CreateComplete = "Yes";
                string QuestionName = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                ViewBag.CreateCompleteMessage = "Question '" + QuestionName + "' successfully created";
        
                string TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Name).Select(Y => Y.Topic_Name).Single();
                ViewBag.TopicName = TopicName;
                // -------------------------------Action Log ----------------------------------------//
                string name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                psal.Action_DateTime = DateTime.Now;
                psal.Action_ID = 11;
                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                //psal.Ac = "Delete";
                psal.Crud_Operation = "Create";
                psal.Action_Performed = "Question: " + name;
                db.Person_Session_Action_Log.Add(psal);
                db.SaveChanges();
                // -------------------------------Action Log ----------------------------------------//

            }
            if (ViewBag.NewTopic == "YES")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Name);
                return View("Index", db.Question_Bank.Where(X => X.Topic_Seq == Topic_Name).ToList());
            }
            else
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                return View("Index", db.Question_Bank.ToList());
            }
        }


        public ActionResult Possible_Answer_Edit_2(int Question_Seq, int Display_Order1, int Display_Order2, string Answer_Text1, string Answer_Text2, string NewTopic)
        {
            db.Possible_Answer.RemoveRange(db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq).ToList());
            db.SaveChanges();

            Possible_Answer Answer1 = new Models.Possible_Answer();
            Answer1.Question_Seq = Question_Seq;
            Answer1.Display_Order = Display_Order1;
            Answer1.Answer_Text = Answer_Text1;
            db.Possible_Answer.Add(Answer1);

            Possible_Answer Answer2 = new Models.Possible_Answer();
            Answer2.Question_Seq = Question_Seq;
            Answer2.Display_Order = Display_Order2;
            Answer2.Answer_Text = Answer_Text2;
            db.Possible_Answer.Add(Answer2);

            db.SaveChanges();

            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            ViewBag.NewTopic = NewTopic;
            ViewBag.EditComplete = "Yes";
            string QuestionName = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
            @ViewBag.EditCompleteMessage = "Question '" + QuestionName + "' successfully edited";

            // -------------------------------Action Log ----------------------------------------//
            string name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
            Person_Session_Action_Log psal = new Person_Session_Action_Log();
            psal.Action_DateTime = DateTime.Now;
            psal.Action_ID = 11;
            psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
            //psal.Ac = "Delete";
            psal.Crud_Operation = "Edit";
            psal.Action_Performed = "Question: " + name;
            db.Person_Session_Action_Log.Add(psal);
            db.SaveChanges();
            // -------------------------------Action Log ----------------------------------------//

            return View("Index", db.Question_Bank.ToList());
        }
        public ActionResult Possible_Answer_Edit_5(int Question_Seq, int Display_Order1, int Display_Order2, int Display_Order3, int Display_Order4, int Display_Order5, string Answer_Text1, string Answer_Text2, string Answer_Text3, string Answer_Text4, string Answer_Text5, string NewTopic)
        {

            db.Possible_Answer.RemoveRange(db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq).ToList());
            db.SaveChanges();

            Possible_Answer Answer1 = new Models.Possible_Answer();
            Answer1.Question_Seq = Question_Seq;
            Answer1.Display_Order = Display_Order1;
            Answer1.Answer_Text = Answer_Text1;
            db.Possible_Answer.Add(Answer1);

            Possible_Answer Answer2 = new Models.Possible_Answer();
            Answer2.Question_Seq = Question_Seq;
            Answer2.Display_Order = Display_Order2;
            Answer2.Answer_Text = Answer_Text2;
            db.Possible_Answer.Add(Answer2);

            if (Answer_Text3 != "")
            {
                Possible_Answer Answer3 = new Models.Possible_Answer();
                Answer3.Question_Seq = Question_Seq;
                Answer3.Display_Order = Display_Order3;
                Answer3.Answer_Text = Answer_Text3;
                db.Possible_Answer.Add(Answer3);
            }

            if (Answer_Text4 != "")
            {
                Possible_Answer Answer4 = new Models.Possible_Answer();
                Answer4.Question_Seq = Question_Seq;
                Answer4.Display_Order = Display_Order4;
                Answer4.Answer_Text = Answer_Text4;
                db.Possible_Answer.Add(Answer4);
            }

            if (Answer_Text5 != "")
            {
                Possible_Answer Answer5 = new Models.Possible_Answer();
                Answer5.Question_Seq = Question_Seq;
                Answer5.Display_Order = Display_Order5;
                Answer5.Answer_Text = Answer_Text5;
                db.Possible_Answer.Add(Answer5);
            }

            db.SaveChanges();

            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            ViewBag.NewTopic = NewTopic;
            ViewBag.EditComplete = "Yes";
            string QuestionName = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
            @ViewBag.EditCompleteMessage = "Question '" + QuestionName + "' successfully edited";

            // -------------------------------Action Log ----------------------------------------//
            string name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
            Person_Session_Action_Log psal = new Person_Session_Action_Log();
            psal.Action_DateTime = DateTime.Now;
            psal.Action_ID = 11;
            psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
            //psal.Ac = "Delete";
            psal.Crud_Operation = "Edit";
            psal.Action_Performed = "Question: " + name;
            db.Person_Session_Action_Log.Add(psal);
            db.SaveChanges();
            // -------------------------------Action Log ----------------------------------------//

            return View("Index", db.Question_Bank.ToList());
        }
       
        public ActionResult ViewQuestion (int id)
        {
            ViewBag.Question1_StyleType = db.Question_Bank.Where(X => X.Question_Seq == id).Select(Y => Y.Style_Type_ID).Single();           
            ViewBag.Question1 = db.Question_Bank.Where(X => X.Question_Seq == id).Select(Y => Y.Question_Text).Single();
            int Count1 = db.Possible_Answer.Where(X => X.Question_Seq == id).Count();
            ViewBag.Count1 = Count1;

            if (Count1 == 0)
            {
                //No possible answers
            }
            else if (Count1 == 1)
            {
                ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
            }
            else if (Count1 == 2)
            {
                ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
            }
            else if (Count1 == 3)
            {
                ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
            }
            else if (Count1 == 4)
            {
                ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
            }
            else if (Count1 == 5)
            {
                ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                ViewBag.PossibleAnswer5_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == id && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
            }

            int _count_ = db.Questionnaire_Questions.Where(X => X.Question_Seq == id).Count();
            List<Questionnaire_Questions> qq = db.Questionnaire_Questions.Where(X => X.Question_Seq == id).ToList();
            string[] UsedWhere = new string[_count_];
            if (_count_ > 0)
            {
                ViewBag.HasBeenUsed = "Yes";
                ViewBag.Count = _count_;

                for (int i = 0; i < _count_; i++)
                {
                    int ID_Name = qq[i].Questionnaire_ID;
                    UsedWhere[i] = db.Questionnaires.Where(X => X.Questionnaire_ID == ID_Name).Select(Y => Y.Name).Single();                        
                }
                
            }
            ViewBag.UsedWhere = UsedWhere;
            return View("View");

        }


        public ActionResult Creating(string Question_Text, int Topic_Seq, string Style_Type_ID, string Topic_Name, string Topic_Des, string NewTopic)
        {
            Question_Bank question_Bank = new Models.Question_Bank();
            question_Bank.Question_Text = Question_Text;
            question_Bank.Topic_Seq = Topic_Seq;
            question_Bank.Style_Type_ID = Style_Type_ID;

            if (AreThereDuplicates(Question_Text, Topic_Seq))
            {
                ViewBag.Duplicate = "Yes";
                ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
                ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID", Style_Type_ID);
                ViewBag.Question_Text = Question_Text;
                ViewBag.Description = db.Style_Type.Where(X => X.Style_Type_ID == Style_Type_ID).Select(Y => Y.Description).Single();
                ViewBag.Image = db.Style_Type.Where(X => X.Style_Type_ID == Style_Type_ID).Select(Y => Y.Preview).Single();
                ViewBag.Topic_Name = Topic_Name;
                ViewBag.Topic_Des = Topic_Des;
                ViewBag.NewTopic = NewTopic;

                return View("Create",question_Bank);
            }
            else
            {
                db.Question_Bank.Add(question_Bank);
                db.SaveChanges();

                int Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question_Text && X.Topic_Seq == Topic_Seq).Select(Y => Y.Question_Seq).Single();

                if (Style_Type_ID == "Free Text")
                {
                    
                    ViewBag.NewTopic = NewTopic;
                    ViewBag.CreateComplete = "Yes";                    
                    
                    ViewBag.TopicName = Topic_Name;
                    string QuestionName = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                    ViewBag.CreateCompleteMessage = "Question '" + QuestionName + "' successfully created";
                    // -------------------------------Action Log ----------------------------------------//
                    string name = db.Question_Bank.Where(X => X.Question_Seq == Question_Seq).Select(Y => Y.Question_Text).Single();
                    Person_Session_Action_Log psal = new Person_Session_Action_Log();
                    psal.Action_DateTime = DateTime.Now;
                    psal.Action_ID = 11;
                    psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                    //psal.Ac = "Delete";
                    psal.Crud_Operation = "Create";
                    psal.Action_Performed = "Question: " + name;
                    db.Person_Session_Action_Log.Add(psal);
                    db.SaveChanges();
                    // -------------------------------Action Log ----------------------------------------//

                    if (ViewBag.NewTopic == "YES")
                    {
                        ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
                        return View("Index", db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList());
                    }
                    else
                    {
                        ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                        return View("Index", db.Question_Bank.ToList());
                    }

                    
                }
                else
                {
                    return RedirectToAction("Possible_Answer", new { Style_Type_ID, Question_Seq, Question_Text, Topic_Seq, Topic_Name, Topic_Des, NewTopic });
                }
            }
        }

        public bool AreThereDuplicates(string QuestionText, int Topic_Seq)
        {
            if (db.Question_Bank.Any(o => o.Question_Text == QuestionText && o.Topic_Seq == Topic_Seq))
            {
                return true;
            }
            else
            {
                return false;
            }
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

            int _count_ = db.Questionnaire_Questions.Where(X => X.Question_Seq == id).Count();
            if (_count_ > 0)
            {
                ViewBag.CannotEdit = "Yes";
                ViewBag.CannotEditMessage = "Question: " + question_Bank.Question_Text + " cannot be edited as it has been assigned to a questionnaire. Click on 'View' for more details.";
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                return View("Index", db.Question_Bank.ToList());
            }
            else
            {
                ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", question_Bank.Topic_Seq);
                ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID", question_Bank.Style_Type_ID);
                return View(question_Bank);
            }
        }        
    
        public ActionResult Come_Back()
        {
            return View();
        }

        public ActionResult Edit_Possible_Answer(int Question_Seq, string Question_Text,string Style_Type_ID)
        {
            ViewBag.Question_Text = Question_Text;
            ViewBag.Style_Type_ID = Style_Type_ID;
            ViewBag.Question_Seq = Question_Seq;
            ViewBag.id = Question_Seq;

            int Count = 0;
             Count = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq).Count();

            if (Count == 5)
            {
                ViewBag.Answer_Text1 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text2 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text3 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text4 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text5 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
            }
            if (Count == 4)
            {
                ViewBag.Answer_Text1 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text2 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text3 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text4 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();                
            }
            if (Count == 3)
            {
                ViewBag.Answer_Text1 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text2 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text3 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
            if (Count == 2)
            {
                ViewBag.Answer_Text1 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                ViewBag.Answer_Text2 = db.Possible_Answer.Where(X => X.Question_Seq == Question_Seq && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();                
            }

            return View("Edit_Possible_Answer");
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
                    if (AreThereDuplicates(question_Bank.Question_Text, question_Bank.Topic_Seq))
                    {
                        //If its the same question then it should be fine
                        if (db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Question_Text).Single() == question_Bank.Question_Text)
                        {
                            //Did you change the style type
                            //No
                            if (db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Style_Type_ID).Single() == question_Bank.Style_Type_ID)
                            {
                                if (question_Bank.Style_Type_ID == "Free Text")
                                {
                                    //Question_Bank Existing = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Single();
                                    //Existing.Question_Text = question_Bank.Question_Text;
                                    //Existing.Style_Type_ID = question_Bank.Style_Type_ID;
                                    //Existing.Topic_Seq = question_Bank.Topic_Seq;

                                    //db.Question_Bank.Add(Existing);                                                              
                                    //db.SaveChanges();

                                    db.Entry(question_Bank).State = EntityState.Modified;
                                    db.SaveChanges();

                                    ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                                    ViewBag.EditComplete = "Yes";

                                    @ViewBag.EditCompleteMessage = "Question '" + question_Bank.Question_Text + "' successfully edited";

                                    // -------------------------------Action Log ----------------------------------------//
                                    string name = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Question_Text).Single();
                                    Person_Session_Action_Log psal = new Person_Session_Action_Log();
                                    psal.Action_DateTime = DateTime.Now;
                                    psal.Action_ID = 11;
                                    psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                                //psal.Ac = "Delete";
                                psal.Crud_Operation = "Edit";
                                psal.Action_Performed = "Question: " + name;
                                    db.Person_Session_Action_Log.Add(psal);
                                    db.SaveChanges();
                                    // -------------------------------Action Log ----------------------------------------//


                                    return View("Index", db.Question_Bank.ToList());
                                }
                                else
                                {
                                    //Question_Bank Existing = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Single();
                                    //Existing.Question_Text = question_Bank.Question_Text;
                                    //Existing.Style_Type_ID = question_Bank.Style_Type_ID;
                                    //Existing.Topic_Seq = question_Bank.Topic_Seq;
                                    //db.Question_Bank.Add(Existing);
                                    //db.SaveChanges();

                                    db.Entry(question_Bank).State = EntityState.Modified;
                                    db.SaveChanges();

                                    return RedirectToAction("Edit_Possible_Answer", new { question_Bank.Question_Seq, question_Bank.Question_Text, question_Bank.Style_Type_ID });
                                }
                            }
                            //Yes
                            else
                            {
                                //Remove previously saved possible answers
                                db.Possible_Answer.RemoveRange(db.Possible_Answer.Where(X => X.Question_Seq == question_Bank.Question_Seq).ToList());

                                if (question_Bank.Style_Type_ID == "Free Text")
                                {
                                    //Question_Bank Existing = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Single();
                                    //Existing.Question_Text = question_Bank.Question_Text;
                                    //Existing.Style_Type_ID = question_Bank.Style_Type_ID;
                                    //Existing.Topic_Seq = question_Bank.Topic_Seq;
                                    //db.Question_Bank.Add(Existing);
                                    //db.SaveChanges();

                                    db.Entry(question_Bank).State = EntityState.Modified;
                                    db.SaveChanges();

                                    ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                                    ViewBag.EditComplete = "Yes";
                                    @ViewBag.EditCompleteMessage = "Question '" + question_Bank.Question_Text + "' successfully edited";

                                    // -------------------------------Action Log ----------------------------------------//
                                    string name = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Question_Text).Single();
                                    Person_Session_Action_Log psal = new Person_Session_Action_Log();
                                    psal.Action_DateTime = DateTime.Now;
                                    psal.Action_ID = 11;
                                    psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                                //psal.Ac = "Delete";
                                psal.Crud_Operation = "Edit";
                                psal.Action_Performed = "Question: " + name;
                                    db.Person_Session_Action_Log.Add(psal);
                                    db.SaveChanges();
                                    // -------------------------------Action Log ----------------------------------------//



                                    return View("Index", db.Question_Bank.ToList());
                                }
                                else
                                {
                                    //Question_Bank Existing = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Single();
                                    //Existing.Question_Text = question_Bank.Question_Text;
                                    //Existing.Style_Type_ID = question_Bank.Style_Type_ID;
                                    //Existing.Topic_Seq = question_Bank.Topic_Seq;
                                    //db.Question_Bank.Add(Existing);
                                    //db.SaveChanges();

                                    db.Entry(question_Bank).State = EntityState.Modified;
                                    db.SaveChanges();

                                    return RedirectToAction("New_Possible_Answer_From_Edit", new { question_Bank.Style_Type_ID, question_Bank.Question_Seq, question_Bank.Question_Text, question_Bank.Topic_Seq });
                                }
                            }
                        }
                        else
                        {
                            ModelState.AddModelError("Question_Text", "This question already exists for the selected topic. Please choose another topic or question.");
                            ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", question_Bank.Topic_Seq);
                            ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID", question_Bank.Style_Type_ID);
                            return View(question_Bank);
                        }
                    }
                    else
                    {
                        //Did you change the style type
                        //No
                        if (db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Style_Type_ID).Single() == question_Bank.Style_Type_ID)
                        {
                            if (question_Bank.Style_Type_ID == "Free Text")
                            {
                                db.Entry(question_Bank).State = EntityState.Modified;
                                db.SaveChanges();
                                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                                ViewBag.EditComplete = "Yes";
                                @ViewBag.EditCompleteMessage = "Question '" + question_Bank.Question_Text + "' successfully edited";

                                // -------------------------------Action Log ----------------------------------------//
                                string name = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Question_Text).Single();
                                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                                psal.Action_DateTime = DateTime.Now;
                                psal.Action_ID = 11;
                                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                            //psal.Ac = "Delete";
                            psal.Crud_Operation = "Edit";
                            psal.Action_Performed = "Question: " + name;
                                db.Person_Session_Action_Log.Add(psal);
                                db.SaveChanges();
                                // -------------------------------Action Log ----------------------------------------//



                                return View("Index", db.Question_Bank.ToList());
                            }
                            else
                            {
                                db.Entry(question_Bank).State = EntityState.Modified;
                                db.SaveChanges();
                                return RedirectToAction("Edit_Possible_Answer", new { question_Bank.Question_Seq, question_Bank.Question_Text, question_Bank.Style_Type_ID });
                            }

                        }
                        //Yes
                        else
                        {
                            //Remove previously saved possible answers
                            db.Possible_Answer.RemoveRange(db.Possible_Answer.Where(X => X.Question_Seq == question_Bank.Question_Seq).ToList());

                            if (question_Bank.Style_Type_ID == "Free Text")
                            {
                                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                                ViewBag.EditComplete = "Yes";
                                @ViewBag.EditCompleteMessage = "Question '" + question_Bank.Question_Text + "' successfully edited";

                                // -------------------------------Action Log ----------------------------------------//
                                string name = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Question_Text).Single();
                                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                                psal.Action_DateTime = DateTime.Now;
                                psal.Action_ID = 11;
                                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                            //psal.Ac = "Delete";
                            psal.Crud_Operation = "Edit";
                            psal.Action_Performed = "Question: " + name;
                                db.Person_Session_Action_Log.Add(psal);
                                db.SaveChanges();
                                // -------------------------------Action Log ----------------------------------------//


                                return View("Index", db.Question_Bank.ToList());
                            }
                            else
                            {
                                return RedirectToAction("New_Possible_Answer_From_Edit", new { question_Bank.Style_Type_ID, question_Bank.Question_Seq, question_Bank.Question_Text, question_Bank.Topic_Seq });
                            }
                        }


                    }
                }
                ViewBag.Topic_Seq = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", question_Bank.Topic_Seq);
                ViewBag.Style_Type_ID = new SelectList(db.Style_Type, "Style_Type_ID", "Style_Type_ID", question_Bank.Style_Type_ID);
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
            
            int _Count = 0;

            _Count = db.Questionnaire_Questions.Where(X => X.Question_Seq == id).Count();

            int Count = db.Question_Bank.Where(X => X.Topic_Seq == question_Bank.Topic_Seq).Count();
            if (_Count != 0)
            {
                ViewBag.Error = "Questions assigned to questionnaires cannot be deleted.";
            }
            if (Count == 1)
            {
                ViewBag.OnlyQinT = "This question is the only question assigned to its topic. Both the question and its topic will be removed upon deletion";
            }


            return View(question_Bank);
    }

        public ActionResult DeleteQ (int id)
        {
            Question_Bank question_Bank = db.Question_Bank.Find(id);
            int Count = db.Question_Bank.Where(X => X.Topic_Seq == question_Bank.Topic_Seq).Count();

            // -------------------------------Action Log ----------------------------------------//
            string name = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Question_Text).Single();
            Person_Session_Action_Log psal = new Person_Session_Action_Log();
            psal.Action_DateTime = DateTime.Now;
            psal.Action_ID = 11;
            psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
            //psal.Ac = "Delete";
            psal.Crud_Operation = "Delete";
            psal.Action_Performed = "Question: " + name;
            db.Person_Session_Action_Log.Add(psal);
            db.SaveChanges();
            // -------------------------------Action Log ----------------------------------------//



            if (Count <= 1)
            {
                Question_Topic question_Topic = db.Question_Topic.Where(X => X.Topic_Seq == question_Bank.Topic_Seq).Single();
                db.Question_Bank.Remove(question_Bank);
                db.Question_Topic.Remove(question_Topic);
            }
            else
            {
                db.Question_Bank.Remove(question_Bank);
            }

            db.SaveChanges();
            ViewBag.DeleteComplete = "Yes";
            @ViewBag.DeleteCompleteMessage = "Question '" + question_Bank.Question_Text + "' deleted to contain integrity";
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");

            return View("Index", db.Question_Bank.ToList());

        }

    // POST: Question_Bank/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public ActionResult DeleteConfirmed(int id)
    {
        Question_Bank question_Bank = db.Question_Bank.Find(id);
            int _Count = 0;

            _Count = db.Questionnaire_Questions.Where(X => X.Question_Seq == id).Count();

            if (_Count != 0)
            {
                ViewBag.Error = "Questions assigned to questionnaires cannot be deleted.";
                Question_Bank q_Bank = db.Question_Bank.Find(id);
                return View("Delete",q_Bank);
            }
            else
            {
                int Count = db.Question_Bank.Where(X => X.Topic_Seq == question_Bank.Topic_Seq).Count();

                // -------------------------------Action Log ----------------------------------------//
                string name = db.Question_Bank.Where(X => X.Question_Seq == question_Bank.Question_Seq).Select(Y => Y.Question_Text).Single();
                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                psal.Action_DateTime = DateTime.Now;
                psal.Action_ID = 11;
                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                psal.Crud_Operation = "Delete";
                //psal.Ac = "Delete";
                psal.Action_Performed = "Question: " + name;
                db.Person_Session_Action_Log.Add(psal);
                db.SaveChanges();
                // -------------------------------Action Log ----------------------------------------//



                if (Count <= 1)
                {
                    Question_Topic question_Topic = db.Question_Topic.Where(X => X.Topic_Seq == question_Bank.Topic_Seq).Single();
                    db.Question_Bank.Remove(question_Bank);
                    db.Question_Topic.Remove(question_Topic);
                }
                else
                {
                    db.Question_Bank.Remove(question_Bank);
                }

                db.SaveChanges();
                ViewBag.DeleteComplete = "Yes";
                @ViewBag.DeleteCompleteMessage = "Question '" + question_Bank.Question_Text + "' successfully deleted";
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                
                return View("Index",db.Question_Bank.ToList());                
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
