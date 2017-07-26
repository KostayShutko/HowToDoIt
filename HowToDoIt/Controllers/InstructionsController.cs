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
        public ActionResult Step(int step,int idInstruction,int idStep)
        {
            Step s=null;
            using (var db = new ApplicationDbContext())
            {
                if (idStep == 0)
                {
                    s = CreateNewStep(db, idInstruction, step);
                    idStep = (db.Steps.OrderByDescending(u => u.Id).FirstOrDefault()).Id;
                }
                else
                {
                    s = db.Steps.Find(idStep);
                    ViewBag.Blocks = s.Blocks;
                }
            }
            MakeDataForStepView(step, idInstruction, idStep);
            return View(s);
        }

        private void MakeDataForStepView(int step, int idInstruction, int idStep)
        {
            ViewBag.Step = step;
            ViewBag.IdInstruction = idInstruction;
            ViewBag.IdStep = idStep;
        }

        private Step CreateNewStep(ApplicationDbContext db, int idInstruction, int step)
        {
            var instruction = db.Instructions.Find(idInstruction);
            Step s = new Step();
            s.Number = step;
            instruction.Steps.Add(s);
            db.Entry(instruction).State = EntityState.Modified;
            db.SaveChanges();
            return s;
            
        }

        public void SaveStep(Step Step)
        {
            using (var db = new ApplicationDbContext())
            {
                UpdateStep(db,Step);
                foreach (var block in Step.Blocks)
                {
                    db.Blocks.Add(block);
                }
                db.SaveChanges();
            }
        }

        private void UpdateStep(ApplicationDbContext db, Step Step)
        {
            var s = db.Steps.Find(Step.Id);
            s.Name = Step.Name;
            s.Number = Step.Number;
            db.Entry(s).State = EntityState.Modified;
            DeleteAllBlocks(db, s);
        }

        public ActionResult Instruction(int? instructionid)
        {
            Instruction instruction=null;
            using (var db = new ApplicationDbContext())
            {
                if ((instructionid == null) || (instructionid == 0))
                    instruction = new Models.Classes_for_Db.Instruction();
                else
                {
                    var inst = db.Instructions.Find(instructionid);
                    WriteDataInViewBag(inst);
                    instruction = inst;
                }
                WriteCatedoryAndTagInViewBag(db);
            }
            return View(instruction);
        }

        private void WriteCatedoryAndTagInViewBag(ApplicationDbContext db)
        {
            ViewBag.Categories = db.Categories.ToArray();
            ViewBag.Tags = db.Tags.ToArray();
        }

        private void WriteDataInViewBag(Instruction inst)
        {
            ViewBag.TagString = "";
            ViewBag.Step = inst.Steps;
            ViewBag.CountStep = inst.Steps.Count;
            foreach (var tag in inst.Tags)
            {
                ViewBag.TagString += tag.Name + ", ";
            }
        }

        private void DeleteAllBlocks(ApplicationDbContext db,Step Step)
        {
            if (Step.Blocks != null)
                db.Blocks.RemoveRange(Step.Blocks);

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

        private void UpdateDataInInstruction(ApplicationDbContext db, Instruction instruction)
        {
            var instr = db.Instructions.Find(instruction.Id);
            DeleteNotExistTag(db, instruction, instr);
            WriteDataInInstruction(db, instruction, instr);
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
                else
                {
                    UpdateDataInInstruction(db, instruction);
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
                else
                {
                    UpdateDataInInstruction(db, instruction);
                    id = instruction.Id;
                }
            }
            return Json(new { success = true, Id = id }, JsonRequestBehavior.AllowGet);
        }

        private void DeleteNotExistTag(ApplicationDbContext db,Instruction instruction, Instruction instr)
        {
            List<Tag> list = new List<Tag>();
            foreach(var i in instr.Tags)
            {
                if (!(instruction.Tags.Any(c => c.Name == i.Name)))
                    list.Add(i);
            }
            foreach(var tag in list)
            {
                instr.Tags.Remove(tag);
            }
        }



        public JsonResult Upload()
        {
            try
            {
                string fileName = "";
                fileName = Manager.UploadFile(Request, Server, "~/Files/");
                return Json(new { success = true, responseText = "~/Files/" + fileName }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception) { return null; }
        }

        public void DeleteStep(int num, int instructionid)
        {
            using (var db = new ApplicationDbContext())
            {
                var instr = db.Instructions.Find(instructionid);
                Step stepForDelete = GetStepForDelete(db,num, instr);
                db.Steps.Remove(stepForDelete);
                db.SaveChanges();
            }
        }

        private Step GetStepForDelete(ApplicationDbContext db,int num, Instruction instr)
        {
            bool fl = false;
            Step stepForDelete = null;
            foreach (var step in instr.Steps.ToList())
            {
                if (fl)
                {
                    step.Number--;
                    db.Entry(step).State = EntityState.Modified;
                }
                if ((step.Number == num)&&(fl==false))
                {
                    stepForDelete = step;
                    fl = true;
                }
            }
            return stepForDelete;
        }
    }
}