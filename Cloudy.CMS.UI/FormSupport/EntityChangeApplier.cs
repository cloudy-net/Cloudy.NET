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
        IFieldProvider FieldProvider,
        IEntityPathNavigator EntityPathNavigator
    ) : IEntityChangeApplier
    {
        static JsonSerializerOptions JsonSerializerOptions => new JsonSerializerOptions
        {
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            Converters = { new DateOnlyJsonConverter(), new TimeOnlyJsonConverter(), new JsonStringEnumConverter() }
        };

        public void Apply(object entity, EntityChange change)
        {
            string[] path = change.Path.ToArray();
            EntityPathNavigator.Navigate(ref entity, ref path);

            switch (change)
            {
                case SimpleChange simpleChange:
                    UpdateSimpleField(entity, change.Path, simpleChange.Value);
                    break;
                case BlockTypeChange blockTypeChange:
                    UpdateBlockType(entity, change.Path, blockTypeChange.Type);
                    break;
                case EmbeddedBlockListAdd embeddedBlockListAdd:
                    AddToEmbeddedBlockList(entity, change.Path, embeddedBlockListAdd.Key, embeddedBlockListAdd.Type);
                    break;
                default:
                    throw new Exception($"Unsupported change type: {change.GetType().Name}");
            }
        }

        private void AddToEmbeddedBlockList(object entity, string[] path, JsonElement key, string type)
        {
            throw new NotImplementedException();
        }

        private void UpdateSimpleField(object target, IEnumerable<string> path, string value)
        {
            var entityType = EntityTypeProvider.Get(target.GetType());

            var name = path.First();

            path = path.Skip(1);

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == name);
            var property = entityType.Type.GetProperty(field.Name);

            if (!path.Any())
            {
                property.GetSetMethod().Invoke(target, new object[] { JsonSerializer.Deserialize(value, property.PropertyType, JsonSerializerOptions) });

                return;
            }

            if (property.GetGetMethod().Invoke(target, null) == null) // create instance implicitly
            {
                if (field.Type.IsInterface || field.Type.IsAbstract)
                {
                    throw new NotImplementedException("Updates to nested interfaces or abstract classes not implemented (yet!)");
                }

                var instance = Activator.CreateInstance(field.Type);
                property.GetSetMethod().Invoke(target, new object[] { instance });
            }

            target = property.GetGetMethod().Invoke(target, null);

            UpdateSimpleField(target, path, value);
        }

        private void UpdateBlockType(object target, IEnumerable<string> path, string type)
        {
            var entityType = EntityTypeProvider.Get(target.GetType());

            var name = path.First();

            path = path.Skip(1);

            var field = FieldProvider.Get(entityType.Name).FirstOrDefault(f => f.Name == name);
            var property = entityType.Type.GetProperty(field.Name);

            if (!path.Any())
            {
                if (!field.Type.IsInterface && !field.Type.IsAbstract)
                {
                    throw new Exception($"Changing block type of ({field.Name}) {field.Type} is not supported");
                }

                var blockType = EntityTypeProvider.Get(type);
                var instance = Activator.CreateInstance(blockType.Type);
                property.GetSetMethod().Invoke(target, new object[] { instance });
                return;
            }

            if (property.GetGetMethod().Invoke(target, null) == null) // create instance implicitly
            {
                if (field.Type.IsInterface || field.Type.IsAbstract)
                {
                    throw new NotImplementedException("Updates to nested interfaces or abstract classes not implemented (yet!)");
                }

                var instance = Activator.CreateInstance(field.Type);
                property.GetSetMethod().Invoke(target, new object[] { instance });
            }

            target = property.GetGetMethod().Invoke(target, null);

            UpdateBlockType(target, path, type);
        }
    }
}
