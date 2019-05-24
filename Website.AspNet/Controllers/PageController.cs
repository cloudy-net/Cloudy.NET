using Cloudy.CMS;
using Cloudy.CMS.AspNet.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Website.AspNet.Features.ArticlePageSupport;

namespace Website.AspNet.Controllers
{
    public class PageController : Controller
    {
        [ContentRoute(typeof(ArticlePage))]
        public ActionResult Index([FromContentRoute]ArticlePage articlePage)
        {
            return Content("HEJ: " + articlePage.Name);
        }
    }
}
