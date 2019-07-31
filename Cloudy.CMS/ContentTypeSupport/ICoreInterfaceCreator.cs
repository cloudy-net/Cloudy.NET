using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface ICoreInterfaceCreator
    {
        IEnumerable<CoreInterfaceDescriptor> Create();
    }
}
