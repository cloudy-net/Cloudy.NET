using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.ContextSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
using Cloudy.CMS.PropertyDefinitionSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

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
        [Route("/{area}/api/form/content/save")]
        public async Task<SaveContentResponse> SaveContent([FromBody] SaveContentRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid state");
            }

            var contexts = new HashSet<IContextWrapper>();

            var result = new List<SaveContentResult>();

            foreach (var changedContent in data.ChangedContent)
            {
                var entityType = EntityTypeProvider.Get(changedContent.ContentReference.EntityType);
                var context = ContextCreator.CreateFor(entityType.Type);

                contexts.Add(context);

                var keyValues = PrimaryKeyConverter.Convert(changedContent.ContentReference.KeyValues.Select(k => k.ToString()), entityType.Type);

                object entity;

                if (keyValues == null)
                {
                    entity = Activator.CreateInstance(entityType.Type);
                }
                else
                {
                    entity = await context.Context.FindAsync(entityType.Type, keyValues).ConfigureAwait(false);

                    if (changedContent.Remove)
                    {
                        context.Context.Remove(entity);
                        result.Add(SaveContentResult.SuccessResult(entityType.Name, keyValues));
                        continue;
                    }
                }

                var propertyDefinitions = PropertyDefinitionProvider.GetFor(entityType.Name).ToDictionary(p => p.Name, p => p);
                var idProperties = PrimaryKeyPropertyGetter.GetFor(entity.GetType());

                if (changedContent.SimpleChanges.Any(c => c.Path.Length == 1 && idProperties.Any(p => p.Name == c.Path[0])))
                {
                    throw new Exception($"Tried to change primary key of content {string.Join(", ", keyValues)} with type {changedContent.ContentReference.EntityType}!");
                }

                var changedSimpleFields = changedContent.SimpleChanges.ToList();

                foreach (var change in changedSimpleFields)
                {
                    UpdateSimpleField(entity, change.Path, change.Value);
                }

                //var arrayChanges = changedContent.SimpleChanges.Where(f => f.Type == ChangeType.Array).ToList();

                //foreach (var change in arrayChanges)
                //{
                //    var name = change.Path.Single();
                //    var field = FieldProvider.GetFor(entityType.Id, name);
                //    var property = entityType.Type.GetProperty(field.Name);
                //    var array = (IList)property.GetGetMethod().Invoke(content, null);

                //    if(array == null)
                //    {
                //        array = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(field.Type));
                //        property.GetSetMethod().Invoke(content, new object[] { array });
                //    }

                //    if (field.Type.IsInterface)
                //    {
                //        foreach (var arrayChange in change.Changes) {
                //            var polymorphicValue = JsonSerializer.Deserialize<PolymorphicValue>(arrayChange.Value);
                //            var form = EntityTypeProvider.Get(polymorphicValue.Type);
                //            var value = JsonSerializer.Deserialize(polymorphicValue.Value, form.Type);
                //            array.Add(value);
                //        }
                //    }
                //}

                if (!TryValidateModel(entity))
                {
                    result.Add(SaveContentResult.ValidationFailureResult(entityType.Name, keyValues, ModelState.ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage))));
                    continue;
                }

                if (keyValues == null)
                {
                    await context.Context.AddAsync(entity).ConfigureAwait(false);
                }
                else
                {
                    //await ContentUpdater.UpdateAsync(content).ConfigureAwait(false);
                }

                result.Add(SaveContentResult.SuccessResult(entityType.Name, PrimaryKeyGetter.Get(entity)));
            }

            foreach(var context in contexts)
            {
                await context.Context.SaveChangesAsync().ConfigureAwait(false);
            }

            return new SaveContentResponse(result);
        }

        public class SaveContentResponse
        {
            public IEnumerable<SaveContentResult> Results { get; }

            public SaveContentResponse(IEnumerable<SaveContentResult> result)
            {
                Results = result.ToList().AsReadOnly();
            }
        }

        public class SaveContentResult
        {
            public bool Success { get; private set; }
            public ContentReference ContentReference { get; private set; }
            public IDictionary<string, IEnumerable<string>> ValidationErrors { get; private set; }

            public static SaveContentResult SuccessResult(string entityTypeId, IEnumerable<object> keyValues)
            {
                return new SaveContentResult
                {
                    Success = true,
                    ContentReference = new ContentReference
                    {
                        EntityType = entityTypeId,
                        KeyValues = keyValues?.Select(k => JsonSerializer.SerializeToElement(k)).ToArray(),
                    },
                };
            }

            public static SaveContentResult ValidationFailureResult(string entityTypeId, IEnumerable<object> keyValues, IDictionary<string, IEnumerable<string>> validationErrors)
            {
                return new SaveContentResult
                {
                    Success = false,
                    ContentReference = new ContentReference
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

            //if (field.IsSortable)
            //{
            //    //var indexString = path.ElementAt(1);

            //    //if (!indexString.StartsWith("original-"))
            //    //{
            //    //    throw new Exception("Simple updates to non-persisted array elements not supported");
            //    //}

            //    //var index = int.Parse(indexString.Substring("original-".Length));

            //    //var elementAtMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name == nameof(Enumerable.ElementAt) && m.GetParameters()[1].ParameterType == typeof(int));
            //    //var genericElementAtMethod = elementAtMethod.Single().MakeGenericMethod(field.Type);
            //    //var element = genericElementAtMethod.Invoke(null, new object[] { instanceValue, index });

            //    //UpdateSimpleField(element, path.Skip(2), value);
            //    throw new NotImplementedException("Simple updates to sortables not implemented (yet!)");
            //}
            //else
            {
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
        }

        public class SaveContentRequestBody
        {
            [Required]
            public IEnumerable<ChangedContent> ChangedContent { get; set; }
        }

        public class ContentReference
        {
            public JsonElement[] KeyValues { get; set; }
            [Required]
            public string EntityType { get; set; }
        }

        public class ChangedContent
        {
            [Required]
            public ContentReference ContentReference { get; set; }
            public bool Remove { get; set; }
            [Required]
            public SimpleChange[] SimpleChanges { get; set; }
        }

        public class SimpleChange
        {
            public string[] Path { get; set; }
            public string Value { get; set; }
        }

        public enum ArrayChangeType
        {
            Add,
            Update,
            Delete,
        }

        public class ArrayChange
        {
            public string Id { get; set; }
            public ArrayChangeType Type { get; set; }
            public string Value { get; set; }
        }

        public class PolymorphicValue
        {
            public string Type { get; set; }
            public string Value { get; set; }
        }
    }
}
