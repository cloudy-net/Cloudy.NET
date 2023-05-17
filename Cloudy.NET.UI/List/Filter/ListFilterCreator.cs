using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.Naming;
using Cloudy.NET.UI.FieldSupport.Select;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using Cloudy.NET.PropertyDefinitionSupport;

namespace Cloudy.NET.UI.List.Filter
{
    public record ListFilterCreator(IEntityTypeProvider EntityTypeProvider, IPropertyDefinitionProvider PropertyDefinitionProvider, IHumanizer Humanizer) : IListFilterCreator
    {
        public IDictionary<Type, IEnumerable<ListFilterDescriptor>> Create()
        {
            var result = new Dictionary<Type, IEnumerable<ListFilterDescriptor>>();

            foreach (var entityType in EntityTypeProvider.GetAll())
            {
                var columns = new List<ListFilterDescriptor>();

                var properties = PropertyDefinitionProvider.GetFor(entityType.Name).Where(p => p.Attributes.OfType<ListFilterAttribute>().Any());

                var order = 10000;
                foreach (var propertyDefinition in properties)
                {
                    var name = propertyDefinition.Name;
                    var humanizedName = Humanizer.Humanize(name);

                    if (propertyDefinition.Attributes.OfType<ISelectAttribute>().Any() && humanizedName.EndsWith(" id"))
                    {
                        humanizedName = humanizedName.Substring(0, humanizedName.Length - " id".Length);
                    }

                    var selectAttribute = propertyDefinition.Attributes.OfType<ISelectAttribute>().FirstOrDefault();
                    var select = selectAttribute != null;
                    var selectType = select ? EntityTypeProvider.Get(selectAttribute.Type)?.Name : null;

                    var attribute = propertyDefinition.Attributes.OfType<ListFilterAttribute>().First();

                    var simpleKey = !propertyDefinition.Type.IsAssignableTo(typeof(ITuple));

                    columns.Add(new ListFilterDescriptor(name, humanizedName, entityType.Name, select, selectType, simpleKey, attribute.Order == -10000 ? order++ : attribute.Order));
                }

                result[entityType.Type] = columns.OrderBy(c => c.Order);
            }

            return result;
        }
    }
}
