using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.PropertyDefinitionSupport
{
    public interface IPropertyDefinitionProvider
    {
        IEnumerable<PropertyDefinitionDescriptor> GetFor(string name);
    }
}
