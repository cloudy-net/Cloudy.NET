using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Website.AspNet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return Content("Hej", "text/html");
        }
    }
}