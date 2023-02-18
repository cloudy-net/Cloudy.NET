using Cloudy.CMS.EntitySupport;
using System;
using System.Collections.Generic;

namespace TestWebsite.Models
{
    public class PageTree : IHierarchy<int>
    {
        public string? ParentType { get; set; }
        public int ParentId { get; set; }
        public string EntityType { get; set; }
        public int EntityId { get; set; }
        public string? Name { get; set; }
        public string? UrlPath { get; set; }
        public int? SortIndex { get; set; }
        public string? SortOrder { get; set; }
        public IList<Tuple<string, int>> Ancestors { get; set; }
        public IList<Tuple<string, int>> Children { get; set; }
    }
}
