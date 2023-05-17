using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.List
{
    public interface IListColumnProvider
    {
        IEnumerable<ListColumnDescriptor> Get(Type type);
    }
}