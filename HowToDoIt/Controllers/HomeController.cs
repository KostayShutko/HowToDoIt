using HowToDoIt.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Nest;
using HowToDoIt.Models;
using Elasticsearch.Net;
using HowToDoIt.Models.Classes_for_Db;

namespace HowToDoIt.Controllers
{
    [Culture]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            /*var node = new Uri("http://localhost:9200");
            var settings = new ConnectionSettings(node);
            settings.DefaultIndex("HowToDo");
            var client = new ElasticClient(settings);

            using (var db = new ApplicationDbContext())
            {
                var instruction = db.Instructions.ToList();
                foreach (var i in instruction)
                {
                    //client.IndexAsync<Instruction>(i, null);
                    client.Index(i, idx => idx.Index(i.Id.ToString()));
                }
            }

            var result = client.Search<Models.Classes_for_Db.Instruction>(s => s
                   .From(0)
                   .Take(10)
                   .Query(qry => qry
                       .Bool(b => b
                           .Must(m => m
                               .QueryString(qs => qs
                                   .DefaultField("_all")
                                   .Query("10"))))));
            var list = result.Documents.ToList();*/
            return View();
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