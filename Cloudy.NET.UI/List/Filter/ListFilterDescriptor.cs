using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List.Filter
{
    public record ListFilterDescriptor(
        string Name,
        string Label,
        string EntityType,
        bool Select,
        string SelectType,
        bool SimpleKey,
        int Order
    );
}
