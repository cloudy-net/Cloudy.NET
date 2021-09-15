using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport
{
    public interface IContextDescriptorProvider
    {
        ContextDescriptor GetFor(Type type);
    }
}