using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ComponentSupport
{
    public interface IComponentAssemblyProvider
    {
        IEnumerable<Assembly> GetAll();
    }
}
