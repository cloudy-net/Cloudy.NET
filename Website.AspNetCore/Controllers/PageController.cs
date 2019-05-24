using Microsoft.AspNetCore.Mvc;
using Cloudy.CMS;
using Cloudy.CMS.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Website.AspNetCore.Features.ArticlePageSupport;

namespace Website.AspNetCore.Controllers
{
    public class PageController : Controller
    {
        [ContentRoute(typeof(ArticlePage))]
        public ActionResult Index([FromContentRoute]ArticlePage articlePage)
        {
            return Content(articlePage.Name);
        }
    }
}
