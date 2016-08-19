using LibraryAssistantApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LibraryAssistantApp.Controllers
{
    public class RoleController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();
        
        // GET: Role
        public ActionResult Index()
        {
            return View();
        }

        // GET: Role/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Role/Create
        public ActionResult Create()
        {
            var RoleAction = db.Role_Action;
            RoleModel roleModel = new RoleModel();
            roleModel.RoleActions = new List<RoleActionModel>();

            foreach (var a in RoleAction)
            {
                RoleActionModel ra = new RoleActionModel();
                ra.CreateInd = a.Create_Ind;
                ra.ReadInd = a.Read_Ind;
                ra.UpdateInd = a.Update_Ind;
                ra.DeleteInd = a.Delete_Ind;
                ra.ActionId = a.Action_ID;
                ra.RoleId = a.Role_ID;
                ra.ActionName = a.Action.Action_Name;
                roleModel.RoleActions.Add(ra);
            }

            var distinctActions =
                roleModel.RoleActions.GroupBy(x => x.ActionId)
                    .Select(g => g.FirstOrDefault())
                        .ToList();

            roleModel.RoleActions = distinctActions;

            return View(roleModel);
        }

        // POST: Role/Create
        [HttpPost]
        public ActionResult Create(RoleModel role)
        {
            try
            {
                Role r = new Role();
                r.Role_Name = role.RoleName;
                db.Roles.Add(r);
                foreach (var o in role.RoleActions)
                {
                    Role_Action ra = new Role_Action();
                    ra.Action_ID = o.ActionId;
                    ra.Role_ID = r.Role_ID;
                    ra.Create_Ind = o.CreateInd;
                    ra.Read_Ind = o.ReadInd;
                    ra.Update_Ind = o.UpdateInd;
                    ra.Delete_Ind = o.DeleteInd;
                    db.Role_Action.Add(ra);
                }
                db.SaveChanges();

                return RedirectToAction("Home","Index");
            }
            catch
            {
                return View();
            }
                    
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Role/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Role/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
