using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace TestWebsite.Models
{
    [Display(Description = "This is a sample class to show off most bells and whistles of the CMS toolkit.")]
    public class Page : INameable, IRoutable
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string Name { get; set; }
        [Display]
        [RegularExpression(@"^[-_a-zA-Z0-9]*$", ErrorMessage = "Invalid URL")]
        public string UrlSegment { get; set; }
        [UIHint("textarea")]
        public string Description { get; set; }
        [UIHint("html")]
        public string MainBody { get; set; }
    }
}
