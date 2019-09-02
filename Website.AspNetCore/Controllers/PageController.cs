using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS;
using Cloudy.CMS.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.AspNetCore.Models;
using Cloudy.CMS.ContentSupport;

namespace Website.AspNetCore.Controllers
{
    public class PageController : Controller
    {
        public ActionResult Index([FromContentRoute] IContent page)
        {
            return Content(page.Id);
        }

        public ActionResult Blog([FromContentRoute] Page page)
        {
            return View("Page", page);
        }

        public ActionResult Start([FromContentRoute] StartPage page)
        {
            return View("Start", page);
        }
    }
}
