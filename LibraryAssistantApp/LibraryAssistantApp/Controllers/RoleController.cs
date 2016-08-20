using LibraryAssistantApp.Models;
using System.Data.Entity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net;

namespace LibraryAssistantApp.Controllers
{
    public class RoleController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();
        
        // GET: Role
        public ActionResult Index(int? id, int? actionID)
        {
            var viewModel = new RoleIndexModel();
            viewModel.Roles = db.Roles
                .Include(i => i.Role_Action.Select(x => x.Action));

            if (id != null)
            {
                ViewBag.RoleID = id.Value;
                viewModel.RoleActions = viewModel.Roles.Single(
                    i => i.Role_ID == id.Value).Role_Action;
            }

            if (actionID != null)
            {
                ViewBag.ActionID = actionID.Value;
                viewModel.Actions = db.Actions.Where(
                    i => i.Action_ID == actionID);
            }

            return View(viewModel);
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

                return RedirectToAction("Index","Role");
            }
            catch
            {
                return View();
            }
                    
        }

        // GET: Role/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleEditModel roleModel = new RoleEditModel();
            roleModel.role = db.Roles.Find(id);
            if (roleModel.role == null)
            {
                return HttpNotFound();
            }
            roleModel.actionList = db.Role_Action.Where(
                    i => i.Role_ID == id.Value).ToList();

            return View(roleModel);
        }

        // POST: Role/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, RoleEditModel roleEdit)
        {
            Role r = db.Roles.Find(id);
            r.Role_Name = roleEdit.role.Role_Name;
            db.Entry(r).State = EntityState.Modified;

            foreach (var o in roleEdit.actionList)
            {
                Role_Action ra = db.Role_Action.Find(o.RoleAction_ID);
                ra.Create_Ind = o.Create_Ind;
                ra.Read_Ind = o.Read_Ind;
                ra.Update_Ind = o.Update_Ind;
                ra.Delete_Ind = o.Delete_Ind;
                db.Entry(ra).State = EntityState.Modified;
            }
            db.SaveChanges();

            return RedirectToAction("Index", "Role");
        }

        // GET: Role/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoleEditModel roleModel = new RoleEditModel();
            roleModel.role = db.Roles.Find(id);
            if (roleModel.role == null)
            {
                return HttpNotFound();
            }
            roleModel.actionList = db.Role_Action.Where(
                    i => i.Role_ID == id.Value).ToList();

            return View(roleModel);
        }

        // POST: Role/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, RoleEditModel roleEdit)
        {
            try
            {
                // TODO: Add delete logic here
                Role r = db.Roles.Find(id);
                db.Roles.Remove(r);

                foreach (var o in roleEdit.actionList)
                {
                    Role_Action ra = db.Role_Action.Find(o.RoleAction_ID);
                    db.Role_Action.Remove(ra);
                }
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
