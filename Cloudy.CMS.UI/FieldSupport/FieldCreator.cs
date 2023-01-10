using Cloudy.CMS.Naming;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FieldTypes.MediaPicker;
using Cloudy.CMS.UI.FormSupport.FieldTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public class FieldCreator : IFieldCreator
    {
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IHumanizer Humanizer { get; }

        public FieldCreator(IPropertyDefinitionProvider propertyDefinitionProvider, IHumanizer humanizer)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            Humanizer = humanizer;
        }

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

                if(propertyDefinition.Attributes.OfType<SelectAttribute>().Any() && humanizedName.EndsWith(" id"))
                {
                    humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                }

                var label = displayAttribute?.GetName() ?? humanizedName;

                var type = propertyDefinition.Type;
                var uiHints = propertyDefinition.Attributes.OfType<UIHintAttribute>().Select(a => a.UIHint).ToList().AsReadOnly();

                string partialName = null;

                if (propertyDefinition.Type == typeof(string))
                {
                    partialName = "text";
                }

                if (propertyDefinition.Type == typeof(bool))
                {
                    partialName = "checkbox";
                }

                if (propertyDefinition.Type == typeof(int))
                {
                    partialName = "number";
                }

                if (propertyDefinition.Type == typeof(double))
                {
                    partialName = "decimal";
                }

                if (propertyDefinition.Type == typeof(DateTime) || propertyDefinition.Type == typeof(DateTimeOffset))
                {
                    partialName = "datetime";
                }

                if (propertyDefinition.Type == typeof(TimeSpan) || propertyDefinition.Type == typeof(TimeOnly))
                {
                    partialName = "time";
                }

                if (propertyDefinition.Type == typeof(DateOnly))
                {
                    partialName = "date";
                }

                if (propertyDefinition.Attributes.Any(a => a is SelectAttribute))
                {
                    partialName = "selectone";
                }

                if (propertyDefinition.Attributes.Any(a => a is MediaPickerAttribute))
                {
                    partialName = "mediapicker";
                }

                if (propertyDefinition.Enum)
                {
                    partialName = "enumdropdown";
                }

                if (uiHints.Any())
                {
                    partialName = uiHints.First();
                }

                var settings = new Dictionary<string, object>();

                if (propertyDefinition.Block)
                {
                    partialName = "embeddedblock";
                    settings["types"] = propertyDefinition.BlockTypes.Select(t => t.Name);
                }

                if(partialName == null)
                {
                    partialName = "failed";
                }

                var partial = partialName != null ? (partialName.StartsWith('/') ? partialName : $"../../form/controls/{partialName}.js") : null;

                var renderChrome = true;

                if (uiHints.Contains("nochrome"))
                {
                    renderChrome = false;
                }

                result.Add(new FieldDescriptor(name, type, label, partial, autoGenerate, renderChrome, group, settings));
            }
            
            return result;
        }
    }
}
