using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.EntitySupport
{
    public interface IHierarchy<TKeyType>
    {
        public string? ParentType { get; set; }
        public TKeyType? ParentId { get; set; }
        public string EntityType { get; set; }
        public TKeyType EntityId { get; set; }
        public string? Name { get; set; }
        public string? UrlPath { get; set; }
        public int? SortIndex { get; set; }
        public string? SortOrder { get; set; }
        public IList<Tuple<string, TKeyType>> Ancestors { get; set; }
        public IList<Tuple<string, TKeyType>> Children { get; set; }
    }
}
