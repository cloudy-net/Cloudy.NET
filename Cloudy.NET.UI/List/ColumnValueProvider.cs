using Cloudy.NET.ContextSupport;
using Cloudy.NET.EntitySupport;
using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.Naming;
using Cloudy.NET.PropertyDefinitionSupport;
using Cloudy.NET.UI.FieldSupport.CustomSelect;
using Cloudy.NET.UI.FieldSupport.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{

    public record ColumnValueProvider(IContextCreator ContextCreator, IEntityTypeProvider EntityTypeProvider, INameGetter NameGetter, IServiceProvider ServiceProvider) : IColumnValueProvider
    {
        public async Task<object> Get(PropertyDefinitionDescriptor propertyDefinition, object instance)
        {
            var value = propertyDefinition.Getter(instance);
            if (value is null) return null;

            var customSelectAttribute = propertyDefinition.Attributes.OfType<ICustomSelectAttribute>().FirstOrDefault();
            if (customSelectAttribute is not null)
            {
                return GetCustomSelectAttributeValue(propertyDefinition, customSelectAttribute, value);
            }

            var selectAttribute = propertyDefinition.Attributes.OfType<ISelectAttribute>().FirstOrDefault();
            if (selectAttribute is not null)
            {
                return await GetSelectAttributeValue(value, selectAttribute).ConfigureAwait(false);
            }

            return value;
        }

        private async Task<object> GetSelectAttributeValue(object value, ISelectAttribute selectAttribute)
        {
            if (value.Equals(Activator.CreateInstance(value.GetType()))) return null;

            var type = EntityTypeProvider.Get(selectAttribute.Type);
            var context = ContextCreator.CreateFor(type.Type);
            var relatedInstance = await context.Context.FindAsync(type.Type, value).ConfigureAwait(false);
            var name = NameGetter.GetName(relatedInstance);
            return new { name = name ?? "Does not exist", image = (relatedInstance as IImageable)?.Image };
        }

        private async Task<object> GetCustomSelectAttributeValue(
            PropertyDefinitionDescriptor propertyDefinition,
            ICustomSelectAttribute customSelectAttribute,
            object value)
        {
            var factoryType = customSelectAttribute.GetType().GenericTypeArguments.FirstOrDefault();
            var factory = ServiceProvider.GetService(factoryType) as ICustomSelectFactory;
            var items = await factory.GetItems();

            if (propertyDefinition.List)
            {
                var selectedValues = value as IList<string>;

                return selectedValues is null ? string.Empty : string.Join(", ", items.Where(i => selectedValues?.Contains(i.Value) ?? false).Select(i => i.Text).Order());
            }

            return items.FirstOrDefault(x => x.Value == value.ToString())?.Text;
        }
    }
}
