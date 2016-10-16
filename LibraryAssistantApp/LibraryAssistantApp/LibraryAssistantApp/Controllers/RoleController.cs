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
    [Authorize(Roles ="Admin")]
    public class RoleController : Controller
    {
        private LibraryAssistantEntities db = new LibraryAssistantEntities();
        
        public ActionResult Index(string search)
        {
            var viewModel = new RoleIndexModel();
            viewModel.Roles = db.Roles
                .Include(i => i.Role_Action.Select(x => x.Action));
            if (search != null)
            {
                viewModel.Roles = (viewModel.Roles.Where(x => x.Role_Name.ToLower().StartsWith(search.ToLower())));
            }
            if (viewModel.Roles.Count() == 0)
            {
                TempData["Error"] = "No roles match search criteria";
            }
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
                ViewBag.ErrorMsg = "";
                var query = (from q in db.Roles
                             where q.Role_Name.ToLower() == role.RoleName.ToLower()
                             select q);
                if (query.Count() != 0)
                {
                    ViewBag.ErrorMsg = "The role name exists, please provide a different role name";
                    return View(roleModel);
                }
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
                ViewBag.ErrorMsg = "";
                var query = (from q in db.Roles
                             where q.Role_Name.ToLower() == roleEdit.role.Role_Name.ToLower()
                             select q);
                if (query.Count() >= 2)
                {
                    ViewBag.ErrorMsg = "The role name exists, please provide a different role name";
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

            ViewBag.ErrorMsg = "Are you sure you want to delete?";
            TempData["Disabled"] = false;

            RoleEditModel roleModel = new RoleEditModel();
            roleModel.role = db.Roles.Find(id);
            roleModel.actionList = db.Role_Action.Where(
            i => i.Role_ID == id.Value).ToList();

            if (roleModel.role == null)
            {
                return HttpNotFound();
            }
            if (roleModel.role.Role_Name == "Super Admin")
            {
                ViewBag.ErrorMsg = "'Super Admin' role cannot be deleted";
                TempData["Disabled"] = true;
                return View(roleModel);
            }
            if (roleModel.role.Role_Name == "Admin")
            {
                ViewBag.ErrorMsg = "'Admin' role cannot be deleted";
                TempData["Disabled"] = true;
                return View(roleModel);
            }

            var rolePerson = from p in db.Person_Role
                             where p.Role_ID == id
                             select p;


            if (rolePerson.Count() != 0)
            {
                ViewBag.ErrorMsg = "Role cannot be deleted becuase there are persons assigned to the role";
                TempData["Disabled"] = true;
                return View(roleModel);
            }

            return RedirectToAction("Index");
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id, RoleEditModel roleEdit)
        {
            try
            {
                var rolePerson = from p in db.Person_Role
                                 where p.Role_ID == id
                                 select p;

                ViewBag.ErrorMsg = "Are you sure you want to delete?";
                TempData["Disabled"] = false;
                if (rolePerson.Count() != 0)
                {
                    ViewBag.ErrorMsg = "Role cannot be deleted becuase there are persons assigned to the role";
                    TempData["Disabled"] = true;
                    return View(roleEdit);
                }

                Role r = db.Roles.Find(id);

                if (roleEdit.role == null)
                {
                    return HttpNotFound();
                }
                if (roleEdit.role.Role_Name == "Super Admin")
                {
                    ViewBag.ErrorMsg = "'Super Admin' role cannot be deleted";
                    TempData["Disabled"] = true;
                    return View(roleEdit);
                }
                if (roleEdit.role.Role_Name == "Admin")
                {
                    ViewBag.ErrorMsg = "'Admin' role cannot be deleted";
                    TempData["Disabled"] = true;
                    return View(roleEdit);
                }

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
                return View(roleEdit);
            }
        }
    }
}
