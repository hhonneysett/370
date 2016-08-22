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
        
        public ActionResult Index(int? id, int? actionID)
        {
            var viewModel = new RoleIndexModel();
            viewModel.Roles = db.Roles
                .Include(i => i.Role_Action.Select(x => x.Action));

            return View(viewModel);
        }
        public PartialViewResult RoleDetails(int? id)
        {
            var viewModel = new RoleIndexModel();
            viewModel.Roles = db.Roles
                .Include(i => i.Role_Action.Select(x => x.Action));
            if (id != null)
            {
                ViewBag.RoleID = id.Value;
                viewModel.RoleActions = db.Roles.Single(
                    i => i.Role_ID == id.Value).Role_Action;
            }
            return PartialView("RoleDetails", viewModel);
        }
        public PartialViewResult ActionDetails(int? actionID)
        {
            var viewModel = new RoleIndexModel();
            
            if (actionID != null)
            {
                ViewBag.ActionID = actionID;
                viewModel.Actions = db.Actions.Where(
                    i => i.Action_ID == actionID);
            }
            return PartialView("ActionDetails", viewModel);
        }

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

        [HttpPost]
        public ActionResult Create(RoleModel role)
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
            try
            {
                int Count = 0;
                bool create = true;
                bool read = true;
                bool update = true;
                bool delete = true;

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
                    create = ra.Create_Ind;
                    if (create == false)
                    {
                        Count++;
                    }
                    read = ra.Read_Ind;
                    if (read == false)
                    {
                        Count++;
                    }
                    update = ra.Update_Ind;
                    if (update == false)
                    {
                        Count++;
                    }
                    delete = ra.Delete_Ind;
                    if (delete == false)
                    {
                        Count++;
                    }
                    if (Count == (role.RoleActions.Count() * 4))
                    {
                        ViewBag.Error = "Role must be assigned at least 1 action";
                        return View(roleModel);
                    }
                }
                db.SaveChanges();

                return RedirectToAction("Index","Role");
            }
            catch
            {
                return View(roleModel);
            }       
        }

        public ActionResult Edit(int? id, RoleIndexModel roleIndex)
        {
            if (id == null)
            {
                TempData["Error"] = "Please select a role before selecting update";
                return RedirectToAction("Index", roleIndex);
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

        [HttpPost]
        public ActionResult Edit(int id, RoleEditModel roleEdit)
        {
            int Count = 0;
            bool create = true;
            bool read = true;
            bool update = true;
            bool delete = true;
            try
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

                    if (create == false)
                    {
                        Count++;
                    }
                    read = o.Read_Ind;
                    if (read == false)
                    {
                        Count++;
                    }
                    update = o.Update_Ind;
                    if (update == false)
                    {
                        Count++;
                    }
                    delete = o.Delete_Ind;
                    if (delete == false)
                    {
                        Count++;
                    }
                    if (Count == (roleEdit.actionList.Count() * 4))
                    {
                        ViewBag.Error = "Role must be assigned at least 1 action";
                        RoleEditModel roleModel = new RoleEditModel();
                        roleModel.role = db.Roles.Find(id);
                        if (roleModel.role == null)
                        {
                            return HttpNotFound();
                        }
                        roleModel.actionList = db.Role_Action.Where(
                                i => i.Role_ID == id).ToList();
                        return View(roleModel);
                    }
                }
                db.SaveChanges();
                return RedirectToAction("Index", "Role");
            }
            catch
            {
                RoleEditModel roleModel = new RoleEditModel();
                roleModel.role = db.Roles.Find(id);
                if (roleModel.role == null)
                {
                    return HttpNotFound();
                }
                roleModel.actionList = db.Role_Action.Where(
                        i => i.Role_ID == id).ToList();
                return View(roleModel);
            }

        }

        public ActionResult Delete(int? id, RoleIndexModel roleIndex)
        {
            if (id == null)
            {
                TempData["Error"] = "Please select a role before selecting delete";
                return RedirectToAction("Index", roleIndex);
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
