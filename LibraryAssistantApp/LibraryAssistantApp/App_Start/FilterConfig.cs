using System.Web.Mvc;

namespace LibraryAssistantApp.App_Start
{
    public class FilterConfig
    {
        //add global HandleErrorAttribute as global filter
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}