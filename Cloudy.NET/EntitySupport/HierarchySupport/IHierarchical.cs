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
