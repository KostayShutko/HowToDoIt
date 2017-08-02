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

            return View();
        }

        public ActionResult SortingHome(string nameClass)
        {
            using (var db = new ApplicationDbContext())
            {
                ISorting classSort = Activator.CreateInstance(Type.GetType(nameClass)) as ISorting;
                List<Instruction> instructions = db.Instructions.ToList();
                instructions = classSort.Sorting(instructions);
                List<Category> listCategory = new List<Category>();
                List<ApplicationUser> listUser = new List<ApplicationUser>();
                Manager.ChangeImgInInstruction(instructions, listUser, listCategory);
                WriteCategoryAndUserToViewBag(listUser, listCategory);
                return PartialView("~/Views/Instructions/_ViewInstructionPartial.cshtml", instructions);
            }
                
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