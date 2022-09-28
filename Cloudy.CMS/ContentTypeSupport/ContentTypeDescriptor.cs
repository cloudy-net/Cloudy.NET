using Cloudy.CMS.ContentSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    [DebuggerDisplay("{Name}")]
    public record ContentTypeDescriptor(
        string Name,
        Type Type,
        bool IsNameable = false,
        bool IsImageable = false,
        bool IsRoutable = false
    );
}
