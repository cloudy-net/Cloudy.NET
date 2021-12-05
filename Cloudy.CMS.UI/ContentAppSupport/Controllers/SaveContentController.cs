using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System.Collections;
using Cloudy.CMS.UI.FormSupport;
using System.Reflection;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class SaveContentController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContentGetter ContentGetter { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IFieldProvider FieldProvider { get; }
        IContentUpdater ContentUpdater { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        IContentCreator ContentCreator { get; }
        IContentDeleter ContentDeleter { get; }

        public SaveContentController(IContentTypeProvider contentTypeProvider, IPrimaryKeyConverter primaryKeyConverter, IContentGetter contentGetter, IPropertyDefinitionProvider propertyDefinitionProvider, IFieldProvider fieldProvider, IContentUpdater contentUpdater, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter, IContentCreator contentCreator, IContentDeleter contentDeleter)
        {
            ContentTypeProvider = contentTypeProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            ContentGetter = contentGetter;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            FieldProvider = fieldProvider;
            ContentUpdater = contentUpdater;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            ContentCreator = contentCreator;
            ContentDeleter = contentDeleter;
        }

        [HttpPost]
        public async Task<SaveContentResponse> SaveContent([FromBody] SaveContentRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                throw new Exception("Invalid state");
            }

            var result = new List<SaveContentResult>();

            foreach (var change in data.Changes)
            {
                var contentType = ContentTypeProvider.Get(change.ContentTypeId);
                var keyValues = PrimaryKeyConverter.Convert(change.KeyValues, contentType.Id);

                object content;

                if(keyValues == null)
                {
                    content = Activator.CreateInstance(contentType.Type);
                }
                else
                {
                    if (change.Remove)
                    {
                        await ContentDeleter.DeleteAsync(contentType.Id, keyValues).ConfigureAwait(false);
                        result.Add(SaveContentResult.SuccessResult(contentType.Id, keyValues));
                        continue;
                    }
                    content = await ContentGetter.GetAsync(contentType.Id, keyValues).ConfigureAwait(false);
                }

                var propertyDefinitions = PropertyDefinitionProvider.GetFor(contentType.Id).ToDictionary(p => p.Name, p => p);
                var idProperties = PrimaryKeyPropertyGetter.GetFor(content.GetType());

                if (change.ChangedFields.Any(c => c.Path.Length == 1 && idProperties.Any(p => p.Name == c.Path[0])))
                {
                    throw new Exception($"Tried to change primary key of content {string.Join(", ", keyValues)} with type {change.ContentTypeId}!");
                }

                var changedSimpleFields = change.ChangedFields.Where(f => f.Type == ChangedFieldType.Simple).ToList();

                foreach (var changedField in changedSimpleFields)
                {
                    UpdateSimpleField(content, changedField.Path, changedField.InitialValue, changedField.Value);
                }

                var changedArrayFields = change.ChangedFields.Where(f => f.Type == ChangedFieldType.Array).ToList();

                foreach (var changedField in changedArrayFields)
                {
                    var name = changedField.Path.Single();
                    var field = FieldProvider.GetFor(contentType.Id, name);
                    var property = contentType.Type.GetProperty(field.Name);
                    var array = (IList)property.GetGetMethod().Invoke(content, null);

                    if(array == null)
                    {
                        array = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(field.Type));
                        property.GetSetMethod().Invoke(content, new object[] { array });
                    }

                    if (field.Type.IsInterface)
                    {
                        foreach (var arrayChange in changedField.Changes) {
                            var polymorphicValue = JsonSerializer.Deserialize<PolymorphicValue>(arrayChange.Value);
                            var form = ContentTypeProvider.Get(polymorphicValue.Type);
                            var value = JsonSerializer.Deserialize(polymorphicValue.Value, form.Type);
                            array.Add(value);
                        }
                    }
                }

                if (!TryValidateModel(content))
                {
                    result.Add(SaveContentResult.ValidationFailureResult(contentType.Id, keyValues, ModelState.ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage))));
                    continue;
                }

                if (keyValues == null)
                {
                    await ContentCreator.CreateAsync(content).ConfigureAwait(false);
                }
                else
                {
                    await ContentUpdater.UpdateAsync(content).ConfigureAwait(false);
                }

                result.Add(SaveContentResult.SuccessResult(contentType.Id, keyValues));
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
            public string ContentTypeId { get; private set; }
            public IEnumerable<object> KeyValues { get; private set; }
            public IDictionary<string, IEnumerable<string>> ValidationErrors { get; private set; }

            public static SaveContentResult SuccessResult(string contentTypeId, IEnumerable<object> keyValues)
            {
                return new SaveContentResult
                {
                    Success = true,
                    ContentTypeId = contentTypeId,
                    KeyValues = keyValues?.ToList().AsReadOnly(),
                };
            }

            public static SaveContentResult ValidationFailureResult(string contentTypeId, IEnumerable<object> keyValues, IDictionary<string, IEnumerable<string>> validationErrors)
            {
                return new SaveContentResult
                {
                    Success = false,
                    ContentTypeId = contentTypeId,
                    KeyValues = keyValues.ToList().AsReadOnly(),
                    ValidationErrors = validationErrors.ToDictionary(e => e.Key, e => (IEnumerable<string>)e.Value.ToList().AsReadOnly()),
                };
            }
        }

        private void UpdateSimpleField(object instance, IEnumerable<string> path, string initialValue, string value)
        {
            var contentType = ContentTypeProvider.Get(instance.GetType());

            var name = path.First();
            var field = FieldProvider.GetFor(contentType.Id, name);
            var property = contentType.Type.GetProperty(field.Name);

            if (path.Count() == 1)
            {
                property.GetSetMethod().Invoke(instance, new object[] { Convert.ChangeType(value, property.PropertyType) });
                return;
            }

            var instanceValue = property.GetValue(instance);
            
            if (field.IsSortable)
            {
                var indexString = path.ElementAt(1);

                if (!indexString.StartsWith("original-"))
                {
                    throw new Exception("Simple updates to non-persisted array elements not supported");
                }

                var index = int.Parse(indexString.Substring("original-".Length));
                
                var elementAtMethod = typeof(Enumerable).GetMethods(BindingFlags.Static | BindingFlags.Public).Where(m => m.Name == nameof(Enumerable.ElementAt) && m.GetParameters()[1].ParameterType == typeof(int));
                var genericElementAtMethod = elementAtMethod.Single().MakeGenericMethod(field.Type);
                var element = genericElementAtMethod.Invoke(null, new object[] { instanceValue, index });

                UpdateSimpleField(element, path.Skip(2), initialValue, value);
            }
            else
            {
                throw new NotImplementedException("Nested types not implemented (yet!)");
            }
        }

        public class SaveContentRequestBody
        {
            public IEnumerable<ChangedContent> Changes { get; set; }
        }

        public class ChangedContent
        {
            public JsonElement[] KeyValues { get; set; }
            [Required]
            public string ContentTypeId { get; set; }
            public bool Remove { get; set; }
            [Required]
            public ChangedField[] ChangedFields { get; set; }
        }

        public static class ChangedFieldType
        {
            public static string Simple => "simple";
            public static string Array => "array";
        }

        public class ChangedField
        {
            public string[] Path { get; set; }
            public string Type { get; set; }
            public ArrayChange[] Changes { get; set; }
            public string InitialValue { get; set; }
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
