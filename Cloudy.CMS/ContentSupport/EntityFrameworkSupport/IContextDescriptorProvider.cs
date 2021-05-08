using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IContextDescriptorProvider
    {
        ContextDescriptor GetFor(Type type);
    }
}