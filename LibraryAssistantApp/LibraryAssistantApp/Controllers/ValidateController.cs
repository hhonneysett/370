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
            var currentUP = db.Current_UP_Person.Any(student => student.Person_ID == Person_ID);
            var alreadyReg = db.Registered_Person.Any(student => student.Person_ID == Person_ID);

            if (currentUP.Equals(true) && alreadyReg.Equals(false))
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
    }
}