using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.EntitySupport
{
    public interface IHierarchy<T>
    {
        T EntityId { get; set; }
        T ParentId { get; set; }
        IList<T> Ancestors { get; set; }
        IList<T> Children { get; set; }
    }
}
