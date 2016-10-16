using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LibraryAssistantApp.Controllers
{
    public class ValidateController : Controller
    {

        LibraryAssistantEntities db = new LibraryAssistantEntities();

        //check if person id is valid
        public JsonResult validateStudentNumber(string Person_ID)
        {
            var alreadyReg = db.Registered_Person.Any(student => student.Person_ID == Person_ID);

            if (alreadyReg.Equals(false))
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }

        //check if email is valid
        public JsonResult checkEmail(string Person_Email)
        {
            return Json(!db.Registered_Person.Any(person => person.Person_Email == Person_Email), JsonRequestBehavior.AllowGet);
        }

        //validate OTP
        public JsonResult checkOTP(int OTP)
        {
            var sentOTP = (int)Session["OTP"];

            if (OTP == sentOTP)
                return Json(true, JsonRequestBehavior.AllowGet);
            else return Json(false, JsonRequestBehavior.AllowGet);
        }

        //check if email is valid
        public JsonResult checkUpdateEmail(string Person_Email)
        {
            return Json(!db.Registered_Person.Any(person => person.Person_Email == Person_Email && person.Person_ID != User.Identity.Name), JsonRequestBehavior.AllowGet);
        }

        //check current password
        public JsonResult currentPass(string CurrentPassword)
        {
            var hashedPass = FormsAuthentication.HashPasswordForStoringInConfigFile(CurrentPassword, "MD5");

            return Json(db.Registered_Person.Any(r => r.Person_ID == User.Identity.Name && r.Person_Password == hashedPass), JsonRequestBehavior.AllowGet);
        }

        //check student is a registered person
        public JsonResult checkRegPerson(string personId)
        {
            return Json(db.Registered_Person.Any(r => r.Person_ID == personId), JsonRequestBehavior.AllowGet);
        }

        //check student is a registered person
        public JsonResult loginCheckPerson(string Person_ID)
        {
            return Json(db.Registered_Person.Any(r => r.Person_ID == Person_ID), JsonRequestBehavior.AllowGet);
        }

        //check common problem tpye name
        public JsonResult checkProblemType(string name)
        {
            var types = db.Common_Problem_Type.ToList();
            var type = types.Where(t => t.Common_Problem_Type_Name.ToLower() == name.ToLower());
            if (type.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check problem name
        public JsonResult checkProblem(string Common_Problem_Name)
        {
            var problems = db.Common_Problem.ToList();
            var problem = problems.Where(p => p.Common_Problem_Name.ToLower() == Common_Problem_Name.ToLower());
            if (problem.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check characteristic name
        public JsonResult checkCharacteristic(string Characteristic_Name)
        {
            var characteristics = db.Characteristics.ToList();
            var c = characteristics.Where(e => e.Characteristic_Name.ToLower() == Characteristic_Name.ToLower());
            if (c.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check training topic 
        public JsonResult checkTopic(string Topic_Name)
        {
            var topics = db.Topics.ToList();
            var check = topics.Where(t => t.Topic_Name.ToLower() == Topic_Name.ToLower());
            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check category name
        public JsonResult checkCategory(string categoryName)
        {
            var categories = db.Categories.ToList();
            var check = categories.Where(c => c.Category_Name.ToLower() == categoryName.ToLower());
            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check file name
        public JsonResult checkFilename(string Document_Name)
        {
            var files = db.Document_Repository.Where(c => c.Document_Status == "Active").ToList();
            var check = files.Where(f => f.Document_Name.ToLower() == Document_Name.ToLower());
            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check file type
        public JsonResult checkFileType(string Type_Name)
        {
            var types = db.Document_Type.ToList();
            var check = types.Where(t => t.Document_Type_Name.ToLower() == Type_Name.ToLower());
            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check file category
        public JsonResult checkFileCategory(string name)
        {
            var categories = db.Document_Category.ToList();
            var check = categories.Where(c => c.Category_Name.ToLower() == name.ToLower());
            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check training category
        public JsonResult checkTrainingCategory(string categoryname)
        {
            var categories = db.Categories.ToList();
            var check = categories.Where(c => c.Category_Name.ToLower() == categoryname.ToLower());
            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }

        //check training topic
        public JsonResult checkTrainingTopic(string topicname)
        {
            var topics = db.Topics.ToList();
            var check = topics.Where(t => t.Topic_Name.ToLower() == topicname.ToLower());
            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}