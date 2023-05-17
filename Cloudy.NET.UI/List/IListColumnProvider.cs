using System;
using System.Collections.Generic;

namespace Cloudy.NET.UI.List
{
    public interface IListColumnProvider
    {
        IEnumerable<ListColumnDescriptor> Get(Type type);
    }
}