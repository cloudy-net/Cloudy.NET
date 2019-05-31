using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [Container("users")]
    [ContentType("a4b8fd79-2432-4535-8ab8-5860c3bdb04d")]
    public class User : IContent
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }

        public string Username { get; set; }
    }
}
