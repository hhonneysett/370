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
    
    public class QuestionnaireController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();

        // GET: Questionnaire
        [Authorize(Roles = "Admin")]
        public ActionResult Index()
        {
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);
            return View(questionnaires.ToList());
        }
        [Authorize]
        public ActionResult Respond_to_questionnaire()
        {
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            var questionnaires = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now).ToList();
            return View(questionnaires.ToList());
        }

        //public ActionResult Answer_Questionnaire()
        //{
        //    ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
        //    var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);
        //    return View(questionnaires.ToList());
        //}
        [Authorize]
        public ActionResult Respond_to_questionnaire_Search(string Search, int? Topic, string Assessment_Type)
        {
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Search = Search;            

            if (Topic == null && Assessment_Type == "" )
            {
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type == "")
            {
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else if (Topic == null && Assessment_Type != "")
            {
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type != "")
            {
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else
            {
                var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);
                return View("Index", questionnaires.ToList());
            }
        }        
        public ActionResult Save_Questionnaire_Responses(int Questionnaire_ID, string Question1, string Question2, string Question3, string Question4, string Question5, string Question1_Reply, string Question2_Reply, string Question3_Reply, string Question4_Reply, string Question5_Reply)
        {
            Person_Questionnaire person_Questionnaire = new Person_Questionnaire();
            person_Questionnaire.Questionnaire_ID = Questionnaire_ID;
            person_Questionnaire.Person_ID = db.Registered_Person.Where(X => X.Person_Name == User.Identity.Name).Select(Y => Y.Person_ID).Single();
            person_Questionnaire.Answer_Date = DateTime.Now;
            person_Questionnaire.Status = null;
            db.Person_Questionnaire.Add(person_Questionnaire);
            db.SaveChanges();

            if (Question1 != "")
            {
                Person_Questionnaire_Result person_Questionnaire_Result1 = new Person_Questionnaire_Result();
                person_Questionnaire_Result1.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result1.Person_ID = db.Registered_Person.Where(X => X.Person_Name == User.Identity.Name).Select(Y => Y.Person_ID).Single();
                person_Questionnaire_Result1.Question_Answer = Question1_Reply;
                person_Questionnaire_Result1.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question1).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result1);
                db.SaveChanges();
            }
            if (Question2 != "")
            {
                Person_Questionnaire_Result person_Questionnaire_Result2 = new Person_Questionnaire_Result();
                person_Questionnaire_Result2.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result2.Person_ID = db.Registered_Person.Where(X => X.Person_Name == User.Identity.Name).Select(Y => Y.Person_ID).Single();
                person_Questionnaire_Result2.Question_Answer = Question2_Reply;
                person_Questionnaire_Result2.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question2).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result2);
                db.SaveChanges();
            }
            if (Question3 != "")
            {
                Person_Questionnaire_Result person_Questionnaire_Result3 = new Person_Questionnaire_Result();
                person_Questionnaire_Result3.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result3.Person_ID = db.Registered_Person.Where(X => X.Person_Name == User.Identity.Name).Select(Y => Y.Person_ID).Single();
                person_Questionnaire_Result3.Question_Answer = Question3_Reply;
                person_Questionnaire_Result3.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question3).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result3);
                db.SaveChanges();
            }
            if (Question4 != "")
            {
                Person_Questionnaire_Result person_Questionnaire_Result4 = new Person_Questionnaire_Result();
                person_Questionnaire_Result4.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result4.Person_ID = db.Registered_Person.Where(X => X.Person_Name == User.Identity.Name).Select(Y => Y.Person_ID).Single();
                person_Questionnaire_Result4.Question_Answer = Question4_Reply;
                person_Questionnaire_Result4.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question4).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result4);
                db.SaveChanges();
            }
            if (Question5 != "")
            {
                Person_Questionnaire_Result person_Questionnaire_Result5 = new Person_Questionnaire_Result();
                person_Questionnaire_Result5.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result5.Person_ID = db.Registered_Person.Where(X => X.Person_Name == User.Identity.Name).Select(Y => Y.Person_ID).Single();
                person_Questionnaire_Result5.Question_Answer = Question5_Reply;
                person_Questionnaire_Result5.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question5).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result5);
                db.SaveChanges();
            }

            return View("Home");
        }


        public ActionResult Search(string Search, int? Topic, string Assessment_Type, string CheckBox)
        {
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Search = Search;
            ViewBag.CheckBox = CheckBox;

            if (Topic == null && Assessment_Type == "" && CheckBox == "true")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type == "" && CheckBox == "true")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic || Search == null).ToList());
            }
            else if (Topic == null && Assessment_Type != "" && CheckBox == "true")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Assessment_Type == Assessment_Type || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type != "" && CheckBox == "true")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type || Search == null).ToList());
            }


            else if (Topic == null && Assessment_Type == "" && CheckBox == "false")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type == "" && CheckBox == "false")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else if (Topic == null && Assessment_Type != "" && CheckBox == "false")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type != "" && CheckBox == "false")
            {
                return View("Index", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now || Search == null).ToList());
            }
            else
            {
                var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);
                return View("Index", questionnaires.ToList());
            }
        }
        [Authorize(Roles = "Admin")]
        // GET: Questionnaire/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            if (questionnaire.Active_From <= DateTime.Now && DateTime.Now <= questionnaire.Active_To)
            {
                ViewBag.Status = "Active";
            }
            else if (DateTime.Now < questionnaire.Active_From)
            {
                ViewBag.Status = "Pending";
            }
            else
            {
                ViewBag.Status = "Expired";
            }
            return View(questionnaire);
        }

        public ActionResult Answering_Questionnaire(int id)
        {
            Questionnaire questionnaire = db.Questionnaires.Where(X => X.Questionnaire_ID == id).Single();
            ViewBag.Questionnaire_Name = questionnaire.Name;
            ViewBag.Questionnaire_ID = questionnaire.Questionnaire_ID;
            ViewBag.Questionnaire_Topic = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();

            int Count_Q_Questions = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID).Count();
            ViewBag.Count_Q_Questions = Count_Q_Questions;

            if (Count_Q_Questions > 0)
            {
                int _question_Seq1 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question1 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq1).Select(Y => Y.Question_Text).Single();
                string _StyleType1 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq1).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question1_StyleType = _StyleType1;
                ViewBag.AsnwerOptional_Q1 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1).Count();
                ViewBag.Count1 = Count1;

                if (Count1 == 0)
                {
                    //No possible answers
                }
                else if (Count1 == 1)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 2)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 3)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 4)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 5)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 1)
            {
                int _question_Seq2 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question2 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq2).Select(Y => Y.Question_Text).Single();
                string _StyleType2 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq2).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question2_StyleType = _StyleType2;
                ViewBag.AsnwerOptional_Q2 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2).Count();
                ViewBag.Count2 = Count2;

                if (Count2 == 0)
                {
                    //No possible answers
                }
                else if (Count2 == 1)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 2)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 3)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 4)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 5)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 2)
            {
                int _question_Seq3 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question3 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq3).Select(Y => Y.Question_Text).Single();
                string _StyleType3 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq3).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question3_StyleType = _StyleType3;
                ViewBag.AsnwerOptional_Q3 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3).Count();
                ViewBag.Count3 = Count3;

                if (Count3 == 0)
                {
                    //No possible answers
                }
                else if (Count3 == 1)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 2)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 3)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 4)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 5)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 3)
            {
                int _question_Seq4 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question4 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq4).Select(Y => Y.Question_Text).Single();
                string _StyleType4 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq4).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question4_StyleType = _StyleType4;
                ViewBag.AsnwerOptional_Q4 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4).Count();
                ViewBag.Count4 = Count4;

                if (Count4 == 0)
                {
                    //No possible answers
                }
                else if (Count4 == 1)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 2)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 3)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 4)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 5)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 4)
            {
                int _question_Seq5 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question5 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq5).Select(Y => Y.Question_Text).Single();
                string _StyleType5 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq5).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question5_StyleType = _StyleType5;
                ViewBag.AsnwerOptional_Q5 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5).Count();
                ViewBag.Count5 = Count5;

                if (Count5 == 0)
                {
                    //No possible answers
                }
                else if (Count5 == 1)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 2)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 3)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 4)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 5)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            return View("Answer_Questionnaire");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Preview(string Name)
        {
            Questionnaire questionnaire = db.Questionnaires.Where(X => X.Name == Name).Single();
            ViewBag.Questionnaire_Name = questionnaire.Name;
            ViewBag.Questionnaire_ID = questionnaire.Questionnaire_ID;
            ViewBag.Questionnaire_Topic = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();

            int Count_Q_Questions = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID).Count();
            ViewBag.Count_Q_Questions = Count_Q_Questions;

            if (Count_Q_Questions > 0)
            {
                int _question_Seq1 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question1 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq1).Select(Y => Y.Question_Text).Single();
                string _StyleType1 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq1).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question1_StyleType = _StyleType1;
                ViewBag.AsnwerOptional_Q1 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1).Count();
                ViewBag.Count1 = Count1;

                if (Count1 == 0)
                {
                    //No possible answers
                }
                else if (Count1 == 1)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 2)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 3)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 4)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count1 == 5)
                {
                    ViewBag.PossibleAnswer1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 1)
            {
                int _question_Seq2 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question2 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq2).Select(Y => Y.Question_Text).Single();
                string _StyleType2 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq2).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question2_StyleType = _StyleType2;
                ViewBag.AsnwerOptional_Q2 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2).Count();
                ViewBag.Count2 = Count2;

                if (Count2 == 0)
                {
                    //No possible answers
                }
                else if (Count2 == 1)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 2)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 3)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 4)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count2 == 5)
                {
                    ViewBag.PossibleAnswer1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 2)
            {
                int _question_Seq3 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question3 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq3).Select(Y => Y.Question_Text).Single();
                string _StyleType3 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq3).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question3_StyleType = _StyleType3;
                ViewBag.AsnwerOptional_Q3 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3).Count();
                ViewBag.Count3 = Count3;

                if (Count3 == 0)
                {
                    //No possible answers
                }
                else if (Count3 == 1)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 2)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 3)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 4)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count3 == 5)
                {
                    ViewBag.PossibleAnswer1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 3)
            {
                int _question_Seq4 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question4 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq4).Select(Y => Y.Question_Text).Single();
                string _StyleType4 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq4).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question4_StyleType = _StyleType4;
                ViewBag.AsnwerOptional_Q4 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4).Count();
                ViewBag.Count4 = Count4;

                if (Count4 == 0)
                {
                    //No possible answers
                }
                else if (Count4 == 1)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 2)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 3)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 4)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count4 == 5)
                {
                    ViewBag.PossibleAnswer1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            if (Count_Q_Questions > 4)
            {
                int _question_Seq5 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Question_Seq).Single();
                ViewBag.Question5 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq5).Select(Y => Y.Question_Text).Single();
                string _StyleType5 = db.Question_Bank.Where(X => X.Question_Seq == _question_Seq5).Select(Y => Y.Style_Type_ID).Single();
                ViewBag.Question5_StyleType = _StyleType5;
                ViewBag.AsnwerOptional_Q5 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Answer_Optional_Ind).Single();
                int Count5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5).Count();
                ViewBag.Count5 = Count5;

                if (Count5 == 0)
                {
                    //No possible answers
                }
                else if (Count5 == 1)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 2)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 3)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 4)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                }
                else if (Count5 == 5)
                {
                    ViewBag.PossibleAnswer1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                    ViewBag.PossibleAnswer5_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                }
            }

            return View("Preview");
        }

        [Authorize(Roles = "Admin")]
        // GET: Questionnaire/Create
        public ActionResult Create()
        {
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            return View();
        }


        public bool AreThereDuplicates(string Name)
        {
            if (db.Questionnaires.Any(X => X.Name == Name))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public ActionResult Back_From_QQuestions(string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {
            Questionnaire questionnaire = db.Questionnaires.Where(X => X.Name == Name).Single();

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);
            db.Questionnaires.Remove(questionnaire);
            db.SaveChanges();
            return View("Create");
        }
        




            public ActionResult Edit_Questionnaire_Questions(int Display_Order1, int Display_Order2, int Display_Order3, int Display_Order4, int Display_Order5, string SelectedQuestion1, string SelectedQuestion2, string SelectedQuestion3, string SelectedQuestion4, string SelectedQuestion5, string SelectedQ_Checkbox1, string SelectedQ_Checkbox2, string SelectedQ_Checkbox3, string SelectedQ_Checkbox4, string SelectedQ_Checkbox5, int Questionnaire_ID)
        {

            db.Questionnaire_Questions.RemoveRange(db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).ToList());

            Questionnaire_Questions questionnaire_Q1 = new Questionnaire_Questions();
            questionnaire_Q1.Questionnaire_ID = Questionnaire_ID;
            questionnaire_Q1.Display_Order = Display_Order1;
            questionnaire_Q1.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion1).Select(Y => Y.Question_Seq).Single();
            if (SelectedQ_Checkbox1 == "true")
            {
                questionnaire_Q1.Answer_Optional_Ind = 1;
            }
            else
            {
                questionnaire_Q1.Answer_Optional_Ind = 0;
            }
            db.Questionnaire_Questions.Add(questionnaire_Q1);

            if (SelectedQuestion2 != "")
            {
                Questionnaire_Questions questionnaire_Q2 = new Questionnaire_Questions();
                questionnaire_Q2.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q2.Display_Order = Display_Order2;
                questionnaire_Q2.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion2).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox2 == "true")
                {
                    questionnaire_Q2.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q2.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q2);

            }

            if (SelectedQuestion3 != "")
            {
                Questionnaire_Questions questionnaire_Q3 = new Questionnaire_Questions();
                questionnaire_Q3.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q3.Display_Order = Display_Order3;
                questionnaire_Q3.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion3).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox3 == "true")
                {
                    questionnaire_Q3.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q3.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q3);

            }

            if (SelectedQuestion4 != "")
            {
                Questionnaire_Questions questionnaire_Q4 = new Questionnaire_Questions();
                questionnaire_Q4.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q4.Display_Order = Display_Order4;
                questionnaire_Q4.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion4).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox4 == "true")
                {
                    questionnaire_Q4.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q4.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q4);

            }

            if (SelectedQuestion5 != "")
            {
                Questionnaire_Questions questionnaire_Q5 = new Questionnaire_Questions();
                questionnaire_Q5.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q5.Display_Order = Display_Order5;
                questionnaire_Q5.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion5).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox5 == "true")
                {
                    questionnaire_Q5.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q5.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q5);

            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [Authorize(Roles = "Admin")]
        public ActionResult Create_Questionnaire_Questions(int Display_Order1, int Display_Order2, int Display_Order3, int Display_Order4, int Display_Order5, string SelectedQuestion1, string SelectedQuestion2, string SelectedQuestion3, string SelectedQuestion4, string SelectedQuestion5, string SelectedQ_Checkbox1, string SelectedQ_Checkbox2, string SelectedQ_Checkbox3, string SelectedQ_Checkbox4, string SelectedQ_Checkbox5, int Questionnaire_ID)
        {

            Questionnaire_Questions questionnaire_Q1 = new Questionnaire_Questions();
            questionnaire_Q1.Questionnaire_ID = Questionnaire_ID;
            questionnaire_Q1.Display_Order = Display_Order1;
            questionnaire_Q1.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion1).Select(Y => Y.Question_Seq).Single();
            if (SelectedQ_Checkbox1 == "true")
            {
                questionnaire_Q1.Answer_Optional_Ind = 1;
            }
            else
            {
                questionnaire_Q1.Answer_Optional_Ind = 0;
            }
            db.Questionnaire_Questions.Add(questionnaire_Q1);

            if (SelectedQuestion2 != "")
            {
                Questionnaire_Questions questionnaire_Q2 = new Questionnaire_Questions();
                questionnaire_Q2.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q2.Display_Order = Display_Order2;
                questionnaire_Q2.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion2).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox2 == "true")
                {
                    questionnaire_Q2.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q2.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q2);

            }

            if (SelectedQuestion3 != "")
            {
                Questionnaire_Questions questionnaire_Q3 = new Questionnaire_Questions();
                questionnaire_Q3.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q3.Display_Order = Display_Order3;
                questionnaire_Q3.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion3).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox3 == "true")
                {
                    questionnaire_Q3.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q3.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q3);

            }

            if (SelectedQuestion4 != "")
            {
                Questionnaire_Questions questionnaire_Q4 = new Questionnaire_Questions();
                questionnaire_Q4.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q4.Display_Order = Display_Order4;
                questionnaire_Q4.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion4).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox4 == "true")
                {
                    questionnaire_Q4.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q4.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q4);

            }

            if (SelectedQuestion5 != "")
            {
                Questionnaire_Questions questionnaire_Q5 = new Questionnaire_Questions();
                questionnaire_Q5.Questionnaire_ID = Questionnaire_ID;
                questionnaire_Q5.Display_Order = Display_Order5;
                questionnaire_Q5.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == SelectedQuestion5).Select(Y => Y.Question_Seq).Single();
                if (SelectedQ_Checkbox5 == "true")
                {
                    questionnaire_Q5.Answer_Optional_Ind = 1;
                }
                else
                {
                    questionnaire_Q5.Answer_Optional_Ind = 0;
                }
                db.Questionnaire_Questions.Add(questionnaire_Q5);

            }

            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Search_QQuestions(string A_New_Name, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic, string SelectedQuestion1, string SelectedQuestion2, string SelectedQuestion3, string SelectedQuestion4, string SelectedQuestion5, string SelectedQ_Checkbox1, string SelectedQ_Checkbox2, string SelectedQ_Checkbox3, string SelectedQ_Checkbox4, string SelectedQ_Checkbox5, int Questionnaire_ID)
        {
            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic && X.Question_Text.Contains(A_New_Name)).ToList();
            int Count = db.Question_Bank.Where(X => X.Topic_Seq == Topic && X.Question_Text.Contains(A_New_Name)).Count();
            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.SelectedQuestion1 = SelectedQuestion1;
            ViewBag.SelectedQuestion2 = SelectedQuestion2;
            ViewBag.SelectedQuestion3 = SelectedQuestion3;
            ViewBag.SelectedQuestion4 = SelectedQuestion4;
            ViewBag.SelectedQuestion5 = SelectedQuestion5;
            ViewBag.Questionnaire_ID = Questionnaire_ID;
            ViewBag.SelectedQ_Checkbox1 = SelectedQ_Checkbox1;
            ViewBag.SelectedQ_Checkbox2 = SelectedQ_Checkbox2;
            ViewBag.SelectedQ_Checkbox3 = SelectedQ_Checkbox3;
            ViewBag.SelectedQ_Checkbox4 = SelectedQ_Checkbox4;
            ViewBag.SelectedQ_Checkbox5 = SelectedQ_Checkbox5;
            ViewBag.Count = Count;
            

            return View("Questionnaire_Questions");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Search_Edit_QQuestions(string A_New_Name, int Topic, string SelectedQuestion1, string SelectedQuestion2, string SelectedQuestion3, string SelectedQuestion4, string SelectedQuestion5, string SelectedQ_Checkbox1, string SelectedQ_Checkbox2, string SelectedQ_Checkbox3, string SelectedQ_Checkbox4, string SelectedQ_Checkbox5, int Questionnaire_ID, string Edit_or_New)
        {
            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic && X.Question_Text.Contains(A_New_Name)).ToList();
            int Count = db.Question_Bank.Where(X => X.Topic_Seq == Topic && X.Question_Text.Contains(A_New_Name)).Count();
            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");

            ViewBag.Topic = Topic;
            ViewBag.SelectedQuestion1 = SelectedQuestion1;
            ViewBag.SelectedQuestion2 = SelectedQuestion2;
            ViewBag.SelectedQuestion3 = SelectedQuestion3;
            ViewBag.SelectedQuestion4 = SelectedQuestion4;
            ViewBag.SelectedQuestion5 = SelectedQuestion5;
            ViewBag.Questionnaire_ID = Questionnaire_ID;
            ViewBag.SelectedQ_Checkbox1 = SelectedQ_Checkbox1;
            ViewBag.SelectedQ_Checkbox2 = SelectedQ_Checkbox2;
            ViewBag.SelectedQ_Checkbox3 = SelectedQ_Checkbox3;
            ViewBag.SelectedQ_Checkbox4 = SelectedQ_Checkbox4;
            ViewBag.SelectedQ_Checkbox5 = SelectedQ_Checkbox5;
            ViewBag.Count = Count;
            ViewBag.Edit_or_New = Edit_or_New;

            return View("Questionnaire_Questions");
        }

        // POST: Questionnaire/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "Admin")]
        public ActionResult Creating(string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {


            if (AreThereDuplicates(Name))
            {
                ViewBag.Name = Name;
                ViewBag.Description = Description;
                ViewBag.Active_From = Active_From;
                ViewBag.Active_To = Active_To;
                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);
                ViewBag.DuplicateError = "A questionnaire with this name already exists. Please choose another name for the questionnaire.";
                return View("Create");
            }
            else
            {
                if (Assessment_Type == "Other")
                {
                    Questionnaire newQuestionnaire = new Questionnaire();
                    newQuestionnaire.Name = Name;
                    newQuestionnaire.Description = Description;
                    newQuestionnaire.Active_From = Active_From;
                    newQuestionnaire.Active_To = Active_To;
                    newQuestionnaire.Assessment_Type = Assessment_Type;
                    newQuestionnaire.Topic_Seq = Topic;
                    newQuestionnaire.Create_Date = DateTime.Now;
                    newQuestionnaire.Person_ID_Creator = User.Identity.Name;
                    newQuestionnaire.Person_ID_Involved = null;
                    newQuestionnaire.Venue_Booking_Seq = null;
                    newQuestionnaire.Venue_ID = null;
                    newQuestionnaire.Building_Floor_ID = null;
                    newQuestionnaire.Building_ID = null;
                    newQuestionnaire.Campus_ID = null;

                    db.Questionnaires.Add(newQuestionnaire);
                    db.SaveChanges();


                    IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic).ToList();

                    ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                    ViewBag.Name = Name;
                    ViewBag.Description = Description;
                    ViewBag.Active_From = Active_From;
                    ViewBag.Active_To = Active_To;
                    ViewBag.Assessment_Type = Assessment_Type;
                    ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                    ViewBag.Topic = Topic;

                    return View("Questionnaire_Questions");
                }
                else if (Assessment_Type == "Venue")
                {
                    return RedirectToAction("Index");
                }
                else if (Assessment_Type == "Employee")
                {
                    return RedirectToAction("Index");
                }
                else if (Assessment_Type == "Training Session")
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        [Authorize(Roles = "Admin")]
        // GET: Questionnaire/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionnaire questionnaire = db.Questionnaires.Where(X => X.Questionnaire_ID == id).Single();
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            ViewBag.Name = questionnaire.Name;
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", questionnaire.Topic_Seq);
            ViewBag.Description = questionnaire.Description;
            ViewBag.Active_From = questionnaire.Active_From;
            ViewBag.Assess = questionnaire.Assessment_Type;
            ViewBag.Active_To = questionnaire.Active_To;
            ViewBag.Questionnaire_ID = questionnaire.Questionnaire_ID;
            return View("Edit", questionnaire);

        }
        [Authorize(Roles = "Admin")]
        // POST: Questionnaire/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.      
        public ActionResult Editing(string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic_Seq, int Questionnaire_ID)
        {
            if (AreThereDuplicates(Name))
            {
                if (db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single() == Name)
                {
                    if (db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Topic_Seq).Single() == Topic_Seq)
                    {
                        //Topic has not been changed
                        //its fine update
                        Questionnaire questionnaire = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Single();
                        questionnaire.Name = Name;
                        questionnaire.Description = Description;
                        questionnaire.Active_From = Active_From;
                        questionnaire.Active_To = Active_To;
                        questionnaire.Assessment_Type = Assessment_Type;
                        questionnaire.Topic_Seq = Topic_Seq;
                        questionnaire.Create_Date = questionnaire.Create_Date;
                        questionnaire.Person_ID_Creator = questionnaire.Person_ID_Creator;
                        questionnaire.Person_ID_Involved = questionnaire.Person_ID_Involved;
                        questionnaire.Venue_Booking_Seq = questionnaire.Venue_Booking_Seq;
                        questionnaire.Venue_ID = questionnaire.Venue_ID;
                        questionnaire.Building_Floor_ID = questionnaire.Building_Floor_ID;
                        questionnaire.Building_ID = questionnaire.Building_ID;
                        questionnaire.Campus_ID = questionnaire.Campus_ID;
                        questionnaire.Questionnaire_ID = questionnaire.Questionnaire_ID;

                        

                        ViewBag.Questionnaire_ID = Questionnaire_ID;
                        ViewBag.Topic = Topic_Seq;
                        int _Count = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).Count();

                        if (_Count >0)
                        {
                            int _Question_Seq1 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Question_Seq).Single();
                            ViewBag.SelectedQuestion1 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq1).Select(Y => Y.Question_Text).Single();

                            if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                            {
                                ViewBag.SelectedQ_Checkbox1 = "false";
                            }
                            else
                            {
                                ViewBag.SelectedQ_Checkbox1 = "true";
                            }   
                        }

                        if (_Count > 1)
                        {
                            int _Question_Seq2 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Question_Seq).Single();
                            ViewBag.SelectedQuestion2 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq2).Select(Y => Y.Question_Text).Single();

                            if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                            {
                                ViewBag.SelectedQ_Checkbox2 = "false";
                            }
                            else
                            {
                                ViewBag.SelectedQ_Checkbox2 = "true";
                            }
                        }

                        if (_Count > 2)
                        {
                            int _Question_Seq3 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Question_Seq).Single();
                            ViewBag.SelectedQuestion3 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq3).Select(Y => Y.Question_Text).Single();

                            if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                            {
                                ViewBag.SelectedQ_Checkbox3 = "false";
                            }
                            else
                            {
                                ViewBag.SelectedQ_Checkbox3 = "true";
                            }
                        }

                        if (_Count > 3)
                        {
                            int _Question_Seq4 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Question_Seq).Single();
                            ViewBag.SelectedQuestion4 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq4).Select(Y => Y.Question_Text).Single();

                            if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                            {
                                ViewBag.SelectedQ_Checkbox4 = "false";
                            }
                            else
                            {
                                ViewBag.SelectedQ_Checkbox4 = "true";
                            }
                        }

                        if (_Count > 4)
                        {
                            int _Question_Seq5 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Question_Seq).Single();
                            ViewBag.SelectedQuestion5 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq5).Select(Y => Y.Question_Text).Single();

                            if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                            {
                                ViewBag.SelectedQ_Checkbox5 = "false";
                            }
                            else
                            {
                                ViewBag.SelectedQ_Checkbox5 = "true";
                            }
                        }

                        IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList();

                        ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                        ViewBag.Edit_or_New = "Edit";
                        return View("Edit_Questionnaire_Questions");
                    }
                    else
                    {
                        db.Questionnaire_Questions.RemoveRange(db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).ToList());
                        db.SaveChanges();
                        ViewBag.Questionnaire_ID = Questionnaire_ID;
                        ViewBag.Topic = Topic_Seq;
                        IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList();

                        ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                        ViewBag.Edit_or_New = "New";
                        return View("Edit_Questionnaire_Questions");
                    }
                    
                }
                else
                {
                    ViewBag.Name = Name;
                    ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
                    ViewBag.Description = Description;
                    ViewBag.Active_From = Active_From;
                   ViewBag.Assess = Assessment_Type;
                    ViewBag.Active_To = Active_To;
                    ViewBag.Questionnaire_ID = Questionnaire_ID;
                    ViewBag.DuplicateError = "A questionnaire with this name already exists. Please choose another name for the questionnaire.";
                    return View("Edit");
                }
            }
            else
            {
                if (db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Topic_Seq).Single() == Topic_Seq)
                {
                    //Topic has not been changed
                    //its fine update
                    Questionnaire questionnaire = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Single();
                    questionnaire.Name = Name;
                    questionnaire.Description = Description;
                    questionnaire.Active_From = Active_From;
                    questionnaire.Active_To = Active_To;
                    questionnaire.Assessment_Type = Assessment_Type;
                    questionnaire.Topic_Seq = Topic_Seq;
                    questionnaire.Create_Date = questionnaire.Create_Date;
                    questionnaire.Person_ID_Creator = questionnaire.Person_ID_Creator;
                    questionnaire.Person_ID_Involved = questionnaire.Person_ID_Involved;
                    questionnaire.Venue_Booking_Seq = questionnaire.Venue_Booking_Seq;
                    questionnaire.Venue_ID = questionnaire.Venue_ID;
                    questionnaire.Building_Floor_ID = questionnaire.Building_Floor_ID;
                    questionnaire.Building_ID = questionnaire.Building_ID;
                    questionnaire.Campus_ID = questionnaire.Campus_ID;
                    questionnaire.Questionnaire_ID = questionnaire.Questionnaire_ID;

                    db.Entry(questionnaire).State = EntityState.Modified;
                    //db.Questionnaires.Remove(db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Single());
                    //db.Questionnaires.Add(questionnaire);
                    db.SaveChanges();

                    ViewBag.Questionnaire_ID = Questionnaire_ID;
                    ViewBag.Topic = Topic_Seq;
                    int _Count = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).Count();

                    if (_Count > 0)
                    {
                        int _Question_Seq1 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Question_Seq).Single();
                        ViewBag.SelectedQuestion1 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq1).Select(Y => Y.Question_Text).Single();

                        if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 1).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                        {
                            ViewBag.SelectedQ_Checkbox1 = "false";
                        }
                        else
                        {
                            ViewBag.SelectedQ_Checkbox1 = "true";
                        }
                    }

                    if (_Count > 1)
                    {
                        int _Question_Seq2 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Question_Seq).Single();
                        ViewBag.SelectedQuestion2 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq2).Select(Y => Y.Question_Text).Single();

                        if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 2).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                        {
                            ViewBag.SelectedQ_Checkbox2 = "false";
                        }
                        else
                        {
                            ViewBag.SelectedQ_Checkbox2 = "true";
                        }
                    }

                    if (_Count > 2)
                    {
                        int _Question_Seq3 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Question_Seq).Single();
                        ViewBag.SelectedQuestion3 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq3).Select(Y => Y.Question_Text).Single();

                        if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 3).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                        {
                            ViewBag.SelectedQ_Checkbox3 = "false";
                        }
                        else
                        {
                            ViewBag.SelectedQ_Checkbox3 = "true";
                        }
                    }

                    if (_Count > 3)
                    {
                        int _Question_Seq4 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Question_Seq).Single();
                        ViewBag.SelectedQuestion4 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq4).Select(Y => Y.Question_Text).Single();

                        if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 4).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                        {
                            ViewBag.SelectedQ_Checkbox4 = "false";
                        }
                        else
                        {
                            ViewBag.SelectedQ_Checkbox4 = "true";
                        }
                    }

                    if (_Count > 4)
                    {
                        int _Question_Seq5 = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Question_Seq).Single();
                        ViewBag.SelectedQuestion5 = db.Question_Bank.Where(X => X.Question_Seq == _Question_Seq5).Select(Y => Y.Question_Text).Single();

                        if (db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID && X.Display_Order == 5).Select(Y => Y.Answer_Optional_Ind).Single() == 0)
                        {
                            ViewBag.SelectedQ_Checkbox5 = "false";
                        }
                        else
                        {
                            ViewBag.SelectedQ_Checkbox5 = "true";
                        }
                    }
                    IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList();
                    ViewBag.Edit_or_New = "Edit";
                    ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                    return View("Edit_Questionnaire_Questions");
                }
                else
                {
                    db.Questionnaire_Questions.RemoveRange(db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).ToList());
                    db.SaveChanges();
                    ViewBag.Questionnaire_ID = Questionnaire_ID;
                    ViewBag.Topic = Topic_Seq;
                    IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList();
                    ViewBag.Edit_or_New = "New";
                    ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                    return View("Edit_Questionnaire_Questions");
                }
            }
            
        }
        [Authorize(Roles = "Admin")]
        // GET: Questionnaire/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Questionnaire questionnaire = db.Questionnaires.Find(id);
            if (questionnaire == null)
            {
                return HttpNotFound();
            }
            if (questionnaire.Active_From <= DateTime.Now && DateTime.Now <= questionnaire.Active_To)
            {
                ViewBag.Status = "Active";
            }
            else if (DateTime.Now < questionnaire.Active_From)
            {
                ViewBag.Status = "Pending";
            }
            else
            {
                ViewBag.Status = "Expired";
            }
            ViewBag.id = id;
            return View(questionnaire);
        }
        [Authorize(Roles = "Admin")]
        // POST: Questionnaire/Delete/5        
        public ActionResult DeleteConfirmed(int id)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(id);

            //int Count = 0;

            //Count = db.Questionnaire_Questions.Where(X => X.Question_Seq == id).Count();

            //if (Count != 0)
            //{
            //    return RedirectToAction("Delete", new {id});
            //    ModelState.AddModelError("Question_Text", "This question already exists for the selected topic. Please choose another topic or question.");
            //}
            //else
            //{
            List<Questionnaire_Questions> remove_List = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == id).ToList();

            db.Questionnaire_Questions.RemoveRange(remove_List);

            db.Questionnaires.Remove(questionnaire);
            db.SaveChanges();
            return RedirectToAction("Index");


        }

        //public ActionResult Session_log()
        //{
            
        //    IEnumerable<Person_Session_Log> p = db.Person_Session_Log;
        //    List<SessionLog> b = new List<SessionLog>();
            
        //    foreach (var item in p)
        //    {
        //        foreach (var items in b)
        //        {
        //            items.system_usage = Convert.ToInt32(item.Logout_DateTime - item.Login_DateTime);
        //            items.Amount_of_sessions = db.Person_Session_Log.Where(X => X.Person_ID == item.Person_ID).Count();
        //            items.Person = 
        //        }
        //    }

        //    SessionLog sessionlog = 
            
        //}


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
