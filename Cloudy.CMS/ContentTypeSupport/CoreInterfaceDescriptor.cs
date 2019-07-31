using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cloudy.CMS.ContentTypeSupport
{
    [DebuggerDisplay("{Id}")]
    public class CoreInterfaceDescriptor
    {
        public string Id { get; }
        public Type Type { get; }
        public IEnumerable<PropertyDefinitionDescriptor> PropertyDefinitions { get; }

        public CoreInterfaceDescriptor(string id, Type type, IEnumerable<PropertyDefinitionDescriptor> propertyDefinitions)
        {
            Id = id;
            Type = type;
            PropertyDefinitions = propertyDefinitions;
        }
    }
}