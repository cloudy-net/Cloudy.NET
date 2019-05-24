using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNet.Features.NewsSupport
{
    [ContentType("NewsItem")]
    public class NewsItem : IContent, INameable
    {
        string IContent.ContentTypeId { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        [UIHint("image")]
        public string Image { get; set; }
        [UIHint("TinyMCE")]
        public string Text { get; set; }
    }
}
