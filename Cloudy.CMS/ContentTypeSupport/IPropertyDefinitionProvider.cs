using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface IPropertyDefinitionProvider
    {
        IEnumerable<PropertyDefinitionDescriptor> GetFor(string contentTypeId);
    }
}
