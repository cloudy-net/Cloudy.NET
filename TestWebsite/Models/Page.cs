using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.UI.FormSupport.FieldTypes;
using Cloudy.CMS.UI.List;
using Cloudy.CMS.UI.List.Filter;
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
        [ListColumn]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }
        [ListColumn(Order = 0)]
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        [UIHint("textarea")]
        public string Description { get; set; }
        [ListFilter]
        [ListColumn]
        [Select(typeof(Page))]
        public Guid? RelatedPageId { get; set; }
        [UIHint("html")]
        public string MainBody { get; set; }
        [ListFilter]
        public Category? Category { get; set; }
    }
}
