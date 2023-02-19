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
        public T Parent { get; set; }
        public int? SortIndex { get; set; }
        public string SortOrder { get; set; }
    }
}
