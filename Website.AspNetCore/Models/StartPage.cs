using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.SingletonSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [Singleton("start-page")]
    [ContentType("start-page")]
    public class StartPage : IContent, IRoutable
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }
        public string UrlSegment { get; set; }
        public string MySetting { get; set; }
    }
}
