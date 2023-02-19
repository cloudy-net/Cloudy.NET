using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntitySupport.HierarchySupport;
using System;

namespace TestWebsite.Models
{
    [AllowedChildren<Page>]
    public class StartPage : INameable, IRoutable, IHierarchical<Guid?>
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string UrlSegment { get; set; }
        public Guid? Parent { get; set; }
        public int? SortIndex { get; set; }
        public string SortOrder { get; set; }

    }
}
