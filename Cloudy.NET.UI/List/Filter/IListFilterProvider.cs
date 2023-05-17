using System;
using System.Collections.Generic;

namespace Cloudy.NET.UI.List.Filter
{
    public interface IListFilterProvider
    {
        IEnumerable<ListFilterDescriptor> Get(Type type);
    }
}