using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Optimization;
using System.Web.Security;
using System.Web.Script.Serialization;
using LibraryAssistantApp.Models;
using LibraryAssistantApp.App_Start;

namespace LibraryAssistantApp
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //invoke RegisterGlobalFilters method
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
        }

        protected void Application_PostAuthenticateRequest()
        {
            HttpCookie authoCookies = Request.Cookies[FormsAuthentication.FormsCookieName];
            if (authoCookies != null)
            {
                FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(authoCookies.Value);
                JavaScriptSerializer js = new JavaScriptSerializer();
                Registered_Person user = js.Deserialize<Registered_Person>(ticket.UserData);
                MyIdentity myIdentity = new MyIdentity(user);
                MyPrincipal myPrincipal = new MyPrincipal(myIdentity);
                HttpContext.Current.User = myPrincipal;
            }
        }
    }
}
