using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
{
    public interface IContextDescriptorProvider
    {
        IEnumerable<ContextDescriptor> GetAll();
        ContextDescriptor GetFor(Type type);
    }
}