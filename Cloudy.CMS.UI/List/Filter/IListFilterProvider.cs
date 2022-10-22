using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.List.Filter
{
    public interface IListFilterProvider
    {
        IEnumerable<ListFilterDescriptor> Get(Type type);
    }
}