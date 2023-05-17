using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List.Filter
{
    public class ListFilterProvider : IListFilterProvider
    {
        IDictionary<Type, IEnumerable<ListFilterDescriptor>> Values { get; }

        public ListFilterProvider(IListFilterCreator listFilterCreator)
        {
            Values = listFilterCreator.Create();
        }

        public IEnumerable<ListFilterDescriptor> Get(Type type)
        {
            if (!Values.ContainsKey(type))
            {
                return null;
            }

            return Values[type];
        }
    }
}
