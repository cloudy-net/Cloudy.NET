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
    [DebuggerDisplay("{Id}")]
    public record ContentTypeDescriptor(string Id, Type Type, bool IsNameable = false, bool IsImageable = false, bool IsRoutable = false)
    {
    }
}
