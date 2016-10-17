using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.SqlClient;
using System.Data.Entity;
using LibraryAssistantApp.Models;
using System.Data;
using System.IO;
using System.Web.Script.Serialization;

namespace LibraryAssistantApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class BackupController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult RestoreDatabase(string path)
        {
            try
            {
                JavaScriptSerializer js = new JavaScriptSerializer();
                string _path = (string)js.Deserialize(path, typeof(string));
                string extension = Path.GetExtension(_path);
                if (extension != ".bak")
                {
                    return Json("extension", JsonRequestBehavior.AllowGet);
                }
                if (_path.Contains(("Log")))
                {
                    return Json("log", JsonRequestBehavior.AllowGet);
                }
                if (!_path.Contains("LibraryAssistant_"))
                {
                    return Json("library", JsonRequestBehavior.AllowGet);
                }
                //string fileName = _path.Substring(path.LastIndexOf("\\"));
                string fileName = Path.GetFileName(_path);
                string dataTime = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH-mm");
                string logName = "LibraryAssistant_LogBackup_" + dataTime + ".bak";
                var sqlCommand = @"USE [master] ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE BACKUP LOG [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'LibraryAssistant_LogBackup', NOSKIP, NOREWIND, NOUNLOAD,  NORECOVERY ,  STATS = 5 RESTORE DATABASE[{0}] FROM DISK = N'C:\Program Files\Microsoft SQL Server\MSSQL11.MSSQLSERVER\MSSQL\Backup\{2}' WITH FILE = 1, NOUNLOAD, STATS = 5 ALTER DATABASE [{0}] SET MULTI_USER";
                using (var db = new LibraryAssistantEntities())
                {
                    string dbname = db.Database.Connection.Database;
                    db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname, logName, fileName));
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }    
        }

        public JsonResult BackupDatabase()
        {
            try
            {
                string dataTime = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH-mm");
                string fileName = "LibraryAssistant_" + dataTime + ".bak";
                string sqlCommand = @"BACKUP DATABASE [{0}] TO  DISK = N'{1}' WITH NOFORMAT, NOINIT,  NAME = N'LibraryAssistant-Full Database Backup', SKIP, NOREWIND, NOUNLOAD,  STATS = 10";

                using (var db = new LibraryAssistantEntities())
                {
                    string dbname = db.Database.Connection.Database;
                    db.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, string.Format(sqlCommand, dbname, fileName));
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            catch
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
        }
    }
}