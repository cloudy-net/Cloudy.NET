using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public record ListColumnViewModel(
        ContentTypeDescriptor ContentType,
        object Instance,
        PropertyDefinitionDescriptor PropertyDefinition,
        object Value
    );
}
