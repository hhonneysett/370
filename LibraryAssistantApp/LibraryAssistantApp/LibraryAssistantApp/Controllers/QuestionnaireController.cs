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
            ViewBag.DeleteComplete = "No";
            ViewBag.CreateComplete = "No";
            ViewBag.EditComplete = "No";
            return View(questionnaires.ToList());
        }
        [Authorize]
        public ActionResult Respond_to_questionnaire()
        {

            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            var questionnaires = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now).ToList();
            int Count_Response = 0;
            int Count_questionnaires = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now).Count();


            List<Questionnaire> Q_IDs = new List<Questionnaire>();
            Q_IDs = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now).ToList();
            string[] RowsResponded = new string[Count_questionnaires];
            string[] AnswerDates = new string[Count_questionnaires];
            for (int i = 0; i < Count_questionnaires; i++)
            {
                int getitem = Q_IDs[i].Questionnaire_ID;
                Count_Response = db.Person_Questionnaire.Where(X => X.Person_ID == User.Identity.Name && X.Questionnaire_ID == getitem).Count();
                if (Count_Response > 0)
                {
                    RowsResponded[i] = "Yes";
                }
                else
                {
                    RowsResponded[i] = "No";
                }

                 string Answer = db.Person_Questionnaire.Where(X => X.Person_ID == User.Identity.Name && X.Questionnaire_ID == getitem).Select(Y => Y.Answer_Date).FirstOrDefault().ToString();
               
                if (Answer == "1/1/0001 12:00:00 AM")
                {
                    AnswerDates[i] = "Not yet responded";
                }
                else
                {
                    AnswerDates[i] = Answer;
                }

                 

            }
            ViewBag.RowsRespond = RowsResponded;
            ViewBag.AnswerDates = AnswerDates;
            return View(questionnaires.ToList());
        }
        [Authorize]
        public ActionResult Get_Questionnaire_Results()
        {

            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");

            List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();
                                                           
            int Count_questionnaires = Q_IDs.Count();

            List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

            for (int i = 0; i < Count_questionnaires; i++)
            {
                int getitem = Q_IDs[i];
                Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem).Single();

                Questionnaires_With_Results.Add(b);
            }
                        
            return View("Get_Questionnaire_Results", Questionnaires_With_Results);
        }


        [Authorize]
        public ActionResult Get_questionnaire_Results_Search(string Search, int? Topic, string Assessment_Type)
        {
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Search = Search;



            if (Topic == null && Assessment_Type == "" && Search == "")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem).SingleOrDefault();

                    Questionnaires_With_Results.Add(b);
                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);


            }
            else if (Topic != null && Assessment_Type == "" && Search == "")
            {

                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem && X.Topic_Seq == Topic).SingleOrDefault();

                    if (b != null)
                    {
                        Questionnaires_With_Results.Add(b);
                    }

                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);

            }
            else if (Topic == null && Assessment_Type != "" && Search == "")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem && X.Assessment_Type == Assessment_Type).SingleOrDefault();
                    if (b != null)
                    {
                        Questionnaires_With_Results.Add(b);
                    }
                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);


            }
            else if (Topic == null && Assessment_Type == "" && Search != "")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem && X.Name.Contains(Search)).SingleOrDefault();
                    if (b != null)
                    {
                        Questionnaires_With_Results.Add(b);
                    }
                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);

            }
            else if (Topic != null && Assessment_Type != "" && Search == "")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type).SingleOrDefault();
                    if (b != null)
                    {
                        Questionnaires_With_Results.Add(b);
                    }
                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);


            }
            else if (Topic != null && Assessment_Type == "" && Search != "")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem && X.Topic_Seq == Topic && X.Name.Contains(Search)).SingleOrDefault();
                    if (b != null)
                    {
                        Questionnaires_With_Results.Add(b);
                    }
                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);


            }
            else if (Topic == null && Assessment_Type != "" && Search != "")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem && X.Assessment_Type == Assessment_Type && X.Name.Contains(Search)).SingleOrDefault();
                    if (b != null)
                    {
                        Questionnaires_With_Results.Add(b);
                    }
                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);


            }
            else if (Topic != null && Assessment_Type != "" && Search != "")
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

                ViewBag.Assessment_Type = Assessment_Type;
                ViewBag.Search = Search;

                List<int> Q_IDs = db.Person_Questionnaire.Select(X => X.Questionnaire_ID).Distinct().ToList();

                int Count_questionnaires = Q_IDs.Count();

                List<Questionnaire> Questionnaires_With_Results = new List<Questionnaire>();

                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i];
                    Questionnaire b = db.Questionnaires.Where(X => X.Questionnaire_ID == getitem && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type && X.Name.Contains(Search)).SingleOrDefault();
                    if (b != null)
                    {
                        Questionnaires_With_Results.Add(b);
                    }
                }

                return View("Get_Questionnaire_Results", Questionnaires_With_Results);


            }
            return View("Get_Questionnaire_Results");

        }
        [Authorize]
        public ActionResult Respond_to_questionnaire_Search(string Search, int? Topic, string Assessment_Type)
        {
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);

            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Search = Search;

            if (Topic == null && Assessment_Type == "")
            {
                int Count_questionnaires = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).Count();
                int Count_Response = 0;

                List<Questionnaire> Q_IDs = new List<Questionnaire>();
                Q_IDs = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList();
                string[] RowsResponded = new string[Count_questionnaires];
                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i].Questionnaire_ID;
                    Count_Response = db.Person_Questionnaire.Where(X => X.Person_ID == User.Identity.Name && X.Questionnaire_ID == getitem).Count();
                    if (Count_Response > 0)
                    {
                        RowsResponded[i] = "Yes";
                    }
                    else
                    {
                        RowsResponded[i] = "No";
                    }
                }

                ViewBag.RowsRespond = RowsResponded;
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type == "")
            {

                int Count_questionnaires = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).Count();
                int Count_Response = 0;

                List<Questionnaire> Q_IDs = new List<Questionnaire>();
                Q_IDs = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList();
                string[] RowsResponded = new string[Count_questionnaires];
                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i].Questionnaire_ID;
                    Count_Response = db.Person_Questionnaire.Where(X => X.Person_ID == User.Identity.Name && X.Questionnaire_ID == getitem).Count();
                    if (Count_Response > 0)
                    {
                        RowsResponded[i] = "Yes";
                    }
                    else
                    {
                        RowsResponded[i] = "No";
                    }
                }
                ViewBag.RowsRespond = RowsResponded;
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList());
            }
            else if (Topic == null && Assessment_Type != "")
            {

                int Count_questionnaires = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).Count();
                int Count_Response = 0;

                List<Questionnaire> Q_IDs = new List<Questionnaire>();
                Q_IDs = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList();
                string[] RowsResponded = new string[Count_questionnaires];
                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i].Questionnaire_ID;
                    Count_Response = db.Person_Questionnaire.Where(X => X.Person_ID == User.Identity.Name && X.Questionnaire_ID == getitem).Count();
                    if (Count_Response > 0)
                    {
                        RowsResponded[i] = "Yes";
                    }
                    else
                    {
                        RowsResponded[i] = "No";
                    }
                }
                ViewBag.RowsRespond = RowsResponded;
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList());
            }
            else if (Topic != null && Assessment_Type != "")
            {

                int Count_questionnaires = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).Count();
                int Count_Response = 0;

                List<Questionnaire> Q_IDs = new List<Questionnaire>();
                Q_IDs = db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList();
                string[] RowsResponded = new string[Count_questionnaires];
                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i].Questionnaire_ID;
                    Count_Response = db.Person_Questionnaire.Where(X => X.Person_ID == User.Identity.Name && X.Questionnaire_ID == getitem).Count();
                    if (Count_Response > 0)
                    {
                        RowsResponded[i] = "Yes";
                    }
                    else
                    {
                        RowsResponded[i] = "No";
                    }
                }
                ViewBag.RowsRespond = RowsResponded;
                return View("Respond_to_questionnaire", db.Questionnaires.Where(X => X.Name.Contains(Search) && X.Topic_Seq == Topic && X.Assessment_Type == Assessment_Type && X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now || Search == null).ToList());
            }
            else
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                var questionnaires = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now).ToList();
                int Count_Response = 0;
                int Count_questionnaires = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now).Count();


                List<Questionnaire> Q_IDs = new List<Questionnaire>();
                Q_IDs = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now).ToList();
                string[] RowsResponded = new string[Count_questionnaires];
                for (int i = 0; i < Count_questionnaires; i++)
                {
                    int getitem = Q_IDs[i].Questionnaire_ID;
                    Count_Response = db.Person_Questionnaire.Where(X => X.Person_ID == User.Identity.Name && X.Questionnaire_ID == getitem).Count();
                    if (Count_Response > 0)
                    {
                        RowsResponded[i] = "Yes";
                    }
                    else
                    {
                        RowsResponded[i] = "No";
                    }


                }
                ViewBag.RowsRespond = RowsResponded;
                return View("Index", questionnaires.ToList());
            }
        }
        [Authorize]
        public ActionResult Save_Questionnaire_Responses(int Questionnaire_ID, string Question1, string Question2, string Question3, string Question4, string Question5, string Question1_Reply, string Question2_Reply, string Question3_Reply, string Question4_Reply, string Question5_Reply)
        {
            Person_Questionnaire person_Questionnaire = new Person_Questionnaire();
            person_Questionnaire.Questionnaire_ID = Questionnaire_ID;
            person_Questionnaire.Person_ID = User.Identity.Name;
            person_Questionnaire.Answer_Date = DateTime.Now;
            person_Questionnaire.Status = "Done";
            db.Person_Questionnaire.Add(person_Questionnaire);
            db.SaveChanges();

            if (Question1 != "undefined")
            {
                Person_Questionnaire_Result person_Questionnaire_Result1 = new Person_Questionnaire_Result();
                person_Questionnaire_Result1.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result1.Person_ID = User.Identity.Name;
                person_Questionnaire_Result1.Question_Answer = Question1_Reply;
                person_Questionnaire_Result1.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question1).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result1);
                db.SaveChanges();
            }
            if (Question2 != "undefined")
            {
                Person_Questionnaire_Result person_Questionnaire_Result2 = new Person_Questionnaire_Result();
                person_Questionnaire_Result2.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result2.Person_ID = User.Identity.Name;
                person_Questionnaire_Result2.Question_Answer = Question2_Reply;
                person_Questionnaire_Result2.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question2).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result2);
                db.SaveChanges();
            }
            if (Question3 != "undefined")
            {
                Person_Questionnaire_Result person_Questionnaire_Result3 = new Person_Questionnaire_Result();
                person_Questionnaire_Result3.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result3.Person_ID = User.Identity.Name;
                person_Questionnaire_Result3.Question_Answer = Question3_Reply;
                person_Questionnaire_Result3.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question3).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result3);
                db.SaveChanges();
            }
            if (Question4 != "undefined")
            {
                Person_Questionnaire_Result person_Questionnaire_Result4 = new Person_Questionnaire_Result();
                person_Questionnaire_Result4.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result4.Person_ID = User.Identity.Name;
                person_Questionnaire_Result4.Question_Answer = Question4_Reply;
                person_Questionnaire_Result4.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question4).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result4);
                db.SaveChanges();
            }
            if (Question5 != "undefined")
            {
                Person_Questionnaire_Result person_Questionnaire_Result5 = new Person_Questionnaire_Result();
                person_Questionnaire_Result5.Questionnaire_ID = Questionnaire_ID;
                person_Questionnaire_Result5.Person_ID = User.Identity.Name;
                person_Questionnaire_Result5.Question_Answer = Question5_Reply;
                person_Questionnaire_Result5.Question_Seq = db.Question_Bank.Where(X => X.Question_Text == Question5).Select(Y => Y.Question_Seq).Single();
                db.Person_Questionnaire_Result.Add(person_Questionnaire_Result5);
                db.SaveChanges();
            }

            // -------------------------------Action Log ----------------------------------------//
            string name = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single();
            Person_Session_Action_Log psal = new Person_Session_Action_Log();
            psal.Action_DateTime = DateTime.Now;
            psal.Action_ID = 11;
            psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
            //psal.Ac = "Delete";
            psal.Action_Performed = "To questionnaire: " + name;
            psal.Crud_Operation = "Respond";
            db.Person_Session_Action_Log.Add(psal);
            db.SaveChanges();
            // -------------------------------Action Log ----------------------------------------//


            return RedirectToAction("Index", "Home");
        }

        [Authorize]
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

                double Amount_of_Days = (questionnaire.Active_To - DateTime.Now).TotalHours;

                if (Amount_of_Days > 24)
                {
                    Amount_of_Days = Amount_of_Days / 24;
                    ViewBag.Days = Math.Round(Amount_of_Days, 0) + " Days";
                    ViewBag.StatusPhrase = "Questionnaire expires in:";
                }
                else
                {
                    ViewBag.Days = Math.Round(Amount_of_Days, 0) + " Hours";
                    ViewBag.StatusPhrase = "Questionnaire expires in:";
                }


            }
            else if (DateTime.Now < questionnaire.Active_From)
            {

                ViewBag.Status = "Pending";
                double Amount_of_Days = (questionnaire.Active_From - DateTime.Now).TotalHours;
                if (Amount_of_Days > 24)
                {
                    Amount_of_Days = Amount_of_Days / 24;
                    ViewBag.Days = Math.Round(Amount_of_Days, 0) + " Days";
                    ViewBag.StatusPhrase = "Questionnaire activates in:";
                }
                else
                {
                    ViewBag.Days = Math.Round(Amount_of_Days, 0) + " Hours";
                    ViewBag.StatusPhrase = "Questionnaire activates in:";
                }
            }
            else
            {
                ViewBag.StatusPhrase = "None";
                ViewBag.Status = "Expired";
            }

            ViewBag.Responded = db.Person_Questionnaire.Where(X => X.Questionnaire_ID == id).Count().ToString();
            ViewBag.AmountOfQuestions = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == id).Count().ToString();

            // ----------Person_ID_Involved
            if (questionnaire.Person_ID_Involved != null)
            {
                ViewBag.Person_Involved = db.Registered_Person.Where(X => X.Person_ID == questionnaire.Person_ID_Involved).Select(Y => Y.Person_Name).Single();
            }
            else
            {
                ViewBag.Person_Involved = "None";
            }


            // ----------Venue_Booking_Seq
            if (questionnaire.Venue_Booking_Seq != null)
            {
                Venue_Booking selectedBooking = db.Venue_Booking.Find(questionnaire.Venue_Booking_Seq);
                ViewBag.Selected_Training_Session = Convert.ToString(selectedBooking.DateTime_From) + " - " + Convert.ToString(selectedBooking.DateTime_To);
                ViewBag.Training_Topic = db.Topics.Where(X => X.Topic_Seq == selectedBooking.Topic_Seq).Select(Y => Y.Topic_Name).Single();
            }
            else
            {
                ViewBag.Selected_Training_Session = "None";
            }

            // ----------Venue
            var qvenue = db.Venues.Where(v => v.Venue_ID == questionnaire.Venue_ID).First();
            if (qvenue != null)
            {
                Venue myVenue = db.Venues.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID && X.Venue_ID == questionnaire.Venue_ID).Single();

                ViewBag.VenueName = myVenue.Venue_Name;
                ViewBag.BuildingName = db.Buildings.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID).Select(Y => Y.Building_Name).Single();
                ViewBag.BuildingFloor = db.Building_Floor.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID).Select(Y => Y.Floor_Name).Single();
                ViewBag.Campus = db.Campus.Where(X => X.Campus_ID == questionnaire.Campus_ID).Select(Y => Y.Campus_Name).Single();
}
            else
            {
                ViewBag.VenueName = "None";
            }


            return View(questionnaire);
        }

        [Authorize]
        public ActionResult ReSubmit(int id)
        {


            List<Person_Questionnaire_Result> remove_List = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Person_ID == User.Identity.Name).ToList();

            db.Person_Questionnaire_Result.RemoveRange(remove_List);
            Person_Questionnaire removing = db.Person_Questionnaire.Where(X => X.Questionnaire_ID == id && X.Person_ID == User.Identity.Name).Single();


            db.Person_Questionnaire.Remove(removing);
            db.SaveChanges();



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

            if (questionnaire.Person_ID_Involved != null)
            {
                ViewBag.Person_Involved = db.Registered_Person.Where(X => X.Person_ID == questionnaire.Person_ID_Involved).Select(Y => Y.Person_Name).Single();
            }
            else
            {
                ViewBag.Person_Involved = "None";
            }

            // ----------Venue_Booking_Seq
            if (questionnaire.Venue_Booking_Seq != null)
            {
                Venue_Booking selectedBooking = db.Venue_Booking.Find(questionnaire.Venue_Booking_Seq);
                ViewBag.Selected_Training_Session = Convert.ToString(selectedBooking.DateTime_From) + " - " + Convert.ToString(selectedBooking.DateTime_To);
                ViewBag.Training_Topic = db.Topics.Where(X => X.Topic_Seq == selectedBooking.Topic_Seq).Select(Y => Y.Topic_Name).Single();
            }
            else
            {
                ViewBag.Selected_Training_Session = "None";
            }
            // ----------Venue
            if (questionnaire.Venue_ID != null)
            {
                Venue myVenue = db.Venues.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID && X.Venue_ID == questionnaire.Venue_ID).Single();

                ViewBag.VenueName = myVenue.Venue_Name;
                ViewBag.BuildingName = db.Buildings.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID).Select(Y => Y.Building_Name).Single();
                ViewBag.BuildingFloor = db.Building_Floor.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID).Select(Y => Y.Floor_Name).Single();
                ViewBag.Campus = db.Campus.Where(X => X.Campus_ID == questionnaire.Campus_ID).Select(Y => Y.Campus_Name).Single();
            }
            else
            {
                ViewBag.VenueName = "None";
            }

            return View("Answer_Questionnaire");
        }
        

       [Authorize]
        public ActionResult Questionnaire_Results(int id)
        {
            
                Questionnaire questionnaire = db.Questionnaires.Where(X => X.Questionnaire_ID == id).Single();
           

                ViewBag.Questionnaire_Name = questionnaire.Name;
            int ResponsesForQ = db.Person_Questionnaire.Where(X => X.Questionnaire_ID == id).Count();
            ViewBag.Responded = ResponsesForQ.ToString();
                //ViewBag.Questionnaire_ID = questionnaire.Questionnaire_ID;
                //ViewBag.Questionnaire_Topic = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();

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
                //List<Person_Questionnaire_Result> Results_For1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1).ToList();

                if (_StyleType1 == "Multiple Select list")
                {

                    //List of answers that are all multiselect
                    List<Person_Questionnaire_Result> v = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1).ToList();

                    //Going to put everything into this list to be used to compare against possible answers
                    List<string> s = new List<string>();

                    foreach (var item in v)
                    {
                        string c = item.Question_Answer.ToString();
                        string[] items = c.Split(',');
                        s.AddRange(items);
                    }

                    if (Count1 == 1)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q1)
                            {
                                Responses_1_Q1++;
                            }
                        }

                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / s.Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;                        
                    }
                    else if (Count1 == 2)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q1)
                            {
                                Responses_1_Q1++;
                            }
                        }
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / s.Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;


                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q1)
                            {
                                Responses_2_Q1++;
                            }
                        }
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / s.Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;
                    }
                    else if (Count1 == 3)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q1)
                            {
                                Responses_1_Q1++;
                            }
                        }
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / s.Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;


                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q1)
                            {
                                Responses_2_Q1++;
                            }
                        }
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / s.Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;


                        string AnswerText3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q1 = AnswerText3_Q1;
                        int Responses_3_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q1)
                            {
                                Responses_3_Q1++;
                            }
                        }
                        ViewBag.Percentage_3_Q1 = (Responses_3_Q1 * 100) / s.Count();
                        ViewBag.Responses_3_Q1 = Responses_3_Q1;
                    }
                    else if (Count1 == 4)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q1)
                            {
                                Responses_1_Q1++;
                            }
                        }
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / s.Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;


                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q1)
                            {
                                Responses_2_Q1++;
                            }
                        }
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / s.Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;


                        string AnswerText3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q1 = AnswerText3_Q1;
                        int Responses_3_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q1)
                            {
                                Responses_3_Q1++;
                            }
                        }
                        ViewBag.Percentage_3_Q1 = (Responses_3_Q1 * 100) / s.Count();
                        ViewBag.Responses_3_Q1 = Responses_3_Q1;


                        string AnswerText4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q1 = AnswerText4_Q1;
                        int Responses_4_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q1)
                            {
                                Responses_4_Q1++;
                            }
                        }
                        ViewBag.Percentage_4_Q1 = (Responses_4_Q1 * 100) / s.Count();
                        ViewBag.Responses_4_Q1 = Responses_4_Q1;
                    }
                    else if (Count1 == 5)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q1)
                            {
                                Responses_1_Q1++;
                            }
                        }
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / s.Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;


                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q1)
                            {
                                Responses_2_Q1++;
                            }
                        }
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / s.Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;


                        string AnswerText3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q1 = AnswerText3_Q1;
                        int Responses_3_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q1)
                            {
                                Responses_3_Q1++;
                            }
                        }
                        ViewBag.Percentage_3_Q1 = (Responses_3_Q1 * 100) / s.Count();
                        ViewBag.Responses_3_Q1 = Responses_3_Q1;


                        string AnswerText4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q1 = AnswerText4_Q1;
                        int Responses_4_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q1)
                            {
                                Responses_4_Q1++;
                            }
                        }

                        ViewBag.Responses_4_Q1 = Responses_4_Q1;
                        ViewBag.Percentage_4_Q1 = (Responses_4_Q1 * 100) / s.Count();


                        string AnswerText5_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q1 = AnswerText5_Q1;
                        int Responses_5_Q1 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText5_Q1)
                            {
                                Responses_5_Q1++;
                            }
                        }
                        ViewBag.Percentage_5_Q1 = (Responses_5_Q1 * 100) / s.Count();
                        ViewBag.Responses_5_Q1 = Responses_5_Q1;
                    }
                }
                else
                {
                    if (Count1 == 0)
                    {//Free Text
                        List<string> Free_text_responses = new List<string>();
                        Free_text_responses.AddRange(db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1).Select(Y => Y.Question_Answer));
                        ViewBag.Free_text_responses_Q1 = Free_text_responses;
                        ViewBag.Free_text_responses_Q1_Count = Free_text_responses.Count();
                    }
                    else if (Count1 == 1)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText1_Q1).Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / ResponsesForQ;
                    }
                    else if (Count1 == 2)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText1_Q1).Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / ResponsesForQ;

                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText2_Q1).Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / ResponsesForQ;

                    }
                    else if (Count1 == 3)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText1_Q1).Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / ResponsesForQ;

                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText2_Q1).Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / ResponsesForQ;

                        string AnswerText3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q1 = AnswerText3_Q1;
                        int Responses_3_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText3_Q1).Count();
                        ViewBag.Responses_3_Q1 = Responses_3_Q1;
                        ViewBag.Percentage_3_Q1 = (Responses_3_Q1 * 100) / ResponsesForQ;
                    }
                    else if (Count1 == 4)
                    {

                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText1_Q1).Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / ResponsesForQ;

                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText2_Q1).Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / ResponsesForQ;

                        string AnswerText3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q1 = AnswerText3_Q1;
                        int Responses_3_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText3_Q1).Count();
                        ViewBag.Responses_3_Q1 = Responses_3_Q1;
                        ViewBag.Percentage_3_Q1 = (Responses_3_Q1 * 100) / ResponsesForQ;

                        string AnswerText4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q1 = AnswerText4_Q1;
                        int Responses_4_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText4_Q1).Count();
                        ViewBag.Responses_4_Q1 = Responses_4_Q1;
                        ViewBag.Percentage_4_Q1 = (Responses_4_Q1 * 100) / ResponsesForQ;

                    }
                    else if (Count1 == 5)
                    {
                        string AnswerText1_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q1 = AnswerText1_Q1;
                        int Responses_1_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText1_Q1).Count();
                        ViewBag.Responses_1_Q1 = Responses_1_Q1;
                        ViewBag.Percentage_1_Q1 = (Responses_1_Q1 * 100) / ResponsesForQ;

                        string AnswerText2_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q1 = AnswerText2_Q1;
                        int Responses_2_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText2_Q1).Count();
                        ViewBag.Responses_2_Q1 = Responses_2_Q1;
                        ViewBag.Percentage_2_Q1 = (Responses_2_Q1 * 100) / ResponsesForQ;

                        string AnswerText3_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q1 = AnswerText3_Q1;
                        int Responses_3_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText3_Q1).Count();
                        ViewBag.Responses_3_Q1 = Responses_3_Q1;
                        ViewBag.Percentage_3_Q1 = (Responses_3_Q1 * 100) / ResponsesForQ;

                        string AnswerText4_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q1 = AnswerText4_Q1;
                        int Responses_4_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText4_Q1).Count();
                        ViewBag.Responses_4_Q1 = Responses_4_Q1;
                        ViewBag.Percentage_4_Q1 = (Responses_4_Q1 * 100) / ResponsesForQ;

                        string AnswerText5_Q1 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq1 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q1 = AnswerText5_Q1;
                        int Responses_5_Q1 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq1 && X.Question_Answer == AnswerText5_Q1).Count();
                        ViewBag.Responses_5_Q1 = Responses_5_Q1;
                        ViewBag.Percentage_5_Q1 = (Responses_5_Q1 * 100) / ResponsesForQ;
                    }
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

                if (_StyleType2 == "Multiple Select list")
                {

                    //List of answers that are all multiselect
                    List<Person_Questionnaire_Result> v = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2).ToList();

                    //Going to put everything into this list to be used to compare against possible answers
                    List<string> s = new List<string>();

                    foreach (var item in v)
                    {
                        string c = item.Question_Answer.ToString();
                        string[] items = c.Split(',');
                        s.AddRange(items);
                    }

                    if (Count2 == 1)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q2)
                            {
                                Responses_1_Q2++;
                            }
                        }
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / s.Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;
                    }
                    else if (Count2 == 2)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q2)
                            {
                                Responses_1_Q2++;
                            }
                        }
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / s.Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;


                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q2)
                            {
                                Responses_2_Q2++;
                            }
                        }
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / s.Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;
                    }
                    else if (Count2 == 3)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q2)
                            {
                                Responses_1_Q2++;
                            }
                        }
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / s.Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;


                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q2)
                            {
                                Responses_2_Q2++;
                            }
                        }
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / s.Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;


                        string AnswerText3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q2 = AnswerText3_Q2;
                        int Responses_3_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q2)
                            {
                                Responses_3_Q2++;
                            }
                        }
                        ViewBag.Percentage_3_Q2 = (Responses_3_Q2 * 100) / s.Count();
                        ViewBag.Responses_3_Q2 = Responses_3_Q2;
                    }
                    else if (Count2 == 4)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q2)
                            {
                                Responses_1_Q2++;
                            }
                        }
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / s.Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;


                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q2)
                            {
                                Responses_2_Q2++;
                            }
                        }
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / s.Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;


                        string AnswerText3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q2 = AnswerText3_Q2;
                        int Responses_3_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q2)
                            {
                                Responses_3_Q2++;
                            }
                        }
                        ViewBag.Percentage_3_Q2 = (Responses_3_Q2 * 100) / s.Count();
                        ViewBag.Responses_3_Q2 = Responses_3_Q2;


                        string AnswerText4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q2 = AnswerText4_Q2;
                        int Responses_4_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q2)
                            {
                                Responses_4_Q2++;
                            }
                        }
                        ViewBag.Percentage_4_Q2 = (Responses_4_Q2 * 100) / s.Count();
                        ViewBag.Responses_4_Q2 = Responses_4_Q2;
                    }
                    else if (Count2 == 5)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q2)
                            {
                                Responses_1_Q2++;
                            }
                        }
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / s.Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;


                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q2)
                            {
                                Responses_2_Q2++;
                            }
                        }
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / s.Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;


                        string AnswerText3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q2 = AnswerText3_Q2;
                        int Responses_3_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q2)
                            {
                                Responses_3_Q2++;
                            }
                        }
                        ViewBag.Percentage_3_Q2 = (Responses_3_Q2 * 100) / s.Count();
                        ViewBag.Responses_3_Q2 = Responses_3_Q2;


                        string AnswerText4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q2 = AnswerText4_Q2;
                        int Responses_4_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q2)
                            {
                                Responses_4_Q2++;
                            }
                        }
                        ViewBag.Percentage_4_Q2 = (Responses_4_Q2 * 100) / s.Count();
                        ViewBag.Responses_4_Q2 = Responses_4_Q2;



                        string AnswerText5_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q2 = AnswerText5_Q2;
                        int Responses_5_Q2 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText5_Q2)
                            {
                                Responses_5_Q2++;
                            }
                        }
                        ViewBag.Percentage_5_Q2 = (Responses_5_Q2 * 100) / s.Count();
                        ViewBag.Responses_5_Q2 = Responses_5_Q2;
                    }
                }
                else
                {
                    if (Count2 == 0)
                    {//Free Text
                        List<string> Free_text_responses = new List<string>();
                        Free_text_responses.AddRange(db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2).Select(Y => Y.Question_Answer));
                        ViewBag.Free_text_responses_Q2 = Free_text_responses;
                        ViewBag.Free_text_responses_Q2_Count = Free_text_responses.Count();
                    }
                    else if (Count2 == 1)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText1_Q2).Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / ResponsesForQ;
                    }
                    else if (Count2 == 2)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText1_Q2).Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / ResponsesForQ;

                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText2_Q2).Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / ResponsesForQ;

                    }
                    else if (Count2 == 3)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText1_Q2).Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / ResponsesForQ;

                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText2_Q2).Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / ResponsesForQ;

                        string AnswerText3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q2 = AnswerText3_Q2;
                        int Responses_3_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText3_Q2).Count();
                        ViewBag.Responses_3_Q2 = Responses_3_Q2;
                        ViewBag.Percentage_3_Q2 = (Responses_3_Q2 * 100) / ResponsesForQ;
                    }
                    else if (Count2 == 4)
                    {

                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText1_Q2).Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / ResponsesForQ;

                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText2_Q2).Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / ResponsesForQ;

                        string AnswerText3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q2 = AnswerText3_Q2;
                        int Responses_3_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText3_Q2).Count();
                        ViewBag.Responses_3_Q2 = Responses_3_Q2;
                        ViewBag.Percentage_3_Q2 = (Responses_3_Q2 * 100) / ResponsesForQ;

                        string AnswerText4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q2 = AnswerText4_Q2;
                        int Responses_4_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText4_Q2).Count();
                        ViewBag.Responses_4_Q2 = Responses_4_Q2;
                        ViewBag.Percentage_4_Q2 = (Responses_4_Q2 * 100) / ResponsesForQ;

                    }
                    else if (Count2 == 5)
                    {
                        string AnswerText1_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q2 = AnswerText1_Q2;
                        int Responses_1_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText1_Q2).Count();
                        ViewBag.Responses_1_Q2 = Responses_1_Q2;
                        ViewBag.Percentage_1_Q2 = (Responses_1_Q2 * 100) / ResponsesForQ;

                        string AnswerText2_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q2 = AnswerText2_Q2;
                        int Responses_2_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText2_Q2).Count();
                        ViewBag.Responses_2_Q2 = Responses_2_Q2;
                        ViewBag.Percentage_2_Q2 = (Responses_2_Q2 * 100) / ResponsesForQ;

                        string AnswerText3_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q2 = AnswerText3_Q2;
                        int Responses_3_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText3_Q2).Count();
                        ViewBag.Responses_3_Q2 = Responses_3_Q2;
                        ViewBag.Percentage_3_Q2 = (Responses_3_Q2 * 100) / ResponsesForQ;

                        string AnswerText4_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q2 = AnswerText4_Q2;
                        int Responses_4_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText4_Q2).Count();
                        ViewBag.Responses_4_Q2 = Responses_4_Q2;
                        ViewBag.Percentage_4_Q2 = (Responses_4_Q2 * 100) / ResponsesForQ;

                        string AnswerText5_Q2 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq2 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q2 = AnswerText5_Q2;
                        int Responses_5_Q2 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq2 && X.Question_Answer == AnswerText5_Q2).Count();
                        ViewBag.Responses_5_Q2 = Responses_5_Q2;
                        ViewBag.Percentage_5_Q2 = (Responses_5_Q2 * 100) / ResponsesForQ;
                    }
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

                if (_StyleType3 == "Multiple Select list")
                {

                    //List of answers that are all multiselect
                    List<Person_Questionnaire_Result> v = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3).ToList();

                    //Going to put everything into this list to be used to compare against possible answers
                    List<string> s = new List<string>();

                    foreach (var item in v)
                    {
                        string c = item.Question_Answer.ToString();
                        string[] items = c.Split(',');
                        s.AddRange(items);
                    }

                    if (Count3 == 1)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q3)
                            {
                                Responses_1_Q3++;
                            }
                        }
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / s.Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;
                    }
                    else if (Count3 == 2)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q3)
                            {
                                Responses_1_Q3++;
                            }
                        }
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / s.Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;


                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q3)
                            {
                                Responses_2_Q3++;
                            }
                        }
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / s.Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;
                    }
                    else if (Count3 == 3)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q3)
                            {
                                Responses_1_Q3++;
                            }
                        }
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / s.Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;


                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q3)
                            {
                                Responses_2_Q3++;
                            }
                        }
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / s.Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;


                        string AnswerText3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q3 = AnswerText3_Q3;
                        int Responses_3_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q3)
                            {
                                Responses_3_Q3++;
                            }
                        }
                        ViewBag.Percentage_3_Q3 = (Responses_3_Q3 * 100) / s.Count();
                        ViewBag.Responses_3_Q3 = Responses_3_Q3;
                    }
                    else if (Count3 == 4)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q3)
                            {
                                Responses_1_Q3++;
                            }
                        }
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / s.Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;


                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q3)
                            {
                                Responses_2_Q3++;
                            }
                        }
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / s.Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;


                        string AnswerText3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q3 = AnswerText3_Q3;
                        int Responses_3_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q3)
                            {
                                Responses_3_Q3++;
                            }
                        }
                        ViewBag.Percentage_3_Q3 = (Responses_3_Q3 * 100) / s.Count();
                        ViewBag.Responses_3_Q3 = Responses_3_Q3;


                        string AnswerText4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q3 = AnswerText4_Q3;
                        int Responses_4_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q3)
                            {
                                Responses_4_Q3++;
                            }
                        }
                        ViewBag.Percentage_4_Q3 = (Responses_4_Q3 * 100) / s.Count();
                        ViewBag.Responses_4_Q3 = Responses_4_Q3;
                    }
                    else if (Count3 == 5)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q3)
                            {
                                Responses_1_Q3++;
                            }
                        }
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / s.Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;


                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q3)
                            {
                                Responses_2_Q3++;
                            }
                        }
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / s.Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;


                        string AnswerText3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q3 = AnswerText3_Q3;
                        int Responses_3_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q3)
                            {
                                Responses_3_Q3++;
                            }
                        }
                        ViewBag.Percentage_3_Q3 = (Responses_3_Q3 * 100) / s.Count();
                        ViewBag.Responses_3_Q3 = Responses_3_Q3;


                        string AnswerText4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q3 = AnswerText4_Q3;
                        int Responses_4_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q3)
                            {
                                Responses_4_Q3++;
                            }
                        }
                        ViewBag.Percentage_4_Q3 = (Responses_4_Q3 * 100) / s.Count();
                        ViewBag.Responses_4_Q3 = Responses_4_Q3;



                        string AnswerText5_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q3 = AnswerText5_Q3;
                        int Responses_5_Q3 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText5_Q3)
                            {
                                Responses_5_Q3++;
                            }
                        }
                        ViewBag.Percentage_5_Q3 = (Responses_5_Q3 * 100) / s.Count();
                        ViewBag.Responses_5_Q3 = Responses_5_Q3;
                    }
                }
                else
                {
                    if (Count3 == 0)
                    {//Free Text
                        List<string> Free_text_responses = new List<string>();
                        Free_text_responses.AddRange(db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3).Select(Y => Y.Question_Answer));
                        ViewBag.Free_text_responses_Q3 = Free_text_responses;
                        ViewBag.Free_text_responses_Q3_Count = Free_text_responses.Count();
                    }
                    else if (Count3 == 1)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText1_Q3).Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / ResponsesForQ;
                    }
                    else if (Count3 == 2)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText1_Q3).Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / ResponsesForQ;

                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText2_Q3).Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / ResponsesForQ;

                    }
                    else if (Count3 == 3)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText1_Q3).Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / ResponsesForQ;

                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText2_Q3).Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / ResponsesForQ;

                        string AnswerText3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q3 = AnswerText3_Q3;
                        int Responses_3_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText3_Q3).Count();
                        ViewBag.Responses_3_Q3 = Responses_3_Q3;
                        ViewBag.Percentage_3_Q3 = (Responses_3_Q3 * 100) / ResponsesForQ;
                    }
                    else if (Count3 == 4)
                    {

                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText1_Q3).Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / ResponsesForQ;

                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText2_Q3).Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / ResponsesForQ;

                        string AnswerText3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q3 = AnswerText3_Q3;
                        int Responses_3_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText3_Q3).Count();
                        ViewBag.Responses_3_Q3 = Responses_3_Q3;
                        ViewBag.Percentage_3_Q3 = (Responses_3_Q3 * 100) / ResponsesForQ;

                        string AnswerText4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q3 = AnswerText4_Q3;
                        int Responses_4_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText4_Q3).Count();
                        ViewBag.Responses_4_Q3 = Responses_4_Q3;
                        ViewBag.Percentage_4_Q3 = (Responses_4_Q3 * 100) / ResponsesForQ;

                    }
                    else if (Count3 == 5)
                    {
                        string AnswerText1_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q3 = AnswerText1_Q3;
                        int Responses_1_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText1_Q3).Count();
                        ViewBag.Responses_1_Q3 = Responses_1_Q3;
                        ViewBag.Percentage_1_Q3 = (Responses_1_Q3 * 100) / ResponsesForQ;

                        string AnswerText2_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q3 = AnswerText2_Q3;
                        int Responses_2_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText2_Q3).Count();
                        ViewBag.Responses_2_Q3 = Responses_2_Q3;
                        ViewBag.Percentage_2_Q3 = (Responses_2_Q3 * 100) / ResponsesForQ;

                        string AnswerText3_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q3 = AnswerText3_Q3;
                        int Responses_3_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText3_Q3).Count();
                        ViewBag.Responses_3_Q3 = Responses_3_Q3;
                        ViewBag.Percentage_3_Q3 = (Responses_3_Q3 * 100) / ResponsesForQ;

                        string AnswerText4_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q3 = AnswerText4_Q3;
                        int Responses_4_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText4_Q3).Count();
                        ViewBag.Responses_4_Q3 = Responses_4_Q3;
                        ViewBag.Percentage_4_Q3 = (Responses_4_Q3 * 100) / ResponsesForQ;

                        string AnswerText5_Q3 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq3 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q3 = AnswerText5_Q3;
                        int Responses_5_Q3 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq3 && X.Question_Answer == AnswerText5_Q3).Count();
                        ViewBag.Responses_5_Q3 = Responses_5_Q3;
                        ViewBag.Percentage_5_Q3 = (Responses_5_Q3 * 100) / ResponsesForQ;
                    }
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

                if (_StyleType4 == "Multiple Select list")
                {

                    //List of answers that are all multiselect
                    List<Person_Questionnaire_Result> v = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4).ToList();

                    //Going to put everything into this list to be used to compare against possible answers
                    List<string> s = new List<string>();

                    foreach (var item in v)
                    {
                        string c = item.Question_Answer.ToString();
                        string[] items = c.Split(',');
                        s.AddRange(items);
                    }

                    if (Count4 == 1)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q4)
                            {
                                Responses_1_Q4++;
                            }
                        }
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / s.Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;
                    }
                    else if (Count4 == 2)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q4)
                            {
                                Responses_1_Q4++;
                            }
                        }
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / s.Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;


                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q4)
                            {
                                Responses_2_Q4++;
                            }
                        }
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / s.Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;
                    }
                    else if (Count4 == 3)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q4)
                            {
                                Responses_1_Q4++;
                            }
                        }
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / s.Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;


                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q4)
                            {
                                Responses_2_Q4++;
                            }
                        }
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / s.Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;


                        string AnswerText3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q4 = AnswerText3_Q4;
                        int Responses_3_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q4)
                            {
                                Responses_3_Q4++;
                            }
                        }
                        ViewBag.Percentage_3_Q4 = (Responses_3_Q4 * 100) / s.Count();
                        ViewBag.Responses_3_Q4 = Responses_3_Q4;
                    }
                    else if (Count4 == 4)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q4)
                            {
                                Responses_1_Q4++;
                            }
                        }
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / s.Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;


                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q4)
                            {
                                Responses_2_Q4++;
                            }
                        }
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / s.Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;


                        string AnswerText3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q4 = AnswerText3_Q4;
                        int Responses_3_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q4)
                            {
                                Responses_3_Q4++;
                            }
                        }
                        ViewBag.Percentage_3_Q4 = (Responses_3_Q4 * 100) / s.Count();
                        ViewBag.Responses_3_Q4 = Responses_3_Q4;


                        string AnswerText4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q4 = AnswerText4_Q4;
                        int Responses_4_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q4)
                            {
                                Responses_4_Q4++;
                            }
                        }
                        ViewBag.Percentage_4_Q4 = (Responses_4_Q4 * 100) / s.Count();
                        ViewBag.Responses_4_Q4 = Responses_4_Q4;
                    }
                    else if (Count4 == 5)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q4)
                            {
                                Responses_1_Q4++;
                            }
                        }
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / s.Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;


                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q4)
                            {
                                Responses_2_Q4++;
                            }
                        }
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / s.Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;


                        string AnswerText3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q4 = AnswerText3_Q4;
                        int Responses_3_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q4)
                            {
                                Responses_3_Q4++;
                            }
                        }
                        ViewBag.Percentage_3_Q4 = (Responses_3_Q4 * 100) / s.Count();
                        ViewBag.Responses_3_Q4 = Responses_3_Q4;


                        string AnswerText4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q4 = AnswerText4_Q4;
                        int Responses_4_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q4)
                            {
                                Responses_4_Q4++;
                            }
                        }
                        ViewBag.Percentage_4_Q4 = (Responses_4_Q4 * 100) / s.Count();
                        ViewBag.Responses_4_Q4 = Responses_4_Q4;



                        string AnswerText5_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q4 = AnswerText5_Q4;
                        int Responses_5_Q4 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText5_Q4)
                            {
                                Responses_5_Q4++;
                            }
                        }
                        ViewBag.Percentage_5_Q4 = (Responses_5_Q4 * 100) / s.Count();
                        ViewBag.Responses_5_Q4 = Responses_5_Q4;
                    }
                }
                else
                {
                    if (Count4 == 0)
                    {//Free Text
                        List<string> Free_text_responses = new List<string>();
                        Free_text_responses.AddRange(db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4).Select(Y => Y.Question_Answer));
                        ViewBag.Free_text_responses_Q4 = Free_text_responses;
                        ViewBag.Free_text_responses_Q4_Count = Free_text_responses.Count();
                    }
                    else if (Count4 == 1)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText1_Q4).Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / ResponsesForQ;
                    }
                    else if (Count4 == 2)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText1_Q4).Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / ResponsesForQ;

                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText2_Q4).Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / ResponsesForQ;

                    }
                    else if (Count4 == 3)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText1_Q4).Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / ResponsesForQ;

                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText2_Q4).Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / ResponsesForQ;

                        string AnswerText3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q4 = AnswerText3_Q4;
                        int Responses_3_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText3_Q4).Count();
                        ViewBag.Responses_3_Q4 = Responses_3_Q4;
                        ViewBag.Percentage_3_Q4 = (Responses_3_Q4 * 100) / ResponsesForQ;
                    }
                    else if (Count4 == 4)
                    {

                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText1_Q4).Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / ResponsesForQ;

                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText2_Q4).Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / ResponsesForQ;

                        string AnswerText3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q4 = AnswerText3_Q4;
                        int Responses_3_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText3_Q4).Count();
                        ViewBag.Responses_3_Q4 = Responses_3_Q4;
                        ViewBag.Percentage_3_Q4 = (Responses_3_Q4 * 100) / ResponsesForQ;

                        string AnswerText4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q4 = AnswerText4_Q4;
                        int Responses_4_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText4_Q4).Count();
                        ViewBag.Responses_4_Q4 = Responses_4_Q4;
                        ViewBag.Percentage_4_Q4 = (Responses_4_Q4 * 100) / ResponsesForQ;

                    }
                    else if (Count4 == 5)
                    {
                        string AnswerText1_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q4 = AnswerText1_Q4;
                        int Responses_1_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText1_Q4).Count();
                        ViewBag.Responses_1_Q4 = Responses_1_Q4;
                        ViewBag.Percentage_1_Q4 = (Responses_1_Q4 * 100) / ResponsesForQ;

                        string AnswerText2_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q4 = AnswerText2_Q4;
                        int Responses_2_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText2_Q4).Count();
                        ViewBag.Responses_2_Q4 = Responses_2_Q4;
                        ViewBag.Percentage_2_Q4 = (Responses_2_Q4 * 100) / ResponsesForQ;

                        string AnswerText3_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q4 = AnswerText3_Q4;
                        int Responses_3_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText3_Q4).Count();
                        ViewBag.Responses_3_Q4 = Responses_3_Q4;
                        ViewBag.Percentage_3_Q4 = (Responses_3_Q4 * 100) / ResponsesForQ;

                        string AnswerText4_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q4 = AnswerText4_Q4;
                        int Responses_4_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText4_Q4).Count();
                        ViewBag.Responses_4_Q4 = Responses_4_Q4;
                        ViewBag.Percentage_4_Q4 = (Responses_4_Q4 * 100) / ResponsesForQ;

                        string AnswerText5_Q4 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq4 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q4 = AnswerText5_Q4;
                        int Responses_5_Q4 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq4 && X.Question_Answer == AnswerText5_Q4).Count();
                        ViewBag.Responses_5_Q4 = Responses_5_Q4;
                        ViewBag.Percentage_5_Q4 = (Responses_5_Q4 * 100) / ResponsesForQ;
                    }
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

                if (_StyleType5 == "Multiple Select list")
                {

                    //List of answers that are all multiselect
                    List<Person_Questionnaire_Result> v = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5).ToList();

                    //Going to put everything into this list to be used to compare against possible answers
                    List<string> s = new List<string>();

                    foreach (var item in v)
                    {
                        string c = item.Question_Answer.ToString();
                        string[] items = c.Split(',');
                        s.AddRange(items);
                    }

                    if (Count5 == 1)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q5)
                            {
                                Responses_1_Q5++;
                            }
                        }
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / s.Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;
                    }
                    else if (Count5 == 2)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q5)
                            {
                                Responses_1_Q5++;
                            }
                        }
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / s.Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;


                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q5)
                            {
                                Responses_2_Q5++;
                            }
                        }
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / s.Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;
                    }
                    else if (Count5 == 3)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q5)
                            {
                                Responses_1_Q5++;
                            }
                        }
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / s.Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;


                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q5)
                            {
                                Responses_2_Q5++;
                            }
                        }
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / s.Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;


                        string AnswerText3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q5 = AnswerText3_Q5;
                        int Responses_3_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q5)
                            {
                                Responses_3_Q5++;
                            }
                        }
                        ViewBag.Percentage_3_Q5 = (Responses_3_Q5 * 100) / s.Count();
                        ViewBag.Responses_3_Q5 = Responses_3_Q5;
                    }
                    else if (Count5 == 4)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q5)
                            {
                                Responses_1_Q5++;
                            }
                        }
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / s.Count();

                        ViewBag.Responses_1_Q5 = Responses_1_Q5;


                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q5)
                            {
                                Responses_2_Q5++;
                            }
                        }
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / s.Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;


                        string AnswerText3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q5 = AnswerText3_Q5;
                        int Responses_3_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q5)
                            {
                                Responses_3_Q5++;
                            }
                        }
                        ViewBag.Percentage_3_Q5 = (Responses_3_Q5 * 100) / s.Count();
                        ViewBag.Responses_3_Q5 = Responses_3_Q5;


                        string AnswerText4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q5 = AnswerText4_Q5;
                        int Responses_4_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q5)
                            {
                                Responses_4_Q5++;
                            }
                        }
                        ViewBag.Percentage_4_Q5 = (Responses_4_Q5 * 100) / s.Count();
                        ViewBag.Responses_4_Q5 = Responses_4_Q5;
                    }
                    else if (Count5 == 5)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText1_Q5)
                            {
                                Responses_1_Q5++;
                            }
                        }
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / s.Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;


                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText2_Q5)
                            {
                                Responses_2_Q5++;
                            }
                        }
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / s.Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;


                        string AnswerText3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q5 = AnswerText3_Q5;
                        int Responses_3_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText3_Q5)
                            {
                                Responses_3_Q5++;
                            }
                        }
                        ViewBag.Percentage_3_Q5 = (Responses_3_Q5 * 100) / s.Count();
                        ViewBag.Responses_3_Q5 = Responses_3_Q5;


                        string AnswerText4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q5 = AnswerText4_Q5;
                        int Responses_4_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText4_Q5)
                            {
                                Responses_4_Q5++;
                            }
                        }
                        ViewBag.Percentage_4_Q5 = (Responses_4_Q5 * 100) / s.Count();
                        ViewBag.Responses_4_Q5 = Responses_4_Q5;



                        string AnswerText5_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q5 = AnswerText5_Q5;
                        int Responses_5_Q5 = 0;
                        foreach (var t in s)
                        {
                            if (t == AnswerText5_Q5)
                            {
                                Responses_5_Q5++;
                            }
                        }
                        ViewBag.Percentage_5_Q5 = (Responses_5_Q5 * 100) / s.Count();
                        ViewBag.Responses_5_Q5 = Responses_5_Q5;
                    }
                }
                else
                {
                    if (Count5 == 0)
                    {//Free Text
                        List<string> Free_text_responses = new List<string>();
                        Free_text_responses.AddRange(db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5).Select(Y => Y.Question_Answer));
                        ViewBag.Free_text_responses_Q5 = Free_text_responses;
                        ViewBag.Free_text_responses_Q5_Count = Free_text_responses.Count();
                    }
                    else if (Count5 == 1)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText1_Q5).Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / ResponsesForQ;
                    }
                    else if (Count5 == 2)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText1_Q5).Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / ResponsesForQ;

                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText2_Q5).Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / ResponsesForQ;

                    }
                    else if (Count5 == 3)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText1_Q5).Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / ResponsesForQ;

                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText2_Q5).Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / ResponsesForQ;

                        string AnswerText3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q5 = AnswerText3_Q5;
                        int Responses_3_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText3_Q5).Count();
                        ViewBag.Responses_3_Q5 = Responses_3_Q5;
                        ViewBag.Percentage_3_Q5 = (Responses_3_Q5 * 100) / ResponsesForQ;
                    }
                    else if (Count5 == 4)
                    {

                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText1_Q5).Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / ResponsesForQ;

                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText2_Q5).Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / ResponsesForQ;

                        string AnswerText3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q5 = AnswerText3_Q5;
                        int Responses_3_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText3_Q5).Count();
                        ViewBag.Responses_3_Q5 = Responses_3_Q5;
                        ViewBag.Percentage_3_Q5 = (Responses_3_Q5 * 100) / ResponsesForQ;

                        string AnswerText4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q5 = AnswerText4_Q5;
                        int Responses_4_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText4_Q5).Count();
                        ViewBag.Responses_4_Q5 = Responses_4_Q5;
                        ViewBag.Percentage_4_Q5 = (Responses_4_Q5 * 100) / ResponsesForQ;

                    }
                    else if (Count5 == 5)
                    {
                        string AnswerText1_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 1).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer1_Q5 = AnswerText1_Q5;
                        int Responses_1_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText1_Q5).Count();
                        ViewBag.Responses_1_Q5 = Responses_1_Q5;
                        ViewBag.Percentage_1_Q5 = (Responses_1_Q5 * 100) / ResponsesForQ;

                        string AnswerText2_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 2).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer2_Q5 = AnswerText2_Q5;
                        int Responses_2_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText2_Q5).Count();
                        ViewBag.Responses_2_Q5 = Responses_2_Q5;
                        ViewBag.Percentage_2_Q5 = (Responses_2_Q5 * 100) / ResponsesForQ;

                        string AnswerText3_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 3).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer3_Q5 = AnswerText3_Q5;
                        int Responses_3_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText3_Q5).Count();
                        ViewBag.Responses_3_Q5 = Responses_3_Q5;
                        ViewBag.Percentage_3_Q5 = (Responses_3_Q5 * 100) / ResponsesForQ;

                        string AnswerText4_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 4).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer4_Q5 = AnswerText4_Q5;
                        int Responses_4_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText4_Q5).Count();
                        ViewBag.Responses_4_Q5 = Responses_4_Q5;
                        ViewBag.Percentage_4_Q5 = (Responses_4_Q5 * 100) / ResponsesForQ;

                        string AnswerText5_Q5 = db.Possible_Answer.Where(X => X.Question_Seq == _question_Seq5 && X.Display_Order == 5).Select(Y => Y.Answer_Text).Single();
                        ViewBag.PossibleAnswer5_Q5 = AnswerText5_Q5;
                        int Responses_5_Q5 = db.Person_Questionnaire_Result.Where(X => X.Questionnaire_ID == id && X.Question_Seq == _question_Seq5 && X.Question_Answer == AnswerText5_Q5).Count();
                        ViewBag.Responses_5_Q5 = Responses_5_Q5;
                        ViewBag.Percentage_5_Q5 = (Responses_5_Q5 * 100) / ResponsesForQ;
                    }
                }
            }

                if (questionnaire.Person_ID_Involved != null)
                {
                    ViewBag.Person_Involved = db.Registered_Person.Where(X => X.Person_ID == questionnaire.Person_ID_Involved).Select(Y => Y.Person_Name).Single();
                }
                else
                {
                    ViewBag.Person_Involved = "None";
                }

                // ----------Venue_Booking_Seq
                if (questionnaire.Venue_Booking_Seq != null)
                {
                    Venue_Booking selectedBooking = db.Venue_Booking.Find(questionnaire.Venue_Booking_Seq);
                    ViewBag.Selected_Training_Session = Convert.ToString(selectedBooking.DateTime_From) + " - " + Convert.ToString(selectedBooking.DateTime_To);
                    ViewBag.Training_Topic = db.Topics.Where(X => X.Topic_Seq == selectedBooking.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                }
                else
                {
                    ViewBag.Selected_Training_Session = "None";
                }
                // ----------Venue
                if (questionnaire.Venue_ID != null)
                {
                    Venue myVenue = db.Venues.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID && X.Venue_ID == questionnaire.Venue_ID).Single();

                    ViewBag.VenueName = myVenue.Venue_Name;
                    ViewBag.BuildingName = db.Buildings.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID).Select(Y => Y.Building_Name).Single();
                    ViewBag.BuildingFloor = db.Building_Floor.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID).Select(Y => Y.Floor_Name).Single();
                    ViewBag.Campus = db.Campus.Where(X => X.Campus_ID == questionnaire.Campus_ID).Select(Y => Y.Campus_Name).Single();
                }
                else
                {
                    ViewBag.VenueName = "None";
                }

                return View("Questionnaire_Results");
            }
        


        [Authorize]
        public ActionResult Answering_Questionnaire(int id)
        {
            if (db.Person_Questionnaire.Where(X => X.Questionnaire_ID == id && X.Person_ID == User.Identity.Name).Count() == 1)
            {
                ViewBag.ErrorMessage = "You have already responded to this questionnaire. Please try another. ";
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                var questionnaires = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now).ToList();
                return View("Respond_to_questionnaire", questionnaires.ToList());
            }
            else
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

                if (questionnaire.Person_ID_Involved != null)
                {
                    ViewBag.Person_Involved = db.Registered_Person.Where(X => X.Person_ID == questionnaire.Person_ID_Involved).Select(Y => Y.Person_Name).Single();
                }
                else
                {
                    ViewBag.Person_Involved = "None";
                }

                // ----------Venue_Booking_Seq
                if (questionnaire.Venue_Booking_Seq != null)
                {
                    Venue_Booking selectedBooking = db.Venue_Booking.Find(questionnaire.Venue_Booking_Seq);
                    ViewBag.Selected_Training_Session = Convert.ToString(selectedBooking.DateTime_From) + " - " + Convert.ToString(selectedBooking.DateTime_To);
                    ViewBag.Training_Topic = db.Topics.Where(X => X.Topic_Seq == selectedBooking.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                }
                else
                {
                    ViewBag.Selected_Training_Session = "None";
                }
                // ----------Venue
                if (questionnaire.Venue_ID != null)
                {
                    Venue myVenue = db.Venues.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID && X.Venue_ID == questionnaire.Venue_ID).Single();

                    ViewBag.VenueName = myVenue.Venue_Name;
                    ViewBag.BuildingName = db.Buildings.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID).Select(Y => Y.Building_Name).Single();
                    ViewBag.BuildingFloor = db.Building_Floor.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID).Select(Y => Y.Floor_Name).Single();
                    ViewBag.Campus = db.Campus.Where(X => X.Campus_ID == questionnaire.Campus_ID).Select(Y => Y.Campus_Name).Single();
                }
                else
                {
                    ViewBag.VenueName = "None";
                }

                return View("Answer_Questionnaire");
            }
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

            if (questionnaire.Person_ID_Involved != null)
            {
                ViewBag.Person_Involved = db.Registered_Person.Where(X => X.Person_ID == questionnaire.Person_ID_Involved).Select(Y => Y.Person_Name).Single();
            }
            else
            {
                ViewBag.Person_Involved = "None";
            }

            // ----------Venue_Booking_Seq
            if (questionnaire.Venue_Booking_Seq != null)
            {
                Venue_Booking selectedBooking = db.Venue_Booking.Find(questionnaire.Venue_Booking_Seq);
                ViewBag.Selected_Training_Session = Convert.ToString(selectedBooking.DateTime_From) + " - " + Convert.ToString(selectedBooking.DateTime_To);
                ViewBag.Training_Topic = db.Topics.Where(X => X.Topic_Seq == selectedBooking.Topic_Seq).Select(Y => Y.Topic_Name).Single();
            }
            else
            {
                ViewBag.Selected_Training_Session = "None";
            }
            // ----------Venue
            if (questionnaire.Venue_ID != null)
            {
                Venue myVenue = db.Venues.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID && X.Venue_ID == questionnaire.Venue_ID).Single();

                ViewBag.VenueName = myVenue.Venue_Name;
                ViewBag.BuildingName = db.Buildings.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID).Select(Y => Y.Building_Name).Single();
                ViewBag.BuildingFloor = db.Building_Floor.Where(X => X.Campus_ID == questionnaire.Campus_ID && X.Building_ID == questionnaire.Building_ID && X.Building_Floor_ID == questionnaire.Building_Floor_ID).Select(Y => Y.Floor_Name).Single();
                ViewBag.Campus = db.Campus.Where(X => X.Campus_ID == questionnaire.Campus_ID).Select(Y => Y.Campus_Name).Single();
            }
            else
            {
                ViewBag.VenueName = "None";
            }

            return View("Preview");
        }

        [Authorize(Roles = "Admin")]
        // GET: Questionnaire/Create
        public ActionResult Create(string Name, int? Topic, string Assessment_Type)
        {

           
                ViewBag.Assessment_Type = Assessment_Type;
            
            ViewBag.Name = Name;
            

            if (Topic == null)
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            }
            else
            {
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic);
            }

            
            return View();
        }


        public bool AreThereDuplicates(string Name, int Questionnaire_ID)
        {
            if (db.Questionnaires.Any(X => X.Name == Name))
            {
                if (db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single() == Name)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return false;
            }
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

        [Authorize(Roles = "Admin")]
        public ActionResult UpdateSelectedVenue(int Venue_ID, int SelectedBuildingFloor, int SelectedBuilding, int SelectedCampus, int Questionnaire_ID)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(Questionnaire_ID);
            questionnaire.Campus_ID = SelectedCampus;
            questionnaire.Building_Floor_ID = SelectedBuildingFloor;
            questionnaire.Building_ID = SelectedBuilding;
            questionnaire.Venue_ID = Venue_ID;

            db.Entry(questionnaire).State = EntityState.Modified;
            db.SaveChanges();

            int CountQuestions = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).Count();

            if (CountQuestions == 0)
            {
                #region Going to questionnaire questions to get new questionnare question data
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.Topic = questionnaire.Topic_Seq;
                ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).ToList();

                ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                ViewBag.Edit_or_New = "Edit";
                #endregion
                return View("Edit_Questionnaire_Questions"); //Changed topic 
            }
            else
            {
                #region Going to questionnaire questions with previously saved data
                // =====Questionnaire questions details =======//
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.Topic = questionnaire.Topic_Seq;
                ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();
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

                IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).ToList();

                ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                ViewBag.Edit_or_New = "Edit";
                // =====Questionnaire questions details =======//
                #endregion
                return View("Edit_Questionnaire_Questions");
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Back_From_VenueAssessment_QQuestions(int SelectedCampus,int SelectedBuilding, int SelectedBuildingFloor, int SelectedVenue, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Topic = Topic;

            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.SelectedBuilding = SelectedBuilding;
            ViewBag.SelectedBuildingFloor = SelectedBuildingFloor;
            ViewBag.SelectedVenue = SelectedVenue;
            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == SelectedCampus).Select(Y => Y.Campus_Name).Single();
            ViewBag.SelectedBuildingName = db.Buildings.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding).Select(Y => Y.Building_Name).Single();
            ViewBag.SelectedBuildingFloorName = db.Building_Floor.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding && X.Building_Floor_ID == SelectedBuildingFloor).Select(Y => Y.Floor_Name).Single();



            ViewBag.Count = db.Venues.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding && X.Building_Floor_ID == SelectedBuildingFloor).Count();
            return View("Venue_Assessment_Venue", db.Venues.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding && X.Building_Floor_ID == SelectedBuildingFloor).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Back_From_Venue(int SelectedBuildingFloor, int SelectedBuilding, int SelectedCampus, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic , string Edit_or_New)
        {

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Topic = Topic;

            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.SelectedBuilding = SelectedBuilding;
            ViewBag.SelectedBuildingFloor = SelectedBuildingFloor;
            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == SelectedCampus).Select(Y => Y.Campus_Name).Single();
            ViewBag.SelectedBuildingName = db.Buildings.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding).Select(Y => Y.Building_Name).Single();
            ViewBag.SelectedBuildingFloorName = db.Building_Floor.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding && X.Building_Floor_ID == SelectedBuildingFloor).Select(Y => Y.Floor_Name).Single();
            ViewBag.Edit_or_New = Edit_or_New;

            ViewBag.Count = db.Building_Floor.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding).Count();
            return View("Venue_Assessment_Building_Floor", db.Building_Floor.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Back_From_BuildingFloors(int SelectedBuilding, int SelectedCampus, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic, string Edit_or_New)
        {

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Topic = Topic;
            
            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.SelectedBuilding = SelectedBuilding;
            ViewBag.Count = db.Buildings.Where(X => X.Campus_ID == SelectedCampus).Count();
            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == SelectedCampus).Select(Y => Y.Campus_Name).Single();
            ViewBag.SelectedBuildingName = db.Buildings.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding).Select(Y => Y.Building_Name).Single();
            ViewBag.Edit_or_New = Edit_or_New;

            return View("Venue_Assessment_Building", db.Buildings.Where(X => X.Campus_ID == SelectedCampus).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Back_From_Buildings(int SelectedCampus, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic, string Edit_or_New)
        {

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Topic = Topic;
            ViewBag.Count = db.Campus.Count();
            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == SelectedCampus).Select(Y => Y.Campus_Name).Single();
            ViewBag.Edit_or_New = Edit_or_New;

            return View("Venue_Assessment_Campus", db.Campus.ToList());
        }

        [Authorize(Roles = "Admin")]
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


        [Authorize(Roles = "Admin")]
        public ActionResult UpdateTrainingSession(int Venue_Booking_Seq, int Questionnaire_ID)
        {
            Venue_Booking existingBooking = db.Venue_Booking.Find(Venue_Booking_Seq);

            Questionnaire questionnaire = db.Questionnaires.Find(Questionnaire_ID);
            questionnaire.Venue_Booking_Seq = Venue_Booking_Seq;

            if (questionnaire.Active_From < existingBooking.DateTime_From)
            {
                questionnaire.Active_From = existingBooking.DateTime_To;
            }

            if (questionnaire.Active_To < existingBooking.DateTime_From)
            {
                questionnaire.Active_To = existingBooking.DateTime_To;
            }

            db.Entry(questionnaire).State = EntityState.Modified;
            db.SaveChanges();

            int CountQuestions = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).Count();

            if (CountQuestions == 0)
            {
                #region Going to questionnaire questions to get new questionnare question data
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.Topic = questionnaire.Topic_Seq;
                ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).ToList();

                ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                ViewBag.Edit_or_New = "Edit";
                #endregion
                return View("Edit_Questionnaire_Questions"); //Changed topic 
            }
            else
            {
                #region Going to questionnaire questions with previously saved data
                // =====Questionnaire questions details =======//
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.Topic = questionnaire.Topic_Seq;
                ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();
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

                IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).ToList();

                ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                ViewBag.Edit_or_New = "Edit";
                // =====Questionnaire questions details =======//
                #endregion
                return View("Edit_Questionnaire_Questions");
            }

            //return View();
        }


        [Authorize(Roles = "Admin")]
        public ActionResult UpdateEmployee(string Employee_ID, int Questionnaire_ID)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(Questionnaire_ID);
            questionnaire.Person_ID_Involved = Employee_ID;

            db.Entry(questionnaire).State = EntityState.Modified;
            db.SaveChanges();

            int CountQuestions = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).Count();

            if (CountQuestions == 0)
            {
                #region Going to questionnaire questions to get new questionnare question data
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.Topic = questionnaire.Topic_Seq;
                ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();
                IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).ToList();

                ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                ViewBag.Edit_or_New = "Edit";
                #endregion
                return View("Edit_Questionnaire_Questions"); //Changed topic 
            }
            else
            {
                #region Going to questionnaire questions with previously saved data
                // =====Questionnaire questions details =======//
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.Topic = questionnaire.Topic_Seq;
                ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).Select(Y => Y.Topic_Name).Single();
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

                IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == questionnaire.Topic_Seq).ToList();

                ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                ViewBag.Edit_or_New = "Edit";
                // =====Questionnaire questions details =======//
                #endregion
                return View("Edit_Questionnaire_Questions");
            }

            //return View();
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Back_From_TrainingSessionAssessment(string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {
            IEnumerable<Venue_Booking> _Venue_Booking = null;
            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Selected_TrainingSession = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Venue_Booking_Seq).Single();
            ViewBag.Topic_For_Training = new SelectList(db.Topics, "Topic_Seq", "Topic_Name");
            ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).Count();
            _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).ToList();

            return View("Training_Session_Assessment", _Venue_Booking);
        }

        [Authorize(Roles = "Admin")]
        public ActionResult Back_From_Employee_Assessment(string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Selected_Employee = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Person_ID_Involved).Single();

            ViewBag.Count = db.Registered_Person.Where(X => X.Person_Type == "Employee").Count();
            return View("Employee_Assessment", db.Registered_Person.Where(X => X.Person_Type == "Employee").ToList());
        }

        [Authorize(Roles = "Admin")]
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
            string Questionnaire_Name = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single();
            db.SaveChanges();
            ViewBag.EditComplete = "Yes";
            ViewBag.EditCompleteMessage = "Questionnaire '" + Questionnaire_Name + "' successfully updated.";
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);


            // -------------------------------Action Log ----------------------------------------//
            string name = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single();
            Person_Session_Action_Log psal = new Person_Session_Action_Log();
            psal.Action_DateTime = DateTime.Now;
            psal.Action_ID = 11;
            psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
            psal.Action_Performed = "Questionnaire: " + name;
            psal.Crud_Operation = "Edit";
            db.Person_Session_Action_Log.Add(psal);
            db.SaveChanges();
            // -------------------------------Action Log ----------------------------------------//



            return View("Index", questionnaires.ToList());
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
            string Questionnaire_Name = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single();
            db.SaveChanges();
            ViewBag.CreateComplete = "Yes";
            ViewBag.CreateCompleteMessage = "Questionnaire '" + Questionnaire_Name + "' successfully created.";
            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);

            // -------------------------------Action Log ----------------------------------------//
            string name = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single();
            Person_Session_Action_Log psal = new Person_Session_Action_Log();
            psal.Action_DateTime = DateTime.Now;
            psal.Action_ID = 11;
            psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
            psal.Action_Performed = "Questionnaire: " + name;
            psal.Crud_Operation = "Create";
            db.Person_Session_Action_Log.Add(psal);
            db.SaveChanges();
            // -------------------------------Action Log ----------------------------------------//


            return View("Index", questionnaires.ToList());

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
            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic).Select(Y => Y.Topic_Name).Single();
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
            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic).Select(Y => Y.Topic_Name).Single();
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

            return View("Edit_Questionnaire_Questions");
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SelectedVenue(int Venue_ID, int SelectedBuildingFloor, int SelectedBuilding, int SelectedCampus, int Questionnaire_ID, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(Questionnaire_ID);
            questionnaire.Campus_ID = SelectedCampus;
            questionnaire.Building_ID = SelectedBuilding;
            questionnaire.Building_Floor_ID = SelectedBuildingFloor;
            questionnaire.Venue_ID = Venue_ID;

            db.Entry(questionnaire).State = EntityState.Modified;
            db.SaveChanges();


            ViewBag.VenueAssessment = "Yes";
            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.SelectedBuilding = SelectedBuilding;
            ViewBag.SelectedBuildingFloor = SelectedBuildingFloor;
            ViewBag.SelectedVenue = Venue_ID;

            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic).ToList();

            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Topic = Topic;
            
            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic).Select(Y => Y.Topic_Name).Single();
            return View("Questionnaire_Questions");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SelectedBuildingFloor(int BuildingFloor_ID, int SelectedBuilding, int SelectedCampus, int Questionnaire_ID, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic, string Edit_or_New)
        {
            ViewBag.Edit_or_New = Edit_or_New;
            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.Questionnaire_ID = Questionnaire_ID;
           
            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.SelectedBuilding = SelectedBuilding;
            ViewBag.SelectedBuildingFloor = BuildingFloor_ID;

            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == SelectedCampus).Select(Y => Y.Campus_Name).Single();
            ViewBag.SelectedBuildingName = db.Buildings.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding).Select(Y => Y.Building_Name).Single();
            ViewBag.SelectedBuildingFloorName = db.Building_Floor.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding && X.Building_Floor_ID == BuildingFloor_ID).Select(Y => Y.Floor_Name).Single();

            ViewBag.Count = db.Venues.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding && X.Building_Floor_ID == BuildingFloor_ID).Count();
            return View("Venue_Assessment_Venue", db.Venues.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == SelectedBuilding && X.Building_Floor_ID == BuildingFloor_ID).ToList());
        }


        [Authorize(Roles = "Admin")]
        public ActionResult SelectedBuilding(int Building_ID, int SelectedCampus, int Questionnaire_ID, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic, string Edit_or_New)
        {
            ViewBag.Edit_or_New = Edit_or_New;
            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.Questionnaire_ID = Questionnaire_ID;
            ViewBag.Count = db.Building_Floor.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == Building_ID).Count();
            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.SelectedBuilding = Building_ID;
            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == SelectedCampus).Select(Y => Y.Campus_Name).Single();
            ViewBag.SelectedBuildingName = db.Buildings.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == Building_ID).Select(Y => Y.Building_Name).Single();

            return View("Venue_Assessment_Building_Floor", db.Building_Floor.Where(X => X.Campus_ID == SelectedCampus && X.Building_ID == Building_ID).ToList());
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SelectedCampus(int Campus_ID, int Questionnaire_ID, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic, string Edit_or_New)
        {
            ViewBag.Edit_or_New = Edit_or_New;
            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.Questionnaire_ID = Questionnaire_ID;
            ViewBag.Count = db.Buildings.Where(X => X.Campus_ID == Campus_ID).Count();
            ViewBag.SelectedCampus = Campus_ID;
            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == Campus_ID).Select(Y => Y.Campus_Name).Single();
            return View("Venue_Assessment_Building", db.Buildings.Where(X => X.Campus_ID == Campus_ID).ToList());
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
                ViewBag.Duplicate = "Yes";
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

                    newQuestionnaire.Active_To = Active_To.AddHours(23).AddMinutes(59).AddSeconds(59);
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
                    ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic).Select(Y => Y.Topic_Name).Single();
                    return View("Questionnaire_Questions");
                }
                else if (Assessment_Type == "Venue")
                {
                    Questionnaire newQuestionnaire = new Questionnaire();
                    newQuestionnaire.Name = Name;
                    newQuestionnaire.Description = Description;
                    newQuestionnaire.Active_From = Active_From;
                    newQuestionnaire.Active_To = Active_To.AddHours(23).AddMinutes(59).AddSeconds(59);
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

                    ViewBag.Name = Name;
                    ViewBag.Description = Description;
                    ViewBag.Active_From = Active_From;
                    ViewBag.Active_To = Active_To;
                    ViewBag.Assessment_Type = Assessment_Type;
                    ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                    ViewBag.Topic = Topic;
                    ViewBag.Count = db.Campus.Count();
                    return View("Venue_Assessment_Campus", db.Campus.ToList());

                }
                else if (Assessment_Type == "Employee")
                {
                    Questionnaire newQuestionnaire = new Questionnaire();
                    newQuestionnaire.Name = Name;
                    newQuestionnaire.Description = Description;
                    newQuestionnaire.Active_From = Active_From;
                    newQuestionnaire.Active_To = Active_To.AddHours(23).AddMinutes(59).AddSeconds(59);
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

                    ViewBag.Name = Name;
                    ViewBag.Description = Description;
                    ViewBag.Active_From = Active_From;
                    ViewBag.Active_To = Active_To;
                    ViewBag.Assessment_Type = Assessment_Type;
                    ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                    ViewBag.Topic = Topic;
                    ViewBag.Count = db.Registered_Person.Where(X => X.Person_Type == "Employee").Count();
                    return View("Employee_Assessment", db.Registered_Person.Where(X => X.Person_Type == "Employee").ToList());
                }
                else if (Assessment_Type == "Training Session")
                {
                    Questionnaire newQuestionnaire = new Questionnaire();
                    newQuestionnaire.Name = Name;
                    newQuestionnaire.Description = Description;
                    newQuestionnaire.Active_From = Active_From;
                    newQuestionnaire.Active_To = Active_To.AddHours(23).AddMinutes(59).AddSeconds(59);
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

                    ViewBag.Name = Name;
                    ViewBag.Description = Description;
                    ViewBag.Active_From = Active_From;
                    ViewBag.Active_To = Active_To;
                    ViewBag.Assessment_Type = Assessment_Type;
                    ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                    ViewBag.Topic = Topic;
                    ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).Count();
                    ViewBag.Topic_For_Training = new SelectList(db.Topics, "Topic_Seq", "Topic_Name");
                    return View("Training_Session_Assessment", db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).ToList().OrderByDescending(X => X.DateTime_From));
                }
                else
                {
                    return RedirectToAction("Index");
                }
            }
        }
        [Authorize(Roles = "Admin")]
        public ActionResult SelectedTrainingSession(int Venue_Booking_Seq, int Questionnaire_ID, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {
            Venue_Booking existingBooking = db.Venue_Booking.Find(Venue_Booking_Seq);

            Questionnaire questionnaire = db.Questionnaires.Where(X => X.Name == Name).Single();
            questionnaire.Venue_Booking_Seq = Venue_Booking_Seq;

            if (questionnaire.Active_From < existingBooking.DateTime_From)
            {
                questionnaire.Active_From = existingBooking.DateTime_To;
            }

            if (questionnaire.Active_To < existingBooking.DateTime_From)
            {
                questionnaire.Active_To = (existingBooking.DateTime_To).AddDays(1);
            }


            db.Entry(questionnaire).State = EntityState.Modified;
            db.SaveChanges();

            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic).ToList();

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic).Select(Y => Y.Topic_Name).Single();
            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
            ViewBag.Questionnaire_ID = Questionnaire_ID;
            ViewBag.TrainingSessionAssessment = "Yes";
            return View("Questionnaire_Questions");
        }

        [Authorize(Roles = "Admin")]
        public ActionResult SelectedEmployee(string Employee_ID, int Questionnaire_ID, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {
            Questionnaire questionnaire = db.Questionnaires.Where(X => X.Name == Name).Single();
            questionnaire.Person_ID_Involved = Employee_ID;

            db.Entry(questionnaire).State = EntityState.Modified;
            db.SaveChanges();

            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic).ToList();

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic).Select(Y => Y.Topic_Name).Single();
            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
            ViewBag.Questionnaire_ID = Questionnaire_ID;
            ViewBag.EmployeeAssessment = "Yes";
            return View("Questionnaire_Questions");
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
            ViewBag.Assessment_Type = questionnaire.Assessment_Type;
            ViewBag.Active_To = questionnaire.Active_To;
            ViewBag.Questionnaire_ID = questionnaire.Questionnaire_ID;

            int Count = db.Person_Questionnaire.Where(X => X.Questionnaire_ID == questionnaire.Questionnaire_ID).Count();

            if (Count > 0)
            {
                ViewBag.CannotEdit = "true";
                ViewBag.CannotEditError = "Updates to this questionnaire have been limited because the questionnaire has been responded to.";
            }

            return View("Edit", questionnaire);

        }



        [Authorize(Roles = "Admin")]
        public ActionResult LimitedEdit(string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic_Seq, int Questionnaire_ID)
        {
            if (AreThereDuplicates(Name, Questionnaire_ID))
            {
                #region Throw error for duplicate
                ViewBag.Name = Name;
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
                ViewBag.Description = Description;
                ViewBag.Active_From = Active_From;
                ViewBag.Assess = Assessment_Type;
                ViewBag.Active_To = Active_To;
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.DuplicateError = "A questionnaire with this name already exists. Please choose another name for the questionnaire.";

                ViewBag.CannotEdit = "true";
                ViewBag.CannotEditError = "This questionnaire has been responded to, therefore, limited editing options are available.";
                return View("Edit");
                #endregion
            }
            else
            {
                Questionnaire questionnaire = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Single();
                questionnaire.Name = Name;
                questionnaire.Description = Description;
                questionnaire.Active_From = Active_From;
                questionnaire.Active_To = Active_To.AddHours(23).AddMinutes(59).AddSeconds(59);
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
                db.SaveChanges();

                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
                var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);
                ViewBag.DeleteComplete = "No";
                ViewBag.CreateComplete = "No";
                ViewBag.EditComplete = "Yes";
                ViewBag.EditCompleteMessage = "Questionnaire '" + Name + "' successfully updated.";

                // -------------------------------Action Log ----------------------------------------//
                string name = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Name).Single();
                Person_Session_Action_Log psal = new Person_Session_Action_Log();
                psal.Action_DateTime = DateTime.Now;
                psal.Action_ID = 11;
                psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
                psal.Action_Performed = "Questionnaire: " + name;
                psal.Crud_Operation = "Edit";
                db.Person_Session_Action_Log.Add(psal);
                db.SaveChanges();
                // -------------------------------Action Log ----------------------------------------//
                return View("Index",questionnaires.ToList());
            }
        }
        [Authorize(Roles = "Admin")]
        // POST: Questionnaire/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.      
        public ActionResult Editing(string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic_Seq, int Questionnaire_ID)
        {
            #region Check if assessment type has been changed
            bool Assessment_Changed = false;

            if (db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Assessment_Type).Single() == Assessment_Type)
            {
                Assessment_Changed = false;
            }
            else
            {
                Assessment_Changed = true;
            }
            #endregion

            if (AreThereDuplicates(Name, Questionnaire_ID))
            {
                #region Throw error for duplicate
                ViewBag.Name = Name;
                ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name", Topic_Seq);
                ViewBag.Description = Description;
                ViewBag.Active_From = Active_From;
                ViewBag.Assess = Assessment_Type;
                ViewBag.Active_To = Active_To;
                ViewBag.Questionnaire_ID = Questionnaire_ID;
                ViewBag.DuplicateError = "A questionnaire with this name already exists. Please choose another name for the questionnaire.";
                return View("Edit");
                #endregion
            }
            else
            {
                #region There are no duplicates
                if (db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Select(Y => Y.Topic_Seq).Single() == Topic_Seq)
                {
                    #region Topic has not been changed
                    //Topic has not been changed
                    //its fine update
                    Questionnaire questionnaire = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Single();
                    questionnaire.Name = Name;
                    questionnaire.Description = Description;
                    questionnaire.Active_From = Active_From;
                    questionnaire.Active_To = Active_To.AddHours(23).AddMinutes(59).AddSeconds(59);

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
                    db.SaveChanges();

                    if (Assessment_Changed == false)// Assessment type not changed
                    {
                        #region Going to view with previously saved data to be edited
                        if (Assessment_Type == "Employee")
                        {
                            #region Going to employee with previously saved data to be edited
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Selected_Employee = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Person_ID_Involved).Single();
                            ViewBag.Edit_or_New = "Edit";
                            ViewBag.Count = db.Registered_Person.Where(X => X.Person_Type == "Employee").Count();
                            #endregion
                            return View("Employee_Assessment", db.Registered_Person.Where(X => X.Person_Type == "Employee").ToList());
                        }
                        else if (Assessment_Type == "Other")
                        {
                            #region Going to questionnaire questions with previously saved data
                            // =====Questionnaire questions details =======//
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Seq).Select(Y => Y.Topic_Name).Single();
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

                            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                            ViewBag.Edit_or_New = "Edit";
                            // =====Questionnaire questions details =======//
                            #endregion
                            return View("Edit_Questionnaire_Questions");
                        }
                        else if (Assessment_Type == "Venue")
                        {

                            #region Going to venue with previously saved data to be edited
                            Questionnaire b = db.Questionnaires.Find(Questionnaire_ID);

                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Count = db.Campus.Count();
                            ViewBag.Edit_or_New = "Edit";
                            ViewBag.Edit_Venue_Assessment = "Yes";

                            ViewBag.SelectedCampus = b.Campus_ID;
                            ViewBag.SelectedBuilding = b.Building_ID;
                            ViewBag.SelectedBuildingFloor = b.Building_Floor_ID;
                            ViewBag.SelectedVenue = b.Venue_ID;
                            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == b.Campus_ID).Select(Y => Y.Campus_Name).Single();
                            ViewBag.SelectedBuildingName = db.Buildings.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID).Select(Y => Y.Building_Name).Single();
                            ViewBag.SelectedBuildingFloorName = db.Building_Floor.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID).Select(Y => Y.Floor_Name).Single();
                            ViewBag.SelectedVenueName = db.Venues.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID && X.Venue_ID == b.Venue_ID).Select(Y => Y.Venue_Name).Single();

                            #endregion
                            ViewBag.Count = db.Venues.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID).Count();
                            return View("Venue_Assessment_Venue", db.Venues.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID).ToList());

                        }
                        else if (Assessment_Type == "Training Session")
                        {
                            #region Going to Training Session with previously saved data to be edited
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Selected_TrainingSession = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Venue_Booking_Seq).Single();
                            ViewBag.Edit_or_New = "Edit";

                            IEnumerable<Venue_Booking> _Venue_Booking = null;
                            ViewBag.Topic_For_Training = new SelectList(db.Topics, "Topic_Seq", "Topic_Name");
                            ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).Count();
                            _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).ToList();
                            #endregion
                            return View("Training_Session_Assessment", _Venue_Booking);
                        }
                        #endregion
                    }
                    else // Assessment type has been changed
                    {
                        #region Remove previous saved assessment type data
                        Questionnaire q = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Single();
                        q.Venue_ID = null;
                        q.Venue_Booking_Seq = null;
                        q.Building_Floor_ID = null;
                        q.Building_ID = null;
                        q.Campus_ID = null;
                        q.Person_ID_Involved = null;
                        db.Entry(q).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                        #region Go to view and get new assessment type data
                        if (Assessment_Type == "Employee")
                        {
                            #region Going to employee for new employee data
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Edit_or_New = "Edit";
                            ViewBag.Count = db.Registered_Person.Where(X => X.Person_Type == "Employee").Count();
                            #endregion
                            return View("Employee_Assessment", db.Registered_Person.Where(X => X.Person_Type == "Employee").ToList());
                        }
                        else if (Assessment_Type == "Other")
                        {
                            #region Going to questionnaire questions with previously saved data
                            // =====Questionnaire questions details =======//
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Seq).Select(Y => Y.Topic_Name).Single();
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
                            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Seq).Select(Y => Y.Topic_Name).Single();
                            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList();

                            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                            ViewBag.Edit_or_New = "Edit";
                            // =====Questionnaire questions details =======//
                            #endregion
                            return View("Edit_Questionnaire_Questions");
                        }
                        else if (Assessment_Type == "Venue")
                        {
                            #region Going to venue to get new venue data
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Count = db.Campus.Count();
                            ViewBag.Edit_or_New = "Edit";
                            #endregion
                            return View("Venue_Assessment_Campus", db.Campus.ToList());
                        }
                        else if (Assessment_Type == "Training Session")
                        {
                            #region Going to Training Session for new training session data
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Edit_or_New = "Edit";

                            IEnumerable<Venue_Booking> _Venue_Booking = null;
                            ViewBag.Topic_For_Training = new SelectList(db.Topics, "Topic_Seq", "Topic_Name");
                            ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).Count();
                            _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).ToList();
                            #endregion
                            return View("Training_Session_Assessment", _Venue_Booking);
                        }
                        #endregion

                    }
                    return View(); // If no assessment type is found (won't happen)
                    #endregion
                }
                else //Topic has been changed - questions must be removed.
                {
                    #region Topic has been changed
                    db.Questionnaire_Questions.RemoveRange(db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == Questionnaire_ID).ToList());
                    db.SaveChanges();

                    if (Assessment_Changed == false)// Assessment type not changed
                    {
                        #region Going to views with previously saved data to be edited
                        if (Assessment_Type == "Employee")
                        {
                            #region Going to employee with previously saved data to be edited
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Selected_Employee = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Person_ID_Involved).Single();
                            ViewBag.Edit_or_New = "Edit";
                            ViewBag.Count = db.Registered_Person.Where(X => X.Person_Type == "Employee").Count();
                            #endregion
                            return View("Employee_Assessment", db.Registered_Person.Where(X => X.Person_Type == "Employee").ToList());
                        }
                        else if (Assessment_Type == "Other")
                        {
                            #region Going to questionnaire questions to get new questionnare question data
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Seq).Select(Y => Y.Topic_Name).Single();
                            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList();

                            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                            ViewBag.Edit_or_New = "New";
                            #endregion
                            return View("Edit_Questionnaire_Questions"); //Changed topic                                 
                        }
                        else if (Assessment_Type == "Venue")
                        {

                            #region Going to venue with previously saved data to be edited

                            Questionnaire b = db.Questionnaires.Find(Questionnaire_ID);

                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Count = db.Campus.Count();
                            ViewBag.Edit_or_New = "Edit";
                            ViewBag.Edit_Venue_Assessment = "Yes";

                            ViewBag.SelectedCampus = b.Campus_ID;
                            ViewBag.SelectedBuilding = b.Building_ID;
                            ViewBag.SelectedBuildingFloor = b.Building_Floor_ID;
                            ViewBag.SelectedVenue = b.Venue_ID;
                            ViewBag.SelectedCampusName = db.Campus.Where(X => X.Campus_ID == b.Campus_ID).Select(Y => Y.Campus_Name).Single();
                            ViewBag.SelectedBuildingName = db.Buildings.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID).Select(Y => Y.Building_Name).Single();
                            ViewBag.SelectedBuildingFloorName = db.Building_Floor.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID).Select(Y => Y.Floor_Name).Single();
                            ViewBag.SelectedVenueName = db.Venues.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID && X.Venue_ID == b.Venue_ID).Select(Y => Y.Venue_Name).Single();
                            #endregion
                            ViewBag.Count = db.Venues.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID).Count();
                            return View("Venue_Assessment_Venue", db.Venues.Where(X => X.Campus_ID == b.Campus_ID && X.Building_ID == b.Building_ID && X.Building_Floor_ID == b.Building_Floor_ID).ToList());

                        }
                        else if (Assessment_Type == "Training Session")
                        {
                            #region Going to Training Session with previously saved data to be edited
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Selected_TrainingSession = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Venue_Booking_Seq).Single();
                            ViewBag.Edit_or_New = "Edit";

                            IEnumerable<Venue_Booking> _Venue_Booking = null;
                            ViewBag.Topic_For_Training = new SelectList(db.Topics, "Topic_Seq", "Topic_Name");
                            ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).Count();
                            _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).ToList();
                            #endregion
                            return View("Training_Session_Assessment", _Venue_Booking);
                        }
                        #endregion
                    }
                    else // Assessment type has been changed
                    {
                        #region Going to views to get new assessment type data

                        #region Remove previously saved assessment type data                            
                        Questionnaire q = db.Questionnaires.Where(X => X.Questionnaire_ID == Questionnaire_ID).Single();
                        q.Venue_ID = null;
                        q.Venue_Booking_Seq = null;
                        q.Building_Floor_ID = null;
                        q.Building_ID = null;
                        q.Campus_ID = null;
                        q.Person_ID_Involved = null;
                        db.Entry(q).State = EntityState.Modified;
                        db.SaveChanges();
                        #endregion

                        //Go to view and get new assessment type data
                        if (Assessment_Type == "Employee")
                        {
                            #region Go to view to get new employee data
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Edit_or_New = "Edit";
                            ViewBag.Count = db.Registered_Person.Where(X => X.Person_Type == "Employee").Count();
                            #endregion
                            return View("Employee_Assessment", db.Registered_Person.Where(X => X.Person_Type == "Employee").ToList());

                        }
                        else if (Assessment_Type == "Other")
                        {
                            #region Go to questionnaire questions to get new questionnaire question details
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.TopicName = db.Question_Topic.Where(X => X.Topic_Seq == Topic_Seq).Select(Y => Y.Topic_Name).Single();
                            IEnumerable<Question_Bank> questions = db.Question_Bank.Where(X => X.Topic_Seq == Topic_Seq).ToList();

                            ViewBag.Questions = new SelectList(questions, "Question_Text", "Question_Text");
                            ViewBag.Edit_or_New = "New";
                            #endregion
                            return View("Edit_Questionnaire_Questions");
                        }
                        else if (Assessment_Type == "Venue")
                        {

                            #region Going to venue to get new venue data
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Count = db.Campus.Count();
                            ViewBag.Edit_or_New = "Edit";
                            #endregion
                            return View("Venue_Assessment_Campus", db.Campus.ToList());

                        }
                        else if (Assessment_Type == "Training Session")
                        {
                            #region Going to Training Session for new training session data
                            ViewBag.Name = Name;
                            ViewBag.Description = Description;
                            ViewBag.Active_From = Active_From;
                            ViewBag.Active_To = Active_To;
                            ViewBag.Assessment_Type = Assessment_Type;
                            ViewBag.Topic = Topic_Seq;
                            ViewBag.Questionnaire_ID = Questionnaire_ID;
                            ViewBag.Edit_or_New = "Edit";

                            IEnumerable<Venue_Booking> _Venue_Booking = null;
                            ViewBag.Topic_For_Training = new SelectList(db.Topics, "Topic_Seq", "Topic_Name");
                            ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).Count();
                            _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).ToList();
                            #endregion
                            return View("Training_Session_Assessment", _Venue_Booking);
                        }
                        #endregion
                    }
                    return View(); // If no assessment type is found (won't happen)
                    #endregion
                }
                #endregion
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

            ViewBag.Responded = db.Person_Questionnaire.Where(X => X.Questionnaire_ID == id).Count().ToString();

            return View(questionnaire);
        }
        [Authorize(Roles = "Admin")]
        // POST: Questionnaire/Delete/5        
        public ActionResult DeleteConfirmed(int id)
        {
            Questionnaire questionnaire = db.Questionnaires.Find(id);

            // -------------------------------Action Log ----------------------------------------//
            string name = db.Questionnaires.Where(X => X.Questionnaire_ID == id).Select(Y => Y.Name).Single();
            Person_Session_Action_Log psal = new Person_Session_Action_Log();
            psal.Action_DateTime = DateTime.Now;
            psal.Action_ID = 11;
            psal.Session_ID = db.Person_Session_Log.Select(Y => Y.Session_ID).Max();
            //psal.Ac = "Delete";
            psal.Crud_Operation = "Delete";
            psal.Action_Performed = "Questionnaire: " + name;
            db.Person_Session_Action_Log.Add(psal);
            db.SaveChanges();
            // -------------------------------Action Log ----------------------------------------//

            List<Questionnaire_Questions> remove_List = db.Questionnaire_Questions.Where(X => X.Questionnaire_ID == id).ToList();

            db.Questionnaire_Questions.RemoveRange(remove_List);

            db.Questionnaires.Remove(questionnaire);
            db.SaveChanges();

            ViewBag.Topic = new SelectList(db.Question_Topic, "Topic_Seq", "Topic_Name");
            var questionnaires = db.Questionnaires.Include(q => q.Question_Topic);
            ViewBag.DeleteCompleteMessage = "Questionnaire '" + questionnaire.Name + "' successfully deleted";
            ViewBag.DeleteComplete = "Yes";




            return View("Index", questionnaires.ToList());


        }

        [Authorize(Roles = "Admin")]
        public ActionResult Respond_To_Random()
        {
            var selection = db.Questionnaires.Where(X => X.Active_To >= DateTime.Now && X.Active_From <= DateTime.Now);

            Questionnaire randomQuestionnaire = selection.OrderBy(c => c.Active_From).Skip(new Random().Next(selection.Count())).First();
            int id = randomQuestionnaire.Questionnaire_ID;

            return RedirectToAction("Answering_Questionnaire", new { id });
        }
        [Authorize(Roles = "Admin")]
        public ActionResult Update_Selected_Venue( int SelectedCampus, string Name, string Description, DateTime Active_From, DateTime Active_To,  string Assessment_Type, int  Topic )
        {
            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Topic = Topic;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();


            ViewBag.Edit_or_New = "Edit";
            ViewBag.SelectedCampus = SelectedCampus;
            ViewBag.Count = db.Campus.Count();
            return View("Venue_Assessment_Campus", db.Campus.ToList());

        }

        [Authorize(Roles = "Admin")]
        public ActionResult TrainingSession_Search(DateTime FromDate, int? Selected_TrainingSession, int? Topic_For_Training, string Name, string Description, DateTime Active_From, DateTime Active_To, string Assessment_Type, int Topic)
        {

            ViewBag.Name = Name;
            ViewBag.Description = Description;
            ViewBag.Active_From = Active_From;
            ViewBag.Active_To = Active_To;
            ViewBag.Assessment_Type = Assessment_Type;
            ViewBag.Questionnaire_ID = db.Questionnaires.Where(X => X.Name == Name).Select(Y => Y.Questionnaire_ID).Single();
            ViewBag.Topic = Topic;
            ViewBag.Selected_TrainingSession = Selected_TrainingSession;
            ViewBag.Topic_For_Training = new SelectList(db.Topics, "Topic_Seq", "Topic_Name", Topic);
            IEnumerable<Venue_Booking> _Venue_Booking = null;

            if (FromDate == Convert.ToDateTime("11/11/1111"))
            {
                ViewBag.From = null;
            }
            else
            {
                ViewBag.From = FromDate;
            }



            if (FromDate == Convert.ToDateTime("11/11/1111") && Topic_For_Training == null)
            {
                ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).Count();
                _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1).ToList();

            }
            else if (FromDate != Convert.ToDateTime("11/11/1111") && Topic_For_Training == null)
            {
                ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1 && X.DateTime_From.Month == FromDate.Month && X.DateTime_From.Day == FromDate.Day).Count();
                _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1 && X.DateTime_From.Month == FromDate.Month && X.DateTime_From.Day == FromDate.Day).ToList();

            }
            else if (FromDate == Convert.ToDateTime("11/11/1111") && Topic_For_Training != null)
            {
                ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1 && X.Topic_Seq == Topic_For_Training).Count();
                _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1 && X.Topic_Seq == Topic_For_Training).ToList();

            }
            else if (FromDate != Convert.ToDateTime("11/11/1111") && Topic_For_Training != null)
            {
                ViewBag.Count = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1 && X.Topic_Seq == Topic_For_Training && X.DateTime_From.Month == FromDate.Month && X.DateTime_From.Day == FromDate.Day).Count();
                _Venue_Booking = db.Venue_Booking.Where(X => X.DateTime_From.Year == DateTime.Now.Year && X.Booking_Type_Seq != 1 && X.Topic_Seq == Topic_For_Training && X.DateTime_From.Month == FromDate.Month && X.DateTime_From.Day == FromDate.Day).ToList();

            }
            return View("Training_Session_Assessment", _Venue_Booking);

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
