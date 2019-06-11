using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS;
using Cloudy.CMS.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.AspNetCore.Models;

namespace Website.AspNetCore.Controllers
{
    public class PageController : Controller
    {
        [ContentRoute(typeof(Page))]
        public ActionResult Blog([FromContentRoute] Page articlePage)
        {
            return View("Article", articlePage);
        }
    }
}
