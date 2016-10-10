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
            var row = new TagBuilder("div");
            var col = new TagBuilder("div");
            var span1 = new TagBuilder("span");
            var span2 = new TagBuilder("span");
            col.AddCssClass("alert alert-" + classStyle);
            span1.AddCssClass("glyphicon glyphicon-exclamation-sign");
            span2.AddCssClass("sr-only");
            span2.SetInnerText("Error:");
            col.InnerHtml = span1.ToString() + span2.ToString() + " " + message;
            row.InnerHtml = col.ToString();
            return MvcHtmlString.Create(row.ToString());
        }

    }
}