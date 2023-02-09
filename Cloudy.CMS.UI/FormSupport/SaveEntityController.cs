using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FieldSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.UI.FormSupport
{
    [Authorize("adminarea")]
    [Area("Admin")]
    public class SaveEntityController : Controller
    {
        IEntityTypeProvider EntityTypeProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IFieldProvider FieldProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }

        public SaveEntityController(IEntityTypeProvider entityTypeProvider, IPrimaryKeyConverter primaryKeyConverter, IContextCreator contextCreator, IPrimaryKeyGetter primaryKeyGetter, IPropertyDefinitionProvider propertyDefinitionProvider, IFieldProvider fieldProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter)
        {
            EntityTypeProvider = entityTypeProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            FieldProvider = fieldProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
        }

        [HttpPost]
        [Route("/{area}/api/form/entity/save")]
        public async Task<SaveEntityResponse> SaveEntity([FromBody] SaveEntityRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception($"Invalid state:\n{string.Join("\n", ModelState.Where(entry => entry.Value.ValidationState == ModelValidationState.Invalid).SelectMany(entry => entry.Value.Errors.Select(error => $"{entry.Key}: {error.ErrorMessage}")))}");
            }

            var contexts = new HashSet<IContextWrapper>();

            var result = new List<SaveEntityResult>();

            foreach (var changedEntity in data.Entities)
            {
                var entityType = EntityTypeProvider.Get(changedEntity.Reference.EntityType);
                var context = ContextCreator.CreateFor(entityType.Type);

                contexts.Add(context);

                var keyValues = changedEntity.Reference.KeyValues != null ? PrimaryKeyConverter.Convert(changedEntity.Reference.KeyValues.Select(k => k.ToString()), entityType.Type) : null;

                object entity;

                if (keyValues == null)
                {
                    entity = Activator.CreateInstance(entityType.Type);
                }
                else
                {
                    entity = await context.Context.FindAsync(entityType.Type, keyValues).ConfigureAwait(false);

                    if (changedEntity.Remove)
                    {
                        context.Context.Remove(entity);
                        result.Add(SaveEntityResult.SuccessResult(entityType.Name, keyValues, null));
                        continue;
                    }
                }

                var propertyDefinitions = PropertyDefinitionProvider.GetFor(entityType.Name).ToDictionary(p => p.Name, p => p);
                var idProperties = PrimaryKeyPropertyGetter.GetFor(entity.GetType());

                if (changedEntity.Changes.OfType<SimpleChange>().Any(c => c.Path.Length == 1 && idProperties.Any(p => p.Name == c.Path[0])))
                {
                    throw new Exception($"Tried to change primary key of entity {string.Join(", ", keyValues)} with type {changedEntity.Reference.EntityType}!");
                }

                foreach(var change in changedEntity.Changes)
                {
                    if(change is SimpleChange simpleChange)
                    {
                        UpdateSimpleField(entity, change.Path, simpleChange.Value);
                    }
                    if(change is BlockTypeChange blockTypeChange)
                    {
                        UpdateBlockType(entity, change.Path, blockTypeChange.Type);
                    }
                }

                if (!TryValidateModel(entity))
                {
                    result.Add(SaveEntityResult.ValidationFailureResult(entityType.Name, keyValues, ModelState.ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage))));
                    continue;
                }

                if (keyValues == null)
                {
                    await context.Context.AddAsync(entity).ConfigureAwait(false);
                }
                else
                {
                    //await EntityUpdater.UpdateAsync(Entity).ConfigureAwait(false);
                }

                result.Add(SaveEntityResult.SuccessResult(entityType.Name, PrimaryKeyGetter.Get(entity), changedEntity.Reference.NewContentKey));
            }

            foreach(var context in contexts)
            {
                await context.Context.SaveChangesAsync().ConfigureAwait(false);
            }

            return new SaveEntityResponse(result);
        }

        public class SaveEntityResponse
        {
            public IEnumerable<SaveEntityResult> Results { get; }

            public SaveEntityResponse(IEnumerable<SaveEntityResult> result)
            {
                Results = result.ToList().AsReadOnly();
            }
        }

        public class SaveEntityResult
        {
            public bool Success { get; private set; }
            public EntityReference EntityReference { get; private set; }
            public IDictionary<string, IEnumerable<string>> ValidationErrors { get; private set; }

            public static SaveEntityResult SuccessResult(string entityType, IEnumerable<object> keyValues, string newContentKey)
            {
                return new SaveEntityResult
                {
                    Success = true,
                    EntityReference = new EntityReference
                    {
                        NewContentKey = newContentKey,
                        EntityType = entityType,
                        KeyValues = keyValues?.Select(k => JsonSerializer.SerializeToElement(k)).ToArray(),
                    },
                };
            }

            public static SaveEntityResult ValidationFailureResult(string entityTypeId, IEnumerable<object> keyValues, IDictionary<string, IEnumerable<string>> validationErrors)
            {
                return new SaveEntityResult
                {
                    Success = false,
                    EntityReference = new EntityReference
                    {
                        EntityType = entityTypeId,
                        KeyValues = keyValues?.Select(k => JsonSerializer.SerializeToElement(k)).ToArray(),
                    },
                    ValidationErrors = validationErrors.ToDictionary(e => e.Key, e => (IEnumerable<string>)e.Value.ToList().AsReadOnly()),
                };
            }
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
                property.GetSetMethod().Invoke(target, new object[] { JsonSerializer.Deserialize(value, property.PropertyType) });
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

        public class SaveEntityRequestBody
        {
            [Required]
            public IEnumerable<ChangedEntity> Entities { get; set; }
        }

        public class ChangedEntity
        {
            [Required]
            public EntityReference Reference { get; set; }
            public bool Remove { get; set; }
            [Required]
            public IEnumerable<EntityChange> Changes { get; set; }
        }

        public class EntityReference
        {
            public string NewContentKey { get; set; }
            public JsonElement[] KeyValues { get; set; }
            [Required]
            public string EntityType { get; set; }
        }

        [JsonDerivedType(typeof(SimpleChange), typeDiscriminator: "simple")]
        [JsonDerivedType(typeof(BlockTypeChange), typeDiscriminator: "blocktype")]
        public abstract class EntityChange
        {
            public DateTime Date { get; set; }
            [Required]
            public string[] Path { get; set; }
        }

        public class SimpleChange : EntityChange
        {
            public string Value { get; set; }
        }

        public class BlockTypeChange : EntityChange
        {
            public string Type { get; set; }
        }

        public class PolymorphicValue
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
