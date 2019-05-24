using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Controllers
{
    public class HomeController
    {
        public ActionResult Index()
        {
            return new ContentResult
            {
                Content = "Hej",
                ContentType = "text/html",
                StatusCode = 200,
            };
        }
    }
}
