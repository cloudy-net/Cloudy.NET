using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContextDescriptorProvider
    {
        ContextDescriptor GetFor(Type type);
    }
}