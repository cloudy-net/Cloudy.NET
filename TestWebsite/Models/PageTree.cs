using Cloudy.CMS.EntitySupport.HierarchySupport;
using System;
using System.Collections.Generic;

namespace TestWebsite.Models
{
    public class PageTree : IHierarchyNode<Guid?>
    {
        public Guid? Id { get; set; }
        public string ParentType { get; set; }
        public Guid? ParentId { get; set; }
        public string EntityType { get; set; }
        public Guid? EntityId { get; set; }
        public string Name { get; set; }
        public string UrlPath { get; set; }
        public int? SortIndex { get; set; }
        public string SortOrder { get; set; }
        public IList<Tuple<string, Guid?>> Ancestors { get; set; }
        public IList<Tuple<string, Guid?>> Children { get; set; }
    }
}
