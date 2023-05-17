using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{
    public record ListColumnDescriptor(
        string Name,
        string Label,
        string Partial,
        int Order,
        bool Sortable,
        ListingColumnWidth width,
        bool ShowInCompactView
    );
}
