using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    public interface IHierarchical
    {
        [Display(GroupName = "Settings")]
        object[] ParentKeyValues { get; set; }
    }
}
