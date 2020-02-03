using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore
{
    [ContentType("test")]
    public class TestContentItem : IContent
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }
    }
}
