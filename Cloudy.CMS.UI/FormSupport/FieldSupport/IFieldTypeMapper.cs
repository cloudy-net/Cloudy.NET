using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Reflection.Metadata;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public interface IFieldTypeMapper
    {
        string MapToPartial(PropertyDefinitionDescriptor propertyDefinitionDescriptor);
    }
}