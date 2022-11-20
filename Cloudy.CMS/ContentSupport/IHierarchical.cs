using Cloudy.CMS.ContentSupport.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    public interface IHierarchical<T> : IHierarchicalMarkerInterface
    {
        T Parent { get; set; }
    }
}
