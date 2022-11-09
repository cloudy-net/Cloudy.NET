using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.PropertyDefinitionSupport
{
    public interface IPropertyDefinitionProvider
    {
        IEnumerable<PropertyDefinitionDescriptor> GetFor(string name);
    }
}
