using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ComponentSupport
{
    public interface IComponentProvider
    {
        IEnumerable<ComponentDescriptor> GetAll();
    }
}
