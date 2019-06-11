using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNet.Models
{
    [ContentType("article-page")]
    public class ArticlePage : Page
    {
        [UIHint("textarea")]
        public string MainBody { get; set; }
    }
}
