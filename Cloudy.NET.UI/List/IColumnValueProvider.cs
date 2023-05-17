using Cloudy.NET.PropertyDefinitionSupport;
using System.Threading.Tasks;

namespace Cloudy.NET.UI.List
{
    public interface IColumnValueProvider
    {
        Task<object> Get(PropertyDefinitionDescriptor propertyDefinition, object instance);
    }
}