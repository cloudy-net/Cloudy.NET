using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport;
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.Naming;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FieldSupport.CustomSelect;
using Cloudy.CMS.UI.FieldSupport.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport
{
    public interface IFieldFriendlyValueProvider
    {
        Task<object> GetFriendlyValue(PropertyDefinitionDescriptor propertyDefinition, object instance);
    }

    public class FieldFriendlyValueProvider : IFieldFriendlyValueProvider
    {
        private readonly IContextCreator contextCreator;
        private readonly IEntityTypeProvider entityTypeProvider;
        private readonly INameGetter nameGetter;
        private readonly IServiceProvider serviceProvider;

        public FieldFriendlyValueProvider(
            IContextCreator contextCreator,
            IEntityTypeProvider entityTypeProvider,
            INameGetter nameGetter,
            IServiceProvider serviceProvider)
        {
            this.contextCreator = contextCreator;
            this.entityTypeProvider = entityTypeProvider;
            this.nameGetter = nameGetter;
            this.serviceProvider = serviceProvider;
        }

        public async Task<object> GetFriendlyValue(PropertyDefinitionDescriptor propertyDefinition, object instance)
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

            var type = entityTypeProvider.Get(selectAttribute.Type);
            var context = contextCreator.CreateFor(type.Type);
            var relatedInstance = await context.Context.FindAsync(type.Type, value).ConfigureAwait(false);
            var name = nameGetter.GetName(relatedInstance);
            return new { name = name ?? "Does not exist", image = (relatedInstance as IImageable)?.Image };
        }

        private async Task<object> GetCustomSelectAttributeValue(
            PropertyDefinitionDescriptor propertyDefinition,
            ICustomSelectAttribute customSelectAttribute,
            object value)
        {
            var factoryType = customSelectAttribute.GetType().GenericTypeArguments.FirstOrDefault();
            var factory = serviceProvider.GetService(factoryType) as ICustomSelectFactory;
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
