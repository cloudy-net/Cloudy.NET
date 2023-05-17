using System;
using System.Collections.Generic;

namespace Cloudy.NET.UI.List
{
    public interface IListColumnCreator
    {
        IDictionary<Type, IEnumerable<ListColumnDescriptor>> Create();
    }
}