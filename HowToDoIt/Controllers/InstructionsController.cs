using HowToDoIt.Filters;
using HowToDoIt.Models;
using HowToDoIt.Models.Classes_for_Db;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HowToDoIt.Controllers
{
    [Culture]
    public class InstructionsController : Controller
    {
        // GET: Instructions
        public ActionResult Step(int step,int id)
        {
            return View();
        }

        public void SaveStep(Step step)
        {
            string str = "";
        }

        public ActionResult Instruction(Instruction instruction)
        {
            using (var db = new ApplicationDbContext())
            {
                ViewBag.Categories= db.Categories.ToArray();
                ViewBag.Tags= db.Tags.ToArray();
            }
            return View(instruction);
        }

        private void UpdateCategory(ApplicationDbContext db,Instruction instr, Instruction instruction,ICollection<Category> categories)
        {
            var category = (from b in db.Categories where b.Name == instruction.Category.Name select b).FirstOrDefault();
            category.Instructions.Add(instr);
            db.Entry(category).State = EntityState.Modified;
        }

        private void CreateTag(ApplicationDbContext db,string nameNewTag,Instruction instr)
        {
            Tag t = new Tag();
            t.Name = nameNewTag;
            db.Tags.Add(t);
            instr.Tags.Add(t);
        }

        private void UpdateTag(ApplicationDbContext db, Instruction instr, Instruction instruction, ICollection<Category> categories)
        {
            foreach (var element in instruction.Tags)
            {
                element.Name=element.Name.Trim();
                var tag = (from b in db.Tags where b.Name == element.Name select b).FirstOrDefault();
                if (tag != null)
                    instr.Tags.Add(tag);
                else
                    CreateTag(db, element.Name, instr);
            }
        }

        private void WriteDataInInstruction(ApplicationDbContext db, Instruction instruction, Instruction instr)
        {
            instr.Name = instruction.Name;
            instr.Date = DateTime.Now.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
            UpdateCategory(db, instr, instruction, db.Categories.ToArray());
            UpdateTag(db, instr, instruction, db.Categories.ToArray());
        }

        private void CreateNewInstruction(ApplicationDbContext db, Instruction instruction)
        {
            Instruction instr = new Instruction();
            WriteDataInInstruction(db, instruction, instr);
            db.Instructions.Add(instr);
            db.SaveChanges();
        }

        public JsonResult SaveInstruction(Instruction instruction)
        {
            using (var db = new ApplicationDbContext())
            {
                if (instruction.Id==0)
                {
                    CreateNewInstruction(db, instruction);
                }
            }
            return Json(new { success = false }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SaveInstructionForAddStep(Instruction instruction)
        {
            int id = 0;
            using (var db = new ApplicationDbContext())
            {
                if (instruction.Id == 0)
                {
                    CreateNewInstruction(db, instruction);
                    id = (db.Instructions.OrderByDescending(u => u.Id).FirstOrDefault()).Id;
                }
            }
            return Json(new { success = true, Id = id }, JsonRequestBehavior.AllowGet);
        }
    }
}