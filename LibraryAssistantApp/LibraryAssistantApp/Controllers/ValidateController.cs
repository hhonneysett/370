using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
    }
}