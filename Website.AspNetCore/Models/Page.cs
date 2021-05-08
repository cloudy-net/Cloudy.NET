using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [ContentType("80ca82a2-b46e-4394-88e0-a77ae93a9366")]
    public class Page
    {
        public string Id { get; set; }
        public string Description { get; set; }
    }
}
