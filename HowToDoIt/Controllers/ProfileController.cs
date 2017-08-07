using HowToDoIt.Filters;
using HowToDoIt.Models;
using HowToDoIt.Models.Classes_for_Db;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HowToDoIt.Controllers
{
    [Culture]
    public class ProfileController : Controller
    {

        [Authorize]
        public new ActionResult Profile()
        {
            Profil profile;
            profile = Manager.GetProfileUser(User.Identity.Name);
            return View(profile);
        }

        public JsonResult Upload()
        {
            try
            {
                string fileName = "";
                fileName= Manager.UploadFile(Request, Server, "~/Files/");
                return Json(new { success = true, responseText = "~/Files/" + fileName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception) { return null; }
        }

        public void SaveProfileData(Profil profil)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Entry(profil).State = EntityState.Modified;
                db.SaveChanges();
            }
        }

        public ActionResult OpenProfile(int idInstruction)
        {
            Profil profile= new Profil();
            using (var db = new ApplicationDbContext())
            {
                var instruction= db.Instructions.Find(idInstruction);
                if (Manager.ProfileExist(profile, instruction.User, db))
                    profile = instruction.User.Profil;
            }
            return View("~/Views/Profile/UserInfo.cshtml",profile);
        }

        public ActionResult OpenProfileFromComent(string idUser)
        {
            Profil profile = new Profil();
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(idUser);
                if (Manager.ProfileExist(profile, user, db))
                    profile = user.Profil;
            }
            return View("~/Views/Profile/UserInfo.cshtml", profile);
        }

        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult Search(string word)
        {
            using (var db = new ApplicationDbContext())
            {
                var user= (db.Users.ToList()).Where(c => c.UserName == word).FirstOrDefault();
                List<ApplicationUser> list = new List<ApplicationUser>();
                AddUserToList(list, user);
                return View("Users", list);
            }
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult Lock(string idUser)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(idUser);
                user.IsLock = true;
                db.SaveChanges();
                return Users();
            }
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult Unlock(string idUser)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(idUser);
                user.IsLock = false;
                db.SaveChanges();
                return Users();
            }
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult DeleteUser(string idUser)
        {
            using (var db = new ApplicationDbContext())
            {
                var user = db.Users.Find(idUser);
                DeleteEntity<Instruction>(db.Instructions, user.Instructions.ToList());
                if (user.Profil!=null)
                {
                    var p = new List<Profil>();
                    p.Add(user.Profil);
                    DeleteEntity<Profil>(db.Profils, p);
                }
                DeleteEntity<Comment>(db.Comments,user.Comments.ToList());
                db.Users.Remove(user);
                db.SaveChanges();
                return Users();
            }
        }

        private void DeleteEntity<T>(IDbSet<T> entity,List<T> list)where T:class
        {
            foreach(var c in list)
            {
                entity.Remove(c);
            }
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]
        [OutputCache(NoStore = true, Duration = 0, VaryByParam = "*")]
        public ActionResult Users()
        {
            using (var db = new ApplicationDbContext())
            {
                var users = FindAllRoleUser(db);
                return View("Users", users);
            }
        }

        private List<ApplicationUser> FindAllRoleUser(ApplicationDbContext db)
        {
            List<ApplicationUser> list = new List<ApplicationUser>();
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));
            SearchUser(db, list, userManager);
            return list;
        }

        private void SearchUser(ApplicationDbContext db,List<ApplicationUser> list, UserManager<ApplicationUser> userManager)
        {
            foreach (var user in db.Users.ToList())
            {
                if (!userManager.IsInRole(user.Id, "Admin"))
                {
                    AddUserToList(list, user);
                }
            }
        }

        private void AddUserToList(List<ApplicationUser> list,ApplicationUser user)
        {
            user.Profil = user.Profil ?? new Profil();
            user.Profil.Avatar = user.Profil.Avatar ?? (user.Profil.Avatar = "~/image/256.jpg");
            user.Instructions = user.Instructions ?? new List<Instruction>();
            user.Comments = user.Comments ?? new List<Comment>();
            list.Add(user);
        }

    }
}