using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntitySupport.HierarchySupport;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.UI.FieldSupport.Select;
using System;

namespace TestWebsite.Models
{
    [AllowedChildren<Page>]
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
