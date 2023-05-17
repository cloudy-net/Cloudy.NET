using System;
using System.Collections.Generic;

namespace Cloudy.NET.ContextSupport
{
    public interface IContextDescriptorCreator
    {
        IEnumerable<ContextDescriptor> Create(IEnumerable<Type> types);
    }
}