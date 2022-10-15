using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Naming;
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
        IFieldTypeMapper FieldTypeMapper { get; set; }

        public FieldCreator(IPropertyDefinitionProvider propertyDefinitionProvider, IHumanizer humanizer, IFieldTypeMapper fieldTypeMapper)
        {
            PropertyDefinitionProvider = propertyDefinitionProvider;
            Humanizer = humanizer;
            FieldTypeMapper = fieldTypeMapper;
        }

        public IEnumerable<FieldDescriptor> Create(string contentType)
        {
            var result = new List<FieldDescriptor>();

            foreach (var propertyDefinition in PropertyDefinitionProvider.GetFor(contentType))
            {
                var displayAttribute = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

                var autoGenerate = displayAttribute?.GetAutoGenerateField() ?? true;
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

                var partialName = uiHints.FirstOrDefault() ?? FieldTypeMapper.MapToPartial(propertyDefinition);

                var partial = $"Form/{partialName}";

                var isSortable = false;

                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    type = type.GetGenericArguments().Single();
                    isSortable = true;
                }

                var renderChrome = true;

                if (uiHints.Contains("nochrome") || propertyDefinition.Attributes.Any(c => c.GetType().GetCustomAttributes<UIHintAttribute>().Any(a => a.UIHint == "nochrome")))
                {
                    renderChrome = false;
                }

                result.Add(new FieldDescriptor(name, type, propertyDefinition.Attributes, label, partial, isSortable, autoGenerate, renderChrome, group));
            }
            
            return result;
        }
    }
}
