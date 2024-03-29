﻿using Microsoft.AspNetCore.Mvc;
using Cloudy.NET;
using Cloudy.NET.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TestWebsite.Models;

namespace TestWebsite.Controllers
{
    public class PageController : Controller
    {
        public ActionResult Index([FromContentRoute] Page page)
        {
            return View(page);
        }
    }
}
