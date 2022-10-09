using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public record FieldViewModel(
        ContentTypeDescriptor ContentType,
        object Instance,
        FieldDescriptor Field,
        PropertyDefinitionDescriptor Property,
        object Value
    );
}
