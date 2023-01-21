using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.Naming;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FieldSupport.CustomSelect;
using Cloudy.CMS.UI.FieldSupport.MediaPicker;
using Cloudy.CMS.UI.FieldSupport.Select;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Cloudy.CMS.UI.FieldSupport
{
    public record FieldCreator(IPropertyDefinitionProvider PropertyDefinitionProvider, IHumanizer Humanizer, IEntityTypeProvider EntityTypeProvider) : IFieldCreator
    {
        public IEnumerable<FieldDescriptor> Create(string entityType)
        {
            var result = new List<FieldDescriptor>();

            foreach (var propertyDefinition in PropertyDefinitionProvider.GetFor(entityType))
            {
                var displayAttribute = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

                var autoGenerate = displayAttribute?.GetAutoGenerateField();
                var group = displayAttribute?.GetGroupName();

                var name = propertyDefinition.Name;
                var humanizedName = Humanizer.Humanize(name);

                if(propertyDefinition.Attributes.OfType<ISelectAttribute>().Any() && humanizedName.EndsWith(" id"))
                {
                    humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                }

                var label = displayAttribute?.GetName() ?? humanizedName;

                var type = propertyDefinition.Type;
                var uiHints = propertyDefinition.Attributes.OfType<UIHintAttribute>().Select(a => a.UIHint).ToList().AsReadOnly();

                var partialName = GetPartialName(propertyDefinition, uiHints);
                var settings = new Dictionary<string, object>();

                if (propertyDefinition.Block)
                {
                    partialName = "embedded-block/embedded-block";
                    settings["types"] = EntityTypeProvider.GetAll().Select(t => t.Type).Where(t => t.IsAssignableTo(propertyDefinition.Type)).Select(t => t.Name).ToList().AsReadOnly();
                }

                var customSelectAttribute = propertyDefinition.Attributes.OfType<ICustomSelectAttribute>().FirstOrDefault();
                if (customSelectAttribute is not null) 
                {
                    var factoryType = customSelectAttribute.GetType().GetGenericArguments().FirstOrDefault();

                    settings["factoryAssemblyQualifiedName"] = factoryType?.AssemblyQualifiedName;
                }

                if (partialName == null)
                {
                    partialName = "failed";
                }

                var partial = partialName != null ? (partialName.StartsWith('/') ? partialName : $"../../form/controls/{partialName}.js") : null;

                var renderChrome = true;

                if (uiHints.Contains("nochrome") || propertyDefinition.Block)
                {
                    renderChrome = false;
                }

                result.Add(new FieldDescriptor(name, type, label, partial, autoGenerate, renderChrome, group, settings));
            }
            
            return result;
        }

        private static string GetPartialName(PropertyDefinitionDescriptor propertyDefinition, ReadOnlyCollection<string> uiHints)
        {
            var customSelectAttribute = propertyDefinition.Attributes.OfType<ICustomSelectAttribute>().FirstOrDefault();
            if (customSelectAttribute is not null) return customSelectAttribute.Multi ? "custom-selectmulti" : "custom-selectone";

            if (propertyDefinition.Attributes.OfType<ISelectAttribute>().Any()) return "selectone";
            
            if (propertyDefinition.Type == typeof(string)) return "text";
            if (propertyDefinition.Type == typeof(bool)) return "checkbox";
            if (propertyDefinition.Type == typeof(int)) return "number";
            if (propertyDefinition.Type == typeof(double)) return "decimal";
            if (propertyDefinition.Type == typeof(DateTime) || propertyDefinition.Type == typeof(DateTimeOffset)) return "datetime";
            if (propertyDefinition.Type == typeof(TimeSpan) || propertyDefinition.Type == typeof(TimeOnly)) return "time";
            if (propertyDefinition.Type == typeof(DateOnly)) return "date";

            if (propertyDefinition.Attributes.Any(a => a is MediaPickerAttribute)) return "media-picker/media-picker";

            if (propertyDefinition.Enum) return "enumdropdown";

            if (uiHints.Any()) return uiHints.First();

            return null;
        }
    }
}
