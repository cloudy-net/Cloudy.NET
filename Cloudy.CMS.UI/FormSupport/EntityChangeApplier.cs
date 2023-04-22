using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FieldSupport;
using Cloudy.CMS.UI.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FormSupport
{
    public record EntityChangeApplier(
        IEntityTypeProvider EntityTypeProvider,
        IFieldProvider FieldProvider
    ) : IEntityChangeApplier
    {
        static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters = { new DateOnlyJsonConverter(), new TimeOnlyJsonConverter(), new JsonStringEnumConverter() }
        };

        public void Apply(object entity, EntityChange change)
        {
            var propertyName = change.Path.Last();
            switch (change)
            {
                case SimpleChange simpleChange:
                    UpdateSimpleField(entity, propertyName, simpleChange.Value);
                    break;
                case BlockTypeChange blockTypeChange:
                    UpdateBlockType(entity, propertyName, blockTypeChange.Type);
                    break;
                case EmbeddedBlockListAdd embeddedBlockListAdd:
                    AddToEmbeddedBlockList(entity, propertyName, embeddedBlockListAdd.Key, embeddedBlockListAdd.Type);
                    break;
                default:
                    throw new Exception($"Unsupported change type: {change.GetType().Name}");
            }
        }

        private void AddToEmbeddedBlockList(object entity, string name, JsonElement key, string type)
        {
            throw new NotImplementedException();
        }

        private void UpdateSimpleField(object target, string name, string value)
        {
            var entityType = EntityTypeProvider.Get(target.GetType());

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == name);
            var property = entityType.Type.GetProperty(field.Name);

            property.GetSetMethod().Invoke(target, new object[] { JsonSerializer.Deserialize(value, property.PropertyType, JsonSerializerOptions) });
        }

        private void UpdateBlockType(object target, string name, string type)
        {
            var entityType = EntityTypeProvider.Get(target.GetType());

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == name);
            var property = entityType.Type.GetProperty(field.Name);

            if (!field.Type.IsInterface && !field.Type.IsAbstract)
            {
                throw new Exception($"Changing block type of ({field.Name}) {field.Type} is not supported");
            }

            var blockType = EntityTypeProvider.Get(type);
            var instance = Activator.CreateInstance(blockType.Type);
            property.GetSetMethod().Invoke(target, new object[] { instance });
        }
    }
}
