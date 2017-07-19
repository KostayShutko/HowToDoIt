using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HowToDoIt.Controllers
{
    public class InstructionsController : Controller
    {
        // GET: Instructions
        public ActionResult Step()
        {
            return View();
        }
    }
}