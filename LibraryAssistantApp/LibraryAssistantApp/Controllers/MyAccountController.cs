using LibraryAssistantApp.Models;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace LibraryAssistantApp.Controllers
{
    public class MyAccountController : Controller
    {
        public ActionResult Login()
        {
            //prevent logged in user from login again
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Login l, string ReturnUrl = "")
        {


            if (ModelState.IsValid)
            {
                var hashedPass = FormsAuthentication.HashPasswordForStoringInConfigFile(l.Person_Password, "MD5");
                bool isValidUser = Membership.ValidateUser(l.Person_ID, hashedPass);
                LibraryAssistantEntities db = new LibraryAssistantEntities();
                if (isValidUser)
                {
                    Registered_Person registered_person = null;
                    registered_person = db.Registered_Person.Where(a => a.Person_ID.Equals(l.Person_ID)).FirstOrDefault();                  
                    if (registered_person != null)
                    {
                        //initiate an instance of a passable registered student 
                        Registered_Person passablePerson = new Registered_Person();
                        passablePerson.Person_ID = registered_person.Person_ID;
                        passablePerson.Person_Name = registered_person.Person_Name;
                        passablePerson.Person_Surname = registered_person.Person_Surname;
                        passablePerson.Person_Email = registered_person.Person_Email;
                        passablePerson.Person_Password = registered_person.Person_Password;

                        JavaScriptSerializer js = new JavaScriptSerializer();
                        string data = js.Serialize(passablePerson);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, registered_person.Person_ID, DateTime.Now, DateTime.Now.AddMinutes(30), l.RememberMe, data);
                        string encToken = FormsAuthentication.Encrypt(ticket);
                        HttpCookie authCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                        Response.Cookies.Add(authCookies);
                        if (ReturnUrl != "")
                        {
                            return Redirect(ReturnUrl);
                        }
                        else
                        {
                            return RedirectToAction("Index", "Home");
                        }                      
                    }                    
                }
                else
                {
                        TempData["classStyle"] = "warning";
                        TempData["Message"] = "Invalid Login Details";
                }
            }
            ModelState.Remove("Person_Password");
            return View();
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            //update session logout time
            LibraryAssistantEntities db = new LibraryAssistantEntities();        
            //end session update

            return RedirectToAction("Index", "Home");
        } //user logout
    }
}