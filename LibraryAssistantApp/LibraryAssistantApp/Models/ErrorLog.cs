using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

namespace LibraryAssistantApp.Models
{
    public class ErrorLog
    {
        public static void WriteErrorLog(Exception ex)
        {
            string filePath = @"C:\Users\Heyden\Documents\GitHub\370\LibraryAssistantApp\LibraryAssistantApp\LogFiles\Error.txt";

            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("Message :" + ex.Message + "<br/>" + Environment.NewLine + "StackTrace :" + ex.StackTrace +
                   "" + Environment.NewLine + "Date :" + DateTime.Now.ToString());
                writer.WriteLine(Environment.NewLine + "-----------------------------------------------------------------------------" + Environment.NewLine);
            }
        }
    }
}