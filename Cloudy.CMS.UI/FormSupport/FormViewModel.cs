using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport
{
    public record FormViewModel(
        IEnumerable<FieldDescriptor> Fields,
        IEnumerable<PropertyDefinitionDescriptor> PropertyDefinitions,
        IEnumerable<string> PrimaryKeyNames,
        ContentTypeDescriptor ContentType,
        object Instance
    );
}
