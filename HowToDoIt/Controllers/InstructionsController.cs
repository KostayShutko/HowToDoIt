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
        public ActionResult Step()
        {
            return View();
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

        public void UpdateCategory(ApplicationDbContext db,Instruction instr, Instruction instruction,ICollection<Category> categories)
        {
            foreach (var c in categories)
            {
                if (c.Name == instruction.Category.Name)
                {
                    c.Instructions.Add(instr);
                    db.Entry(c).State = EntityState.Modified;
                }
            }
        }

        public void SaveInstruction(Instruction instruction)
        {
            using (var db = new ApplicationDbContext())
            {
                if (instruction.Id==0)
                {
                    Instruction instr = new Instruction();
                    instr.Name = instruction.Name;
                    instr.Date= DateTime.Now.ToString("dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    UpdateCategory(db, instr, instruction, db.Categories.ToArray());
                    
                    db.Instructions.Add(instr);
                    db.SaveChanges();
                }
            }
        }
    }
}