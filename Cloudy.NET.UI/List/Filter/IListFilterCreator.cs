using System;
using System.Collections.Generic;

namespace Cloudy.NET.UI.List.Filter
{
    public interface IListFilterCreator
    {
        IDictionary<Type, IEnumerable<ListFilterDescriptor>> Create();
    }
}