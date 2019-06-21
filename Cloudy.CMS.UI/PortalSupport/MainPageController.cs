using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.PortalSupport
{
    [Area("Cloudy.CMS.Admin")]
    public class MainPageController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
