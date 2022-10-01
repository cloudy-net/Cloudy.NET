using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.Naming;
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
                var label = displayAttribute?.GetName() ?? humanizedName;

                var type = propertyDefinition.Type;
                var isSortable = false;

                if (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>) || type.GetGenericTypeDefinition() == typeof(List<>) || type.GetGenericTypeDefinition() == typeof(IList<>)))
                {
                    type = type.GetGenericArguments().Single();
                    isSortable = true;
                }

                var uiHints = propertyDefinition.Attributes.OfType<UIHintAttribute>().Select(a => a.UIHint).ToList().AsReadOnly();

                result.Add(new FieldDescriptor(name, type, uiHints, label, isSortable, autoGenerate, group));
            }
            
            return result;
        }
    }
}
