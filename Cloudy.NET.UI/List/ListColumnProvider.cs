using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{
    public class ListColumnProvider : IListColumnProvider
    {
        IDictionary<Type, IEnumerable<ListColumnDescriptor>> ValuesByType { get; }

        public ListColumnProvider(IListColumnCreator listColumnCreator)
        {
            ValuesByType = new Dictionary<Type, IEnumerable<ListColumnDescriptor>>(listColumnCreator.Create());
        }

        public IEnumerable<ListColumnDescriptor> Get(Type type)
        {
            if (!ValuesByType.ContainsKey(type))
            {
                return null;
            }

            return ValuesByType[type];
        }
    }
}
