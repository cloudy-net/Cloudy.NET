using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudy.CMS.EntitySupport.HierarchySupport.Internal;

namespace Cloudy.CMS.EntitySupport.HierarchySupport
{
    public interface IHierarchical<T> : IHierarchicalMarkerInterface
    {
        public string ParentType { get; set; }
        public T ParentId { get; set; }
        public int? SortIndex { get; set; }
        public string SortOrder { get; set; }
    }
}
