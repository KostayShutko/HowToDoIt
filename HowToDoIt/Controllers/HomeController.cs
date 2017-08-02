using HowToDoIt.Filters;
using HowToDoIt.Models.Classes_for_Db;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nest;
using HowToDoIt.Models;
using Elasticsearch.Net;
using HowToDoIt.Models.Sort;

namespace HowToDoIt.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            List<Instruction> instructions = GetSortingInstruction("HowToDoIt.Models.Sort.SortingByDate");
            return View(instructions);
        }

        public ActionResult SortingHome(string nameClass)
        {
            return PartialView("~/Views/Instructions/_ViewInstructionPartial.cshtml", GetSortingInstruction(nameClass));          
        }

        private List<Instruction> GetSortingInstruction(string nameClass)
        {
            using (var db = new ApplicationDbContext())
            {
                List<Instruction> instructions = SortInstruction(db, nameClass);
                CreateDataInViewBag(instructions);
                return instructions;
            }
        }

        private List<Instruction> SortInstruction(ApplicationDbContext db, string nameClass)
        {
            ISorting classSort = Activator.CreateInstance(Type.GetType(nameClass)) as ISorting;
            List<Instruction> instructions = db.Instructions.ToList();
            return (classSort.Sorting(instructions)).Take(5).ToList();
        }

        private void CreateDataInViewBag(List<Instruction> instructions)
        {
            List<Category> listCategory = new List<Category>();
            List<ApplicationUser> listUser = new List<ApplicationUser>();
            Manager.ChangeImgInInstruction(instructions, listUser, listCategory);
            WriteCategoryAndUserToViewBag(listUser, listCategory);
        }

        private void WriteCategoryAndUserToViewBag(List<ApplicationUser> listUser, List<Category> listCategory)
        {
            ViewBag.Category = listCategory;
            ViewBag.Author = listUser;
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            // Список культур
            List<string> cultures = new List<string>() { "ru", "en" };
            if (!cultures.Contains(lang))
            {
                lang = "ru";
            }
            // Сохраняем выбранную культуру в куки
            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
                cookie.Value = lang;   // если куки уже установлено, то обновляем значение
            else
            {

                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }
            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }
    }
}