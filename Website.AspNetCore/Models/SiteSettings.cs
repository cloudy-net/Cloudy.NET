using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.SingletonSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [Singleton("site-settings")]
    [ContentType("site-settings")]
    public class SiteSettings : IContent
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }
        public string SiteTitle { get; set; }
    }
}
