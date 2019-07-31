using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface ICoreInterfaceProvider
    {
        CoreInterfaceDescriptor GetFor(Type type);
    }
}