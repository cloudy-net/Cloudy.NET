using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ComponentSupport
{
    public interface IComponentCreator
    {
        IEnumerable<ComponentDescriptor> Create();
    }
}