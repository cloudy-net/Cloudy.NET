using Cloudy.NET.EntitySupport;
using Cloudy.NET.EntitySupport.HierarchySupport;
using Cloudy.NET.SingletonSupport;
using Cloudy.NET.UI.FieldSupport.Select;
using System;
using System.ComponentModel.DataAnnotations;
using TestWebsite.Constants;

namespace TestWebsite.Models
{
    [AllowedChildren<Page>]
    [Display(GroupName = General.GroupNames.Page)]
    public class StartPage : INameable, IRoutable, IHierarchical<Guid?>, ISingleton
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public string ParentType { get; set; }
        [Select<Page>]
        public Guid? ParentId { get; set; }
        public int? SortIndex { get; set; }
        public string SortOrder { get; set; }

    }
}
