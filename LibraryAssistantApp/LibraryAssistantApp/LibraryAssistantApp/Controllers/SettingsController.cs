using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace LibraryAssistantApp.Controllers
{
    public class SettingsController : Controller
    {
        public const int ImageMinimumBytes = 512;

        // GET: Settings
        public ActionResult Index()
        {
            var path = Path.Combine(Server.MapPath("~"), "settings.xml");

            //read XML
            XElement settings = XElement.Load(path);

            //get time
            XElement scheduler = (from el in settings.Elements("scheduler")
                                         select el).FirstOrDefault();
            string time = scheduler.Element("time").Value;

            var datetime = Convert.ToDateTime(time);

            ViewBag.Time = time;

            //get pictures
            IEnumerable<XElement> pictures = from el in settings.Elements("picture")
                                             select el;

            List<string> spictures = new List<string>();

            foreach (var picture in pictures)
            {
                var link = picture.Value;
                var filename = Path.GetFileName(link);
                spictures.Add(filename);
            }

            ViewBag.Pictures = spictures;

            //get training session durations
            List<string> trainingdurations = (from d in settings.Elements("trainingduration")
                                                      select d.Value).ToList();

            trainingdurations.Sort();

            ViewBag.TrainingDuration = trainingdurations;

            //get disussion room session durations
            List<string> discussiondurations = (from d in settings.Elements("discussionduration")
                                              select d.Value).ToList();
            discussiondurations.Sort();

            ViewBag.DiscussionDuration = discussiondurations;

            //get opening times
            string openTime = (from d in settings.Elements("opentime")
                                     select d.Value).First();
            ViewBag.OpenTime = openTime;

            //get closing times
            string closeTime = (from d in settings.Elements("closetime")
                               select d.Value).First();
            ViewBag.CloseTime = closeTime;

            return View();
        }

        //delete picture
        [HttpPost]
        public JsonResult deletePicture(string filename)
        {
            //get uploaded file path
            var path = Path.Combine(Server.MapPath("~/img/"), filename);

            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

           (from c in doc.Elements("picture")
            where c.Value == path
                           select c).Remove();

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            doc.Save(settingsPath);

            //record action
            global.addAudit("Settings", "Settings: Delete Picture", "Delete", User.Identity.Name);

            //get pictures
            IEnumerable <XElement> pictures = from el in doc.Elements("picture")
                                             select el;

            List<string> spictures = new List<string>();

            foreach (var picture in pictures)
            {
                var link = picture.Value;
                var name = Path.GetFileName(link);
                spictures.Add(name);
            }

            return Json(spictures.ToArray(), JsonRequestBehavior.AllowGet);

        } 

        //update time
        [HttpPost]
        public void updateTime(string time)
        {
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            doc.Element("scheduler").Element("time").Value = time;

            //record action
            global.addAudit("Settings", "Settings: Update Email Time", "Update", User.Identity.Name);

            doc.Save(settingsPath);
        }

        //upload photo
        [HttpPost]
        public JsonResult Upload()
        {
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            for (int i = 0; i < Request.Files.Count; i++)
            {
                HttpPostedFileBase file = Request.Files[i]; //Uploaded file
                                                            //Use the following properties to get file's name, size and MIMEType

                var valid = true;

                //check the image mime types
                if (file.ContentType.ToLower() != "image/jpg" &&
                    file.ContentType.ToLower() != "image/jpeg" &&
                    file.ContentType.ToLower() != "image/pjpeg" &&
                    file.ContentType.ToLower() != "image/gif" &&
                    file.ContentType.ToLower() != "image/x-png" &&
                    file.ContentType.ToLower() != "image/png")
                {
                    valid = false;
                }

                //check the image extension
                if (Path.GetExtension(file.FileName).ToLower() != ".jpg"
                    && Path.GetExtension(file.FileName).ToLower() != ".png"
                    && Path.GetExtension(file.FileName).ToLower() != ".gif"
                    && Path.GetExtension(file.FileName).ToLower() != ".jpeg")
                {
                    valid = false;
                }

                //attempt to read the file and check the first bytes
                try
                {
                    if (!file.InputStream.CanRead)
                    {
                        valid = false;
                    }

                    if (file.ContentLength < ImageMinimumBytes)
                    {
                        valid = false;
                    }

                    byte[] buffer = new byte[512];
                    file.InputStream.Read(buffer, 0, 512);
                    string content = System.Text.Encoding.UTF8.GetString(buffer);
                    if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                    {
                        valid = false;
                    }
                }
                catch (Exception)
                {
                    valid = false;
                }

                //  try to instantiate new Bitmap, if .NET will throw exception
                //  we can assume that it's not a valid image
                try
                {
                    using (var bitmap = new System.Drawing.Bitmap(file.InputStream))
                    {
                    }
                }
                catch (Exception)
                {
                    valid = false;
                }
                try
                {
                    using (var bitmap = new System.Drawing.Bitmap(file.InputStream))
                    {
                    }
                }
                catch (Exception)
                {
                    valid = false;
                }

                //if file is a valid image save the file and add to xml
                if (valid)
                {
                    int fileSize = file.ContentLength;
                    string fileName = file.FileName;
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    //To save file, use SaveAs method
                    file.SaveAs(Server.MapPath("~/img/") + fileName); //File will be saved in application root
                    doc.Add(new XElement("picture", Server.MapPath("~/img/") + fileName));

                    //record action
                    global.addAudit("Settings", "Settings: Upload Photo", "Create", User.Identity.Name);
                }
                else return Json(false, JsonRequestBehavior.AllowGet);               
            }

            //save xml
            doc.Save(settingsPath);

            //get pictures
            IEnumerable<XElement> pictures = from el in doc.Elements("picture")
                                             select el;

            List<string> spictures = new List<string>();

            foreach (var picture in pictures)
            {
                var link = picture.Value;
                var name = Path.GetFileName(link);
                spictures.Add(name);
            }

            return Json(spictures.ToArray(), JsonRequestBehavior.AllowGet);
        }

        //delete training duration
        [HttpPost]
        public JsonResult deleteTrainingDuration(string time)
        {
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            //remove selected time
            (from c in doc.Elements("trainingduration")
             where c.Value == time
             select c).Remove();

            //save changes
            doc.Save(settingsPath);

            //record action
            global.addAudit("Settings", "Settings: Delete Training Duration", "Delete", User.Identity.Name);

            //return list of remaining durations
            IEnumerable<string> durations = (from el in doc.Elements("trainingduration")
                                            select el.Value).ToArray();
            return Json(durations, JsonRequestBehavior.AllowGet);
        }

        //add training duration
        [HttpPost]
        public JsonResult addTrainingDuration(int hour, int min)
        {
            //get string hour
            var shour = "";
            if (hour < 10)
                shour = "0" + hour.ToString();
            else shour = hour.ToString();

            //get string minuntes
            var smin = "";
            if (min < 9)
                smin = "0" + min.ToString();
            else smin = min.ToString();

            //get string time
            var time = shour + ":" + smin;

            //check if time already exists
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            var check = (from c in doc.Elements("trainingduration")
                         where c.Value == time
                         select c);

            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                doc.Add(new XElement("trainingduration", time));
                //save xml
                doc.Save(settingsPath);
                //return list of durations
                IEnumerable<string> durations = (from el in doc.Elements("trainingduration")
                                                 select el.Value).ToArray();

                //record action
                global.addAudit("Settings", "Settings: Add Training Duration", "Create", User.Identity.Name);

                return Json(durations, JsonRequestBehavior.AllowGet);
            }
        }

        //delete discussion duration
        [HttpPost]
        public JsonResult deleteDiscussionDuration(string time)
        {
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            //remove selected time
            (from c in doc.Elements("discussionduration")
             where c.Value == time
             select c).Remove();

            //save changes
            doc.Save(settingsPath);

            //record action
            global.addAudit("Settings", "Settings: Delete Discussion Duration", "Delete", User.Identity.Name);

            //return list of remaining durations
            IEnumerable<string> durations = (from el in doc.Elements("discussionduration")
                                             select el.Value).ToArray();
            return Json(durations, JsonRequestBehavior.AllowGet);
        }

        //add discussion duration
        [HttpPost]
        public JsonResult addDiscussionDuration(int hour, int min)
        {
            //get string hour
            var shour = "";
            if (hour < 10)
                shour = "0" + hour.ToString();
            else shour = hour.ToString();

            //get string minuntes
            var smin = "";
            if (min < 9)
                smin = "0" + min.ToString();
            else smin = min.ToString();

            //get string time
            var time = shour + ":" + smin;

            //check if time already exists
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            var check = (from c in doc.Elements("discussionduration")
                         where c.Value == time
                         select c);

            if (check.Any())
                return Json(false, JsonRequestBehavior.AllowGet);
            else
            {
                doc.Add(new XElement("discussionduration", time));
                //save xml
                doc.Save(settingsPath);
                //return list of durations
                IEnumerable<string> durations = (from el in doc.Elements("discussionduration")
                                                 select el.Value).ToArray();

                //record action
                global.addAudit("Settings", "Settings: Add Discussion Duration", "Create", User.Identity.Name);

                durations.OrderBy(c => c);
                return Json(durations, JsonRequestBehavior.AllowGet);
            }
        }

        //save open time
        [HttpPost]
        public JsonResult saveOpenTime(string time)
        {
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            var closing = doc.Element("closetime").Value;
            var opening = doc.Element("opentime").Value;

            var dclosing = Convert.ToDateTime(closing);
            var dopening = Convert.ToDateTime(opening);

            if (dopening < dclosing)
            {
                doc.Element("opentime").Value = time;
                doc.Save(settingsPath);

                //record action
                global.addAudit("Settings", "Settings: Update Open Time", "Update", User.Identity.Name);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else return Json(false, JsonRequestBehavior.AllowGet);        
        }

        //save close time
        [HttpPost]
        public JsonResult saveCloseTime(string time)
        {
            //get xml
            var settingsPath = Path.Combine(Server.MapPath("~"), "settings.xml");
            XElement doc = XElement.Load(settingsPath);

            var closing = doc.Element("closetime").Value;
            var opening = doc.Element("opentime").Value;

            var dclosing = Convert.ToDateTime(closing);
            var dopening = Convert.ToDateTime(opening);

            if (dopening < dclosing)
            {
                doc.Element("closetime").Value = time;
                doc.Save(settingsPath);

                //record action
                global.addAudit("Settings", "Settings: Update Closin Time", "Update", User.Identity.Name);

                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}