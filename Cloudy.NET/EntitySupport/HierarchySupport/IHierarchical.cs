using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cloudy.NET.EntitySupport.HierarchySupport.Internal;

namespace Cloudy.NET.EntitySupport.HierarchySupport
{
    public interface IHierarchical<T> : IHierarchicalMarkerInterface
    {
        [Display(AutoGenerateField = false)]
        public string ParentType { get; set; }
        [Display(AutoGenerateField = false)]
        public T ParentId { get; set; }
        [Display(AutoGenerateField = false)]
        public int? SortIndex { get; set; }
        [Display(AutoGenerateField = false)]
        public string SortOrder { get; set; }
    }
}
