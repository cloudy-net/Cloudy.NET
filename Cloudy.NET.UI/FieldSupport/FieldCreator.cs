﻿using Cloudy.NET.EntitySupport;
using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.Naming;
using Cloudy.NET.PropertyDefinitionSupport;
using Cloudy.NET.UI.FieldSupport.CustomSelect;
using Cloudy.NET.UI.FieldSupport.MediaPicker;
using Cloudy.NET.UI.FieldSupport.Select;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Cloudy.NET.UI.FieldSupport
{
    public record FieldCreator(IPropertyDefinitionProvider PropertyDefinitionProvider, IHumanizer Humanizer, IEntityTypeProvider EntityTypeProvider) : IFieldCreator
    {
        private static IEnumerable<KeyValuePair<string, object>> GetValidators(IEnumerable<Attribute> attributes)
        {
            var requiredAttribute = attributes.OfType<RequiredAttribute>().FirstOrDefault();
            if (requiredAttribute is not null)
            {
                yield return new KeyValuePair<string, object>(
                    "required",
                    new { message = requiredAttribute.ErrorMessage ?? "This field is required" }
                );
            }

            var maxLengthAttribute = attributes.OfType<MaxLengthAttribute>().FirstOrDefault();
            if (maxLengthAttribute is not null)
            {
                yield return new KeyValuePair<string, object>(
                    "maxLength",
                    new { message = maxLengthAttribute.ErrorMessage, maxLength = maxLengthAttribute.Length }
                );
            }
        }

        public IEnumerable<FieldDescriptor> Create(string entityType)
        {
            var result = new List<FieldDescriptor>();

            foreach (var propertyDefinition in PropertyDefinitionProvider.GetFor(entityType))
            {
                var displayAttribute = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();
                var settings = new Dictionary<string, object>()
                {
                    { "isRequired", propertyDefinition.Attributes.OfType<RequiredAttribute>().Any() }
                };

                var autoGenerate = displayAttribute?.GetAutoGenerateField();
                var group = displayAttribute?.GetGroupName();
                var name = propertyDefinition.Name;
                var humanizedName = Humanizer.Humanize(name);

                var selectAttribute = propertyDefinition.Attributes.OfType<ISelectAttribute>().FirstOrDefault();
                if (selectAttribute is not null)
                {
                    var referencedType = selectAttribute.Type;

                    settings["simpleKey"] = !propertyDefinition.Type.IsAssignableTo(typeof(ITuple));
                    settings["referencedTypeName"] = referencedType.Name;
                    settings["imageable"] = typeof(IImageable).IsAssignableFrom(referencedType);

                    if (humanizedName.EndsWith(" id"))
                    {
                        humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                    }
                }

                var label = displayAttribute?.GetName() ?? humanizedName;
                var description = displayAttribute?.GetDescription();
                var type = propertyDefinition.Type;

                var uiHints = propertyDefinition.Attributes.OfType<UIHintAttribute>().Select(a => a.UIHint).ToList().AsReadOnly();
                var partialName = GetPartialName(propertyDefinition, uiHints);
                var validators = GetValidators(propertyDefinition.Attributes).ToDictionary(x => x.Key, x => x.Value);

                var renderChrome = true;

                if (propertyDefinition.Block)
                {
                    partialName = "embedded-block/embedded-block";
                    settings["types"] = EntityTypeProvider.GetAll().Select(t => t.Type).Where(t => t.IsAssignableTo(propertyDefinition.Type)).Select(t => t.Name).ToList().AsReadOnly();
                    renderChrome = false;
                }

                if (partialName == null)
                {
                    partialName = "failed";
                }

                var partial = partialName != null ? (partialName.StartsWith('/') ? partialName : $"controls/{partialName}.js") : null;

                string listPartial = null;

                if (propertyDefinition.List)
                {
                    listPartial = "controls/generic-list/generic-list.js";

                    if (propertyDefinition.Block)
                    {
                        listPartial = "controls/embedded-block-list/embedded-block-list.js";
                        renderChrome = true;
                    }
                }

                if (uiHints.Contains("nochrome"))
                {
                    renderChrome = false;
                }

                result.Add(new FieldDescriptor(name, type, label, description, partial, listPartial, autoGenerate, renderChrome, group, settings, validators));
            }

            return result;
        }

        private static string GetPartialName(PropertyDefinitionDescriptor propertyDefinition, ReadOnlyCollection<string> uiHints)
        {
            if (uiHints.Any()) return uiHints.First();

            var customSelectAttribute = propertyDefinition.Attributes.OfType<ICustomSelectAttribute>().FirstOrDefault();
            if (customSelectAttribute is not null) return propertyDefinition.List
                ? "custom-select/custom-select-list"
                : "custom-select/custom-select";

            if (propertyDefinition.Attributes.OfType<ISelectAttribute>().Any()) return "select-one/select-one";
            if (propertyDefinition.Attributes.Any(a => a is MediaPickerAttribute)) return "media-picker/media-picker";
            if (propertyDefinition.Type == typeof(string)) return "text";
            if (propertyDefinition.Type == typeof(bool)) return "checkbox";
            if (propertyDefinition.Type == typeof(int)) return "number";
            if (propertyDefinition.Type == typeof(double)) return "decimal";
            if (propertyDefinition.Type == typeof(DateTime) || propertyDefinition.Type == typeof(DateTimeOffset)) return "datetime";
            if (propertyDefinition.Type == typeof(TimeSpan) || propertyDefinition.Type == typeof(TimeOnly)) return "time";
            if (propertyDefinition.Type == typeof(DateOnly)) return "date";
            if (propertyDefinition.Enum) return "enum-dropdown";

            return null;
        }
    }
}
