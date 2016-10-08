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

namespace LibraryAssistantApp.Controllers
{
    public class BackupController : Controller
    {
        // Backup
        public void BackUp()
        {

            LibraryAssistantEntities entity = new LibraryAssistantEntities();
            string dataTime = DateTime.Now.ToString("yyyy-MM-dd") + "-" + DateTime.Now.ToString("HH-mm");
            string directory = HttpContext.Current.Server.MapPath("~/") + "/backups/" + dataTime + "/";
            string fileName = directory + dataTime + ".bak";

            #region Response
            HttpResponse Response = HttpContext.Current.Response;
            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentType = "application/zip";
            Response.AddHeader("content-disposition", "inline; filename=\"" + dataTime + "\".zip");
            #endregion

            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);


            // Here the procedure is called and executes successfully
            entity.Database.ExecuteSqlCommand(System.Data.Entity.TransactionalBehavior.DoNotEnsureTransaction, "EXEC [dbo].[BackUp] @path = N'" + fileName + "'");

            #region Compress
            using (var memoryStream = new System.IO.MemoryStream())
            {
                using (ZipFile zip = new ZipFile())
                {
                    zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
                    zip.ParallelDeflateThreshold = -1;
                    zip.AddDirectory(directory);
                    zip.Save(memoryStream);
                }

                memoryStream.Position = 0;
                var b = new byte[1024];
                int n;
                while ((n = memoryStream.Read(b, 0, b.Length)) > 0)
                    Response.OutputStream.Write(b, 0, n);
            }
            #endregion

            Directory.Delete(directory, true);

            Response.Close();
        }
    }
}