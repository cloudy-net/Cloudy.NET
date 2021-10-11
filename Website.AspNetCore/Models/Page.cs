using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore.Models
{
    [ContentType("80ca82a2-b46e-4394-88e0-a77ae93a9366")]
    public class Page : INameable, IRoutable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }
        public string Name { get; set; }
        [Display]
        public string UrlSegment { get; set; }
        [UIHint("textarea({rows:2})")]
        public string Description { get; set; }
        //[UIHint("html")]
        //public string MainBody { get; set; }
    }
}
