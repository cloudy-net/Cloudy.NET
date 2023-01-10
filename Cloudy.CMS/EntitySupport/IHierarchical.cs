using Cloudy.CMS.EntitySupport.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.EntitySupport
{
    public interface IHierarchical<T> : IHierarchicalMarkerInterface
    {
        T Parent { get; set; }
    }
}
