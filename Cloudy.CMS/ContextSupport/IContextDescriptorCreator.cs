using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContextSupport
{
    public interface IContextDescriptorCreator
    {
        IEnumerable<ContextDescriptor> Create(IEnumerable<Type> types);
    }
}