using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Models
{
    public static class HtmlExtensions
    {
        public static MvcHtmlString Notification(this HtmlHelper htmlHelper)
        {
            // Look first in ViewData
            var notification = htmlHelper.ViewData["Message"] as string;
            if (string.IsNullOrEmpty(notification))
            {
                // Not found in ViewData, try TempData
                notification = htmlHelper.ViewContext.TempData["Message"] as string;
            }

            // You may continue searching for a notification in Session, Request, ... if you will

            if (string.IsNullOrEmpty(notification))
            {
                // no notification found
                return MvcHtmlString.Empty;
            }

            string classStyleVar = htmlHelper.ViewContext.TempData["classStyle"] as string;

            return FormatNotification(notification, classStyleVar);
        }

        private static MvcHtmlString FormatNotification(string message, string classStyle)
        {
            var div = new TagBuilder("div");
            div.AddCssClass("alert alert-" + classStyle);
            div.SetInnerText(message);
            return MvcHtmlString.Create(div.ToString());
        }

    }
}