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

        public static void ChangeImgInInstruction(List<Instruction> instructions, List<ApplicationUser> listUser, List<Category> listCategory)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                listUser.Add(instructions[i].User);
                listCategory.Add(instructions[i].Category);
                instructions[i].Image = FindImg(instructions[i]);
            }
        }

        private static string FindImg(HowToDoIt.Models.Classes_for_Db.Instruction instr)
        {
            if (instr.Steps != null)
            {
                var sortingStep = (instr.Steps.OrderBy(c => c.Number).ToList());
                sortingStep.Reverse();
                return FindImgInStep(sortingStep);
            }
            return "~/image/not foto2.png";
        }

        private static string FindImgInStep(List<Step> steps)
        {
            foreach (var step in steps)
            {
                if (step.Blocks != null)
                {
                    return FindImgInBlock(step.Blocks.ToList());
                }
            }
            return "~/image/not foto2.png";
        }

        private static string FindImgInBlock(List<Block> blocks)
        {
            blocks.Reverse();
            foreach (var block in blocks)
            {
                if (block.Type == "Image")
                    return block.Name;
            }
            return "~/image/not foto2.png";
        }

    }
}