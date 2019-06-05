using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNet.Models
{
    [ContentType("article-page")]
    public class ArticlePage : IContent, IRoutable, INameable
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }
        public string UrlSegment { get; set; }
        public string Name { get; set; }

        [UIHint("textarea")]
        public string MainBody { get; set; }
    }
}
