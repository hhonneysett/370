using LibraryAssistantApp.Models;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Web.Security;

namespace LibraryAssistantApp.Controllers
{
    public class AccountController : Controller
    {
        LibraryAssistantEntities db = new LibraryAssistantEntities();

        public object WebSecurity { get; private set; }

        public ActionResult Login(string ReturnUrl = "")
        {
            //prevent logged in user from login again
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        [HttpPost]
        public string Login(Login l, string ReturnUrl = "")
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
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, registered_person.Person_ID, DateTime.Now, DateTime.Now.AddMinutes(30), false, data);
                        string encToken = FormsAuthentication.Encrypt(ticket);
                        HttpCookie authCookies = new HttpCookie(FormsAuthentication.FormsCookieName, encToken);
                        Response.Cookies.Add(authCookies);

                        Person_Session_Log newSession = new Person_Session_Log();

                        newSession.Person_ID = l.Person_ID;
                        newSession.Login_DateTime = DateTime.Now;

                        var tempLogout = new TimeSpan(0, 100, 0);

                        newSession.Logout_DateTime = DateTime.Now.Add(tempLogout);

                        db.Person_Session_Log.Add(newSession);

                        db.SaveChanges();

                        Session["loginSession"] = newSession;

                        //add layout tpye
                        if (registered_person.Person_Type == "Administrator")
                        {
                            Session["layout"] = "~/Views/Shared/_Layout.cshtml";
                        }
                        else if (registered_person.Person_Type == "Student")
                        {
                            Session["layout"] = "~/Views/Shared/_LayoutStudent.cshtml";
                        }
                        else
                        {
                            Session["layout"] = "~/Views/Shared/_LayoutEmp.cshtml";
                        }

                        if (ReturnUrl != "")
                        {
                            return ReturnUrl;
                        }
                        else
                        {
                            return Url.Action("Index", "Home");
                        }                      
                    }                    
                }
            }
            ModelState.Remove("Person_Password");
            return "TEST FAILURE";
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();

            //update session logout time

            var session = (Person_Session_Log)Session["loginSession"];

            var logoutSession = (from a in db.Person_Session_Log
                                 where a.Session_ID == session.Session_ID
                                 select a).FirstOrDefault();

            logoutSession.Logout_DateTime = DateTime.Now;

            db.Entry(logoutSession).State = System.Data.Entity.EntityState.Modified;

            db.SaveChanges();
            //end session update

            return RedirectToAction("Index", "Home");
        } //user logout

        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(LostPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                var person = (from a in db.Registered_Person
                              where a.Person_ID == model.personId
                              select a).FirstOrDefault();

                var result = db.spResetPasswordFunc(model.personId).FirstOrDefault();

                if (Convert.ToBoolean(result.ReturnCode))
                {
                    SendPasswordResetEmail(result.Email, model.personId, result.UniqueId.ToString());
                }

                return RedirectToAction("passResetPost");
            }
            else return View();           
        }

        private void SendPasswordResetEmail(string ToEmail, string UserName, string UniqueId)
        {
            // MailMessage class is present is System.Net.Mail namespace
            MailMessage mailMessage = new MailMessage("YourEmail@gmail.com", ToEmail);


            // StringBuilder class is present in System.Text namespace
            StringBuilder sbEmailBody = new StringBuilder();
            sbEmailBody.Append("Dear " + UserName + ",<br/><br/>");
            sbEmailBody.Append("Please click on the following link to reset your password");
            sbEmailBody.Append("<br/>"); sbEmailBody.Append("http://localhost:52621/Account/ResetPassword/?id=" + UniqueId);
            sbEmailBody.Append("<br/><br/>");
            sbEmailBody.Append("<b>UP Library Assistant</b>");

            mailMessage.IsBodyHtml = true;

            MailMessage message = new MailMessage();
            SmtpClient client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;

            message.From = new MailAddress("uplibraryassistant@gmail.com");
            message.To.Add(ToEmail);
            message.Subject = "Account Activation";
            message.Body = sbEmailBody.ToString();
            message.IsBodyHtml = true;
            client.EnableSsl = true;
            client.UseDefaultCredentials = true;
            client.Credentials = new System.Net.NetworkCredential("uplibraryassistant@gmail.com", "tester123#");
            client.Send(message);
        }

        public ActionResult passResetPost()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            var rp = db.ResetPasswordRequests.ToList();
            var check = rp.Any(r => r.Id.ToString() == id);

            if(check)
            {
                var passReset = (from c in rp
                                 where c.Id.ToString() == id
                                 select c).FirstOrDefault();

                Session["passReset"] = passReset;

                return View();
            }
            else
            {
                return RedirectToAction("invalidReset");
            }

        }

        [HttpPost]
        public ActionResult ResetPassword(ResetPassModel model)
        {
            if (ModelState.IsValid)
            {
                var Pass = (ResetPasswordRequest)Session["passReset"];
                var Person = db.Registered_Person.Where(p => p.Person_ID == Pass.Person_ID).FirstOrDefault();

                var deletePass = db.ResetPasswordRequests.Where(p => p.Person_ID == Person.Person_ID).FirstOrDefault();

                var hashedPass = FormsAuthentication.HashPasswordForStoringInConfigFile(model.Person_Password, "MD5");

                Person.Person_Password = hashedPass;

                db.Entry(Person).State = System.Data.Entity.EntityState.Modified;

                db.ResetPasswordRequests.Remove(deletePass);

                db.SaveChanges();

                return RedirectToAction("Login");
            }
            else
            {
                return View();
            }
            
        }

        public ActionResult invalidReset()
        {
            return View();
        }

        private T Deserialise<T>(string json)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(json)))
            {
                var serialiser = new DataContractJsonSerializer(typeof(T));
                return (T)serialiser.ReadObject(ms);
            }
        }
    }
}