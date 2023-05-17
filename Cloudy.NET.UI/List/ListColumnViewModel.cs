using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.PropertyDefinitionSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{
    public record ListColumnViewModel(
        EntityTypeDescriptor EntityType,
        object Instance,
        PropertyDefinitionDescriptor PropertyDefinition,
        object Value
    );
}
