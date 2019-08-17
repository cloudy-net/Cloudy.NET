using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Controllers
{
    public class TestController
    {
        public string Index(string route, [FromContentRoute] IContent content)
        {
            return $"Lorem ipsum ({route} matched {content.GetType()})";
        }
    }
}
