using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.PropertyDefinitionSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public record ListColumnViewModel(
        EntityTypeDescriptor EntityType,
        object Instance,
        PropertyDefinitionDescriptor PropertyDefinition,
        object Value
    );
}
