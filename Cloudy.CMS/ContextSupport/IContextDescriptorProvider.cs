using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContextSupport
{
    public interface IContextDescriptorProvider
    {
        IEnumerable<ContextDescriptor> GetAll();
        ContextDescriptor GetFor(Type type);
    }
}