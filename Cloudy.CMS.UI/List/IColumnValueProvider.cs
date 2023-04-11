using Cloudy.CMS.PropertyDefinitionSupport;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public interface IColumnValueProvider
    {
        Task<object> Get(PropertyDefinitionDescriptor propertyDefinition, object instance);
    }
}