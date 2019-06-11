using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [ContentType("43b1eea2-9b7f-478e-8aca-f3cee8ce3f5a")]
    public class Page : IContent, IRoutable, IHierarchical, INameable
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }
        public string ParentId { get; set; }
        public string UrlSegment { get; set; }
        public string Name { get; set; }

        [UIHint("textarea")]
        public string Text { get; set; }
    }
}
