using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
{
    public interface IContextDescriptorCreator
    {
        IEnumerable<ContextDescriptor> Create(IEnumerable<Type> types);
    }
}