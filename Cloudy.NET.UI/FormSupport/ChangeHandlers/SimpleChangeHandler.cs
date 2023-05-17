using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.UI.FieldSupport;
using Cloudy.NET.UI.FormSupport.Changes;
using Cloudy.NET.UI.Serialization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cloudy.NET.UI.FormSupport.ChangeHandlers
{
    public record SimpleChangeHandler(IEntityTypeProvider EntityTypeProvider, IFieldProvider FieldProvider, IEntityNavigator EntityNavigator) : ISimpleChangeHandler
    {
        static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters = { new DateOnlyJsonConverter(), new TimeOnlyJsonConverter(), new JsonStringEnumConverter() }
        };

        public void SetValue(object entity, SimpleChange change)
        {
            var entityType = EntityTypeProvider.Get(entity.GetType());

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == change.PropertyName);
            var property = entityType.Type.GetProperty(field.Name);

            property.GetSetMethod().Invoke(entity, new object[] { JsonSerializer.Deserialize(change.Value, property.PropertyType, JsonSerializerOptions) });
        }
    }
}
