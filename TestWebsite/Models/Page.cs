using Cloudy.CMS.EntitySupport;
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
    [Display(Description = "Create pages for your website.")]
    public class Page : INameable, IRoutable, IImageable, IHierarchical<Guid?>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }
        [ListColumn(Order = 0, Sortable = true)]
        public string Name { get; set; }
        [Select(typeof(Page))]
        public Guid? Parent { get; set; }
        public string UrlSegment { get; set; }
        [UIHint("textarea")]
        public string Description { get; set; }
        [ListFilter]
        [ListColumn]
        [Select(typeof(Page))]
        public Guid? RelatedPageId { get; set; }
        [MediaPicker("azure")]
        public string Image { get; set; }
        [UIHint("html")]
        public string MainBody { get; set; }
        [ListFilter]
        public Category? Category { get; set; }
        //[Block(typeof(Page))]
        //public IList<LayoutItem> Test { get; set; }
        public Link Link { get; set; }
    }

    public class BlockAttribute : Attribute
    {
        public Type Type { get; }

        public BlockAttribute(Type type)
        {
            Type = type;
        }
    }

    public class LayoutItem : IBlockItem<Guid>
    {
        public Guid ItemId { get; set; }
        public Guid Item { get; set; }
        public ColumnWidth Width { get; set; }
        public string Title { get; set; }
    }

    public enum ColumnWidth
    {
        Full = 0,
        Half = 1,
        Third = 2,
    }

    public interface IBlockItem<T>
    {
        Guid ItemId { get; set; }
        T Item { get; }
    }

    public class Link
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
