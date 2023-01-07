using Cloudy.CMS.ContentTypeSupport;
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
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class SaveContentController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContextCreator ContextCreator { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IFieldProvider FieldProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }

        public SaveContentController(IContentTypeProvider contentTypeProvider, IPrimaryKeyConverter primaryKeyConverter, IContextCreator contextCreator, IPrimaryKeyGetter primaryKeyGetter, IPropertyDefinitionProvider propertyDefinitionProvider, IFieldProvider fieldProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            ContextCreator = contextCreator;
            PrimaryKeyGetter = primaryKeyGetter;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            FieldProvider = fieldProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
        }

        [HttpPost]
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
                var contentType = ContentTypeProvider.Get(changedContent.ContentReference.ContentTypeId);
                var context = ContextCreator.CreateFor(contentType.Type);

                contexts.Add(context);

                var keyValues = PrimaryKeyConverter.Convert(changedContent.ContentReference.KeyValues.Select(k => k.ToString()), contentType.Type);

                object content;

                if (keyValues == null)
                {
                    content = Activator.CreateInstance(contentType.Type);
                }
                else
                {
                    content = await context.Context.FindAsync(contentType.Type, keyValues).ConfigureAwait(false);

                    if (changedContent.Remove)
                    {
                        context.Context.Remove(content);
                        result.Add(SaveContentResult.SuccessResult(contentType.Name, keyValues));
                        continue;
                    }
                }

                var propertyDefinitions = PropertyDefinitionProvider.GetFor(contentType.Name).ToDictionary(p => p.Name, p => p);
                var idProperties = PrimaryKeyPropertyGetter.GetFor(content.GetType());

                if (changedContent.SimpleChanges.Any(c => c.Path.Length == 1 && idProperties.Any(p => p.Name == c.Path[0])))
                {
                    throw new Exception($"Tried to change primary key of content {string.Join(", ", keyValues)} with type {changedContent.ContentReference.ContentTypeId}!");
                }

                var changedSimpleFields = changedContent.SimpleChanges.ToList();

                foreach (var change in changedSimpleFields)
                {
                    UpdateSimpleField(content, change.Path, change.Value);
                }

                //var arrayChanges = changedContent.SimpleChanges.Where(f => f.Type == ChangeType.Array).ToList();

                //foreach (var change in arrayChanges)
                //{
                //    var name = change.Path.Single();
                //    var field = FieldProvider.GetFor(contentType.Id, name);
                //    var property = contentType.Type.GetProperty(field.Name);
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
                //            var form = ContentTypeProvider.Get(polymorphicValue.Type);
                //            var value = JsonSerializer.Deserialize(polymorphicValue.Value, form.Type);
                //            array.Add(value);
                //        }
                //    }
                //}

                if (!TryValidateModel(content))
                {
                    result.Add(SaveContentResult.ValidationFailureResult(contentType.Name, keyValues, ModelState.ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage))));
                    continue;
                }

                if (keyValues == null)
                {
                    await context.Context.AddAsync(content).ConfigureAwait(false);
                }
                else
                {
                    //await ContentUpdater.UpdateAsync(content).ConfigureAwait(false);
                }

                result.Add(SaveContentResult.SuccessResult(contentType.Name, PrimaryKeyGetter.Get(content)));
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

            public static SaveContentResult SuccessResult(string contentTypeId, IEnumerable<object> keyValues)
            {
                return new SaveContentResult
                {
                    Success = true,
                    ContentReference = new ContentReference
                    {
                        ContentTypeId = contentTypeId,
                        KeyValues = keyValues?.Select(k => JsonSerializer.SerializeToElement(k)).ToArray(),
                    },
                };
            }

            public static SaveContentResult ValidationFailureResult(string contentTypeId, IEnumerable<object> keyValues, IDictionary<string, IEnumerable<string>> validationErrors)
            {
                return new SaveContentResult
                {
                    Success = false,
                    ContentReference = new ContentReference
                    {
                        ContentTypeId = contentTypeId,
                        KeyValues = keyValues?.Select(k => JsonSerializer.SerializeToElement(k)).ToArray(),
                    },
                    ValidationErrors = validationErrors.ToDictionary(e => e.Key, e => (IEnumerable<string>)e.Value.ToList().AsReadOnly()),
                };
            }
        }

        private void UpdateSimpleField(object target, IEnumerable<string> path, string value)
        {
            var contentType = ContentTypeProvider.Get(target.GetType());

            var name = path.First();

            path = path.Skip(1);

            var field = FieldProvider.Get(contentType.Name).FirstOrDefault(f => f.Name == name);
            var property = contentType.Type.GetProperty(field.Name);

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
            public string ContentTypeId { get; set; }
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
