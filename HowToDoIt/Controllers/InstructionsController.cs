using HowToDoIt.Filters;
using HowToDoIt.Models;
using HowToDoIt.Models.Classes_for_Db;
using HowToDoIt.Models.Sort;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace HowToDoIt.Controllers
{
    [Culture]
    public class InstructionsController : Controller
    {

        public JsonResult AddComment(int idInstruction,string content)
        {
            using (var db = new ApplicationDbContext())
            {
                AddCoomentToDB(db, idInstruction, content);
                db.SaveChanges();
                return Json(new { Status = "200", Message = "Ok" }, JsonRequestBehavior.AllowGet);
            }
        }

        private void AddCoomentToDB(ApplicationDbContext db, int idInstruction, string content)
        {
            var instruction = db.Instructions.Find(idInstruction);
            ApplicationUser user = Manager.GetCurrentUser(db, User.Identity.Name);
            Comment comment = CreateComment(instruction, user, content);
            SendCommentOtherUser(idInstruction, comment);
            db.Comments.Add(comment);
        }

        private void SendCommentOtherUser(int idInstruction, Comment comment)
        {
            var avatar = comment.User.Profil.Avatar;
            var idUser = comment.User.Id;
            var userName = comment.User.UserName;
            var date = comment.Date;
            var text = comment.Text;
            var hub = GlobalHost.ConnectionManager.GetHubContext<commetsHub>();
            hub.Clients.Group(idInstruction.ToString()).createComment(avatar,userName,date,text, idUser);
        }

        private Comment CreateComment( Instruction instruction, ApplicationUser user, string content)
        {
            Comment comment = new Comment();
            comment.Date = DateTime.Now.ToString();
            comment.Text = content;
            comment.User = user;
            comment.Instruction = instruction;
            return comment;
        }

        [System.Web.Mvc.Authorize(Roles = "admin")]
        public ActionResult DeleteInstruction(int instructionid)
        {
            using (var db = new ApplicationDbContext())
            {
                var instruction = db.Instructions.Find(instructionid);
                db.Instructions.Remove(instruction);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
        }

        public void Rating(int countStar,int instructionid)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Ratings.Add(SaveInfoRating(countStar, instructionid));
                db.SaveChanges();
            }
        }

        private Rating SaveInfoRating(int countStar, int instructionid)
        {
            Rating r = new Rating();
            r.Mark = countStar;
            r.UserLogin = User.Identity.Name;
            r.InstructionId = instructionid;
            return r;
        }

        public ActionResult ViewInstruction(int idInstruction)
        {
            using (var db = new ApplicationDbContext())
            {
                var instruction = db.Instructions.Find(idInstruction);
                ViewBagRating(instruction);
                UpdateDataForViewInstruction(instruction);
                return View(instruction);
            }
        }

        private void ViewBagRating(Instruction instruction)
        {
            if (User.Identity.Name != "")
            {
                ViewBag.Rating = GetRatingCurrentUser(instruction);
            }
            else
            {
                ViewBag.Rating = null;
            }
        }
        
        private Rating GetRatingCurrentUser(Instruction instruction)
        {
            var ratings = instruction.Ratings.ToList();
            return ratings.Where(c => c.UserLogin == User.Identity.Name).FirstOrDefault();
        }
        
        private void UpdateDataForViewInstruction(Instruction instruction)
        {
            var user = instruction.User.UserName;
            var steps = instruction.Steps;
            var comment = instruction.Comments;
            foreach (var c in comment)
            {
                var t = c.User.Profil;
            }
            var tags = instruction.Tags;
            foreach(var step in steps)
            {
                var blocks = step.Blocks;
            }
            instruction.Steps = steps.OrderBy(c => c.Number).ToList();
        }


    //---------------------------------------------

        public ActionResult FilterByCategory(int idCategory)
        {
            var instr = ((List<Instruction>)(Session["instructions-original"])).ToList();
            var sort = from c in instr where c.CategoryId == idCategory select c;
            Session["instructions"] = sort.ToList();
            return ViewInstructions(0);
        }

        

        public ActionResult Sorting(string nameClass)
        {
            ISorting classSort= Activator.CreateInstance(Type.GetType(nameClass)) as ISorting;
            Session["classSort"] = classSort;
            Session["instructions"] = Session["instructions-original"]; 
            return ViewInstructions(0);
        }


        public ActionResult SearchInstruction()
        {
            using (var db = new ApplicationDbContext())
            {
                SetSession(db.Instructions.ToList());
            }
            return ViewInstructions(0);
        }

        public ActionResult History()
        {
            using (var db = new ApplicationDbContext())
            {
                ApplicationUser user = Manager.GetCurrentUser(db, User.Identity.Name);
                SetSession( user.Instructions.ToList());
            }
            return ViewInstructions(0);
        }

        public ActionResult SearchByCategory(int idInstruction)
        {
            using (var db = new ApplicationDbContext())
            {
                SetSession(db.Instructions.Find(idInstruction).Category.Instructions.ToList());
            }
            return ViewInstructions(0);
        }

        public ActionResult SearchTag(int idTag)
        {
            using (var db = new ApplicationDbContext())
            {
                SetSession(db.Tags.Find(idTag).Instructions.ToList());
            }
            return ViewInstructions(0);
        }

        private void SetSession(List<Instruction> inst)
        {
            Session["instructions"] = inst;
            Session["instructions-original"] = inst;
            Session["classSort"] = null;
        }

        public ActionResult ViewInstructions(int? id)
        {
            int page = id ?? 0;
            if (Request.IsAjaxRequest())
            {
                return PartialView("_ViewInstructionPartial", GetItemsPage(page));
            }
            return View("ViewInstructions", GetItemsPage(page));
        }

        private List<Instruction> GetItemsPage(int page = 1)
        {
            using (var db = new ApplicationDbContext())
            {
                var itemsToSkip = page * 5;
                ViewBag.Categories = db.Categories.ToArray();
                return AddImageToInstruction(db,itemsToSkip);
            }
        }

        private List<Instruction> AddImageToInstruction(ApplicationDbContext db,int itemsToSkip)
        {
            var instructions = GetInstructionFromSession(db, itemsToSkip);
            List<Category> listCategory = new List<Category>();
            List<ApplicationUser> listUser = new List<ApplicationUser>();
            ChangeImgInInstruction(instructions, listUser, listCategory);
            return instructions;
        }

        private List<Instruction> GetInstructionFromSession(ApplicationDbContext db, int itemsToSkip)
        {
            var listOfRoleId = ((List<Instruction>)(Session["instructions"])).Select(r => r.Id);
            var instr = db.Instructions.Where(r => listOfRoleId.Contains(r.Id));
            List<Instruction> listInstr = ImplementInterfaceBySort(instr, itemsToSkip);
            return SkipTake(listInstr, itemsToSkip);
        }

        private List<Instruction> ImplementInterfaceBySort(IQueryable<Instruction> instr, int itemsToSkip)
        {
            List<Instruction> listInstr;
            if (Session["classSort"] != null)
            {
                ISorting s = Session["classSort"] as ISorting;
                listInstr = s.Sorting(instr.ToList());
                if (itemsToSkip + 5 >= listInstr.Count)
                    Session["classSort"] = null;
            }
            else
                listInstr = instr.ToList();
            return listInstr;
        }

        private List<Instruction> SkipTake(List<Instruction> list, int itemsToSkip)
        {
            List<Instruction> newlist = new List<Models.Classes_for_Db.Instruction>();
            int k = 5;
            if (list.Count < itemsToSkip + 5)
                k = list.Count - itemsToSkip;
            for (int i = itemsToSkip; i < itemsToSkip + k; i++)
                newlist.Add(list[i]);
            return newlist;
        }

        private void ChangeImgInInstruction(List<Instruction> instructions, List<ApplicationUser> listUser, List<Category> listCategory)
        {
            for (int i = 0; i < instructions.Count; i++)
            {
                listUser.Add(instructions[i].User);
                listCategory.Add(instructions[i].Category);
                instructions[i].Image = FindImg(instructions[i]);
            }
            ViewBag.Category = listCategory;
            ViewBag.Author = listUser;
        }

        private string FindImg(HowToDoIt.Models.Classes_for_Db.Instruction instr)
        {
            if (instr.Steps != null)
            {
                var sortingStep= (instr.Steps.OrderBy(c => c.Number).ToList());
                sortingStep.Reverse();
                return FindImgInStep(sortingStep);
            }
            return "~/image/not foto2.png";
        }

        private string FindImgInStep(List<Step> steps)
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

        private string FindImgInBlock(List<Block> blocks)
        {
            blocks.Reverse();
            foreach (var block in blocks)
            {
                if (block.Type == "Image")
                    return block.Name;
            }
            return "~/image/not foto2.png";
        }



        //-----------------------------------------------------------------------
        [System.Web.Mvc.Authorize]
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

        [System.Web.Mvc.Authorize]
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
            this.Response.Cache.SetCacheability(System.Web.HttpCacheability.NoCache);
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
            var step = inst.Steps;
            ViewBag.Step = step.OrderBy(c=>c.Number).ToList();
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

        private void AddDataInInstruction(ApplicationDbContext db, Instruction instruction, Instruction instr)
        {
            WriteDataInInstruction(db, instruction, instr);
            AddUserByInstruction(db, instr);
        }

        private void CreateNewInstruction(ApplicationDbContext db, Instruction instruction)
        {
            Instruction instr = new Instruction();
            AddDataInInstruction(db, instruction, instr);
            db.Instructions.Add(instr);
            db.SaveChanges();
        }

        private void AddUserByInstruction(ApplicationDbContext db, Instruction instruction)
        {
            ApplicationUser user = Manager.GetCurrentUser(db, User.Identity.Name);
            instruction.User = user;
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

        public JsonResult MoveStep(int numstart, int numend, int instructionid)
        {
            using (var db = new ApplicationDbContext())
            {
                var instr = db.Instructions.Find(instructionid);
                MoveSteps(db, numstart, numend, instr);
                db.SaveChanges();
                return Json(new { success = true, responseText = GetStepId(db.Instructions.Find(instructionid)) }, JsonRequestBehavior.AllowGet);
            }
            
        }

        private int[] GetStepId(Instruction instr)
        {
            List<int> list = new List<int>();
            var steps = instr.Steps.ToList();
            for (int i=0;i<steps.Count;i++)
            {
                list.Add(steps[i].Id);
            }
            return list.ToArray();
        }

        private void MoveSteps(ApplicationDbContext db, int numstart, int numend, Instruction instr)
        {
            var listForDeleteFromDb = instr.Steps.ToList();

            var list = instr.Steps.ToList();
            list= list.OrderBy(c => c.Number).ToList();
            ShiftStepInList(list, numstart, numend);

            List<List<Block>> listblocks= GetBlocks(list);
            db.Steps.RemoveRange(listForDeleteFromDb);
            db.SaveChanges();
            db.Steps.AddRange(list); 
            db.SaveChanges();
            RenameBlocks(db, listblocks, instr);
        }

        private void RenameBlocks(ApplicationDbContext db, List<List<Block>> blocks, Instruction instr)
        {
            var instruction = db.Instructions.Find(instr.Id);
            var list = instruction.Steps.ToList();
            list = list.OrderBy(c => c.Number).ToList();
            for (int i = 0; i < list.Count; i++)
            {
                RenameBlockId(db, blocks[i], list[i].Id);
            }
        }

        private void ShiftStepInList(List<Step> list, int numstart, int numend)
        {
            Step step = list.Single(s => s.Number == numstart + 1);
            list.Remove(step);
            list.Insert(numend, step);
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Number = i + 1;
            }
        }

        private List<List<Block>> GetBlocks(List<Step> list)
        {
            List<List<Block>> listblocks = new List<List<Block>>();
            for (int i = 0; i < list.Count; i++)
            {
                listblocks.Add(list[i].Blocks.ToList());
            }
            return listblocks;
        }

        private void RenameBlockId(ApplicationDbContext db,List<Block> listblock,int id)
        {
            foreach(var b in listblock)
            {
                b.StepId = id;
                db.Blocks.Add(b);
            }
        }

        private Step GetStepForDelete(ApplicationDbContext db,int num, Instruction instr)
        {
            bool fl = false;
            Step stepForDelete = null;
            var list = instr.Steps.ToList();
            list = list.OrderBy(c => c.Number).ToList();
            foreach (var step in list)
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