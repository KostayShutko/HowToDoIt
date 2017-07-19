using HowToDoIt.Models;
using HowToDoIt.Models.Classes_for_Db;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HowToDoIt.Controllers
{
    public class ProfileController : Controller
    {

        [Authorize]
        public new ActionResult Profile()
        {
            Profile profile = new Profile();
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser user;
                user = GetCurrentUser(db);
                if (user.Profile == null)
                {
                    profile.Avatar = "~/image/256.jpg";
                    profile.Users = user;
                    db.Profiles.Add(profile);
                    db.SaveChanges();
                }
                else
                    profile = user.Profile;
            }

            return View(profile);
        }

        // GET: Profile
        public JsonResult Upload()
        {
            try
            {
                string fileName = "";
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    HttpPostedFileBase file = Request.Files[i];                                              
                    int fileSize = file.ContentLength;
                    fileName = file.FileName;
                    string mimeType = file.ContentType;
                    System.IO.Stream fileContent = file.InputStream;
                    //To save file, use SaveAs method
                    file.SaveAs(Server.MapPath("~/Files/") + fileName); //File will be saved in application root
                }
                return Json(new { success = true, responseText = "~/Files/" + fileName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception) { return null; }
        }

        private ApplicationUser GetCurrentUser(ApplicationDbContext db)
        {
            var users = db.Users.ToList();
            foreach (var user in users)
            {
                if (user.UserName == User.Identity.Name)
                {
                    return user;
                }
            }
            return null;
        }

        public void SaveProfileData(Profile profile)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Entry(profile).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}