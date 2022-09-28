using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.List
{
    public interface IListColumnCreator
    {
        IDictionary<Type, IEnumerable<ListColumnDescriptor>> Create();
    }
}