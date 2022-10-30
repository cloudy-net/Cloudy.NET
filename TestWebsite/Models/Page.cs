using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.UI.FieldTypes.MediaPicker;
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
        [MediaPicker("azure")]
        public string MainImage { get; set; }
        [UIHint("html")]
        public string MainBody { get; set; }
        [ListFilter]
        public Category? Category { get; set; }
        public int Integer { get; set; }
        public int? NullableInteger { get; set; }
        public double Double { get; set; }
        public double? NullableDouble { get; set; }
        public DateTime DateTime { get; set; }
        public DateTimeOffset DateTimeOffset { get; set; }
        [UIHint("date")]
        public DateTime DateTimeWithDate { get; set; }
        [UIHint("date")]
        public DateTimeOffset DateTimeOffsetWithDate { get; set; }
        [UIHint("time")]
        public DateTime DateTimeWithTime { get; set; }
        [UIHint("time")]
        public DateTimeOffset DateTimeOffsetWithTime { get; set; }
        public DateOnly DateOnly { get; set; }
        public TimeOnly TimeOnly { get; set; }
        public bool Checkbox { get; set; }
        public bool? NullableCheckbox { get; set; }
    }
}
