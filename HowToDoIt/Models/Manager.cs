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

namespace HowToDoIt.Models
{
    public class Manager
    {
        public static ApplicationUser GetCurrentUser(ApplicationDbContext db,string nameCurrentUser)
        {
            var users = db.Users.ToList();
            foreach (var user in users)
            {
                if (user.UserName == nameCurrentUser)
                {
                    return user;
                }
            }
            return null;
        }

        public static string UploadFile(HttpRequestBase request,HttpServerUtilityBase server,string directiry)
        {
            string fileName = "";
            for (int i = 0; i < request.Files.Count; i++)
            {
                HttpPostedFileBase file = request.Files[i];
                int fileSize = file.ContentLength;
                fileName = file.FileName;
                string mimeType = file.ContentType;
                System.IO.Stream fileContent = file.InputStream;
                file.SaveAs(server.MapPath(directiry) + fileName);
            }
            return fileName;
        }

    }
}