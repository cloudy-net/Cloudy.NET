using Cloudy.CMS.ContentSupport.RepositorySupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Serialization;
using System.Text.Json;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using System.Collections;
using Cloudy.CMS.UI.FormSupport;

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
        IFormProvider FormProvider { get; }
        IFieldProvider FieldProvider { get; }
        IContentUpdater ContentUpdater { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();
        IContentCreator ContentCreator { get; }

        public SaveContentController(IContentTypeProvider contentTypeProvider, IPrimaryKeyConverter primaryKeyConverter, IContentGetter contentGetter, IPropertyDefinitionProvider propertyDefinitionProvider, IFormProvider formProvider, IFieldProvider fieldProvider, IContentUpdater contentUpdater, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter, IContentCreator contentCreator)
        {
            ContentTypeProvider = contentTypeProvider;
            PrimaryKeyConverter = primaryKeyConverter;
            ContentGetter = contentGetter;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            FormProvider = formProvider;
            FieldProvider = fieldProvider;
            ContentUpdater = contentUpdater;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            ContentCreator = contentCreator;
        }

        [HttpPost]
        public async Task<ContentResponseMessage> SaveContent([FromBody] SaveContentRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                return ContentResponseMessage.CreateFrom(ModelState);
            }

            foreach (var change in data.Changes)
            {
                var contentType = ContentTypeProvider.Get(change.ContentTypeId);

                object content;

                if(change.KeyValues == null)
                {
                    content = Activator.CreateInstance(contentType.Type);
                }
                else
                {
                    content = await ContentGetter.GetAsync(contentType.Id, PrimaryKeyConverter.Convert(change.KeyValues, contentType.Id)).ConfigureAwait(false);
                }

                var propertyDefinitions = PropertyDefinitionProvider.GetFor(contentType.Id).ToDictionary(p => CamelCaseNamingStrategy.GetPropertyName(p.Name, false), p => p);
                var idProperties = PrimaryKeyPropertyGetter.GetFor(content.GetType());

                if (change.ChangedFields.Any(c => c.Path.Length == 1 && idProperties.Any(p => p.Name == c.Path[0])))
                {
                    throw new Exception($"Tried to change primary key of content {string.Join(", ", change.KeyValues)} with type {change.ContentTypeId}!");
                }

                var fields = FieldProvider.GetAllFor(change.ContentTypeId).ToDictionary(f => f.Id, f => f);

                var changedSimpleFields = change.ChangedFields.Where(f => f.Type == ChangedFieldType.Simple).ToList();

                foreach (var changedField in changedSimpleFields)
                {
                    var name = changedField.Path.Single();
                    var field = fields[name.Substring(0, 1).ToUpper() + name.Substring(1)];
                    var property = contentType.Type.GetProperty(field.Id);

                    property.GetSetMethod().Invoke(content, new object[] { Convert.ChangeType(changedField.Value, property.PropertyType) });
                }

                var changedArrayFields = change.ChangedFields.Where(f => f.Type == ChangedFieldType.Array).ToList();

                foreach (var changedField in changedArrayFields)
                {
                    var name = changedField.Path.Single();
                    var field = fields[name.Substring(0, 1).ToUpper() + name.Substring(1)];
                    var property = contentType.Type.GetProperty(name);
                    var array = (IList)property.GetGetMethod().Invoke(content, null);

                    

                    if (field.Type.IsInterface)
                    {
                        foreach (var arrayChange in changedField.Changes) {
                            var polymorphicValue = System.Text.Json.JsonSerializer.Deserialize<PolymorphicValue>(arrayChange.Value.GetRawText());
                            var form = FormProvider.Get(polymorphicValue.Type);
                            var value = System.Text.Json.JsonSerializer.Deserialize(polymorphicValue.Value.GetRawText(), form.Type);
                            array.Add(value);
                        }
                    }
                }

                if (!TryValidateModel(content))
                {
                    return ContentResponseMessage.CreateFrom(ModelState);
                }

                if (change.KeyValues == null)
                {
                    await ContentCreator.CreateAsync(content).ConfigureAwait(false);
                }
                else
                {
                    await ContentUpdater.UpdateAsync(content).ConfigureAwait(false);
                }
            }

            return new ContentResponseMessage(true, "Updated");
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
            public JsonElement Value { get; set; }
        }

        public class PolymorphicValue
        {
            public string Type { get; set; }
            public JsonElement Value { get; set; }
        }

        public class ContentResponseMessage
        {
            public bool Success { get; }
            public string Message { get; }
            public IDictionary<string, IEnumerable<string>> ValidationErrors { get; }

            public ContentResponseMessage(bool success)
            {
                Success = success;
                ValidationErrors = new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>());
            }

            public ContentResponseMessage(bool success, string message)
            {
                Success = success;
                Message = message;
                ValidationErrors = new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>());
            }

            public ContentResponseMessage(IDictionary<string, IEnumerable<string>> validationErrors)
            {
                Success = false;
                Message = "Validation failed";
                ValidationErrors = new ReadOnlyDictionary<string, IEnumerable<string>>(new Dictionary<string, IEnumerable<string>>(validationErrors));
            }

            public static ContentResponseMessage CreateFrom(ModelStateDictionary modelState)
            {
                return new ContentResponseMessage(modelState.ToDictionary(i => i.Key, i => i.Value.Errors.Select(e => e.ErrorMessage)));
            }
        }
    }
}
