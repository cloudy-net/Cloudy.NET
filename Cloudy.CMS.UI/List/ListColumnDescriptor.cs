using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public record ListColumnDescriptor(
        string Name,
        string Label,
        string Partial,
        int Order,
        bool Sortable,
        ListingColumnWidth width
    );
}
