using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.FieldSupport.CustomSelect;
using Cloudy.CMS.UI.FieldSupport.MediaPicker;
using Cloudy.CMS.UI.FieldSupport.Select;
using Cloudy.CMS.UI.List;
using Cloudy.CMS.UI.List.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWebsite.Factories;

namespace TestWebsite.Models
{
    [Display(Description = "Create pages for your website.")]
    public class Page : INameable, IRoutable, IImageable, IHierarchical<Guid?>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid? Id { get; set; }
        [ListColumn(Order = 0, Sortable = true, Width = ListingColumnWidth.Fill)]
        public string Name { get; set; }

        [Select<Page>]
        public Guid? Parent { get; set; }
        public string UrlSegment { get; set; }
        [UIHint("textarea")]
        public string Description { get; set; }
        [ListFilter]
        [ListColumn]
        [Select<Page>]
        public Guid? RelatedPageId { get; set; }
        [MediaPicker("azure")]
        public string Image { get; set; }
        [UIHint("html")]
        public string MainBody { get; set; }
        [ListFilter]
        public Category? Category { get; set; }
        //[Block(typeof(Page))]
        //public IList<LayoutItem> Test { get; set; }
        public IFrontpageBlock FrontpageBlock { get; set; }

        [ListColumn]
        [CustomSelect<IColorFactory>]
        [Display(Name = "This is the 2nd color prop", Prompt = "Pick something!")]
        public string Color { get; set; }

        [ListColumn]
        [RequiredInput]
        [CustomSelect<IColorFactory>]
        [Display(Description = "This is required but validation is yet to come...", Prompt = "Pick something!")]
        public string SecondColor { get; set; }

        [ListColumn]
        [CustomSelect<IColorFactory>(Multi = true)]
        public IList<string> Colors { get; set; }
    }

    public interface IFrontpageBlock { }

    public class HeroBlock : IFrontpageBlock
    {
        public string Heading { get; set; }
        [MediaPicker("azure")]
        public string Image { get; set; }
    }

    public class TextBlock : IFrontpageBlock
    {
        public string Heading { get; set; }
        [UIHint("textarea")]
        public string Text { get; set; }
    }

    //public class BlockAttribute : Attribute
    //{
    //    public Type Type { get; }

    //    public BlockAttribute(Type type)
    //    {
    //        Type = type;
    //    }
    //}

    //public class LayoutItem : IBlockItem<Guid>
    //{
    //    public Guid ItemId { get; set; }
    //    public Guid Item { get; set; }
    //    public ColumnWidth Width { get; set; }
    //    public string Title { get; set; }
    //}

    //public enum ColumnWidth
    //{
    //    Full = 0,
    //    Half = 1,
    //    Third = 2,
    //}

    //public interface IBlockItem<T>
    //{
    //    Guid ItemId { get; set; }
    //    T Item { get; }
    //}

    public class Link
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
