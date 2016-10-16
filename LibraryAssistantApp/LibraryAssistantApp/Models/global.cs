using System;
using System.Linq;

namespace LibraryAssistantApp.Models
{
    public static class global
    {
        static LibraryAssistantEntities db = new LibraryAssistantEntities();

        public static void addAudit(string action, string performed, string crud, string personid)
        {
            try
            {
                //get action id
                var actions = db.Actions.ToList();
                var a_id = actions.Where(a => a.Action_Name.ToLower() == action.ToLower()).FirstOrDefault();

                //get session id
                var s_id = db.Person_Session_Log.Where(p => p.Person_ID == personid).OrderByDescending(s => s.Session_ID).FirstOrDefault();

                //create entry 
                var log = new Person_Session_Action_Log
                {
                    Action_ID = a_id.Action_ID,
                    Action_DateTime = DateTime.Now,
                    Session_ID = s_id.Session_ID,
                    Action_Performed = performed,
                    Crud_Operation = crud,
                };

                //save entry
                db.Person_Session_Action_Log.Add(log);
                db.SaveChanges();
            }
            catch
            {

            }
        }
    }
}