﻿using HowToDoIt.Filters;
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
    [Culture]
    public class ProfileController : Controller
    {

        [Authorize]
        public new ActionResult Profile()
        {
            Profil profile;
            profile = GetProfileUser();
            return View(profile);
        }

        private Profil GetProfileUser()
        {
            Profil profile = new Profil();
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser user;
                user = Manager.GetCurrentUser(db, User.Identity.Name);
                if (ProfileExist(profile,user,db))
                    profile = user.Profil;
            }
            return profile;
        }

        private bool ProfileExist(Profil profile,ApplicationUser user,ApplicationDbContext db)
        {
            if (user.Profil == null)
            {
                profile.Avatar = "~/image/256.jpg";
                profile.Users = user;
                db.Profils.Add(profile);
                db.SaveChanges();
                return false;
            }
            return true;
        }

        // GET: Profile
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
    }
}