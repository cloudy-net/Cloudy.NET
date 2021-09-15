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

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class SaveContentController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }
        IPrimaryKeyConverter PrimaryKeyConverter { get; }
        IContentGetter ContentGetter { get; }
        IContentTypeCoreInterfaceProvider ContentTypeCoreInterfaceProvider { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContentUpdater ContentUpdater { get; }
        IContentCreator ContentCreator { get; }
        PolymorphicFormConverter PolymorphicFormConverter { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();

        public SaveContentController(IContentTypeProvider contentTypeProvider, IPrimaryKeyGetter primaryKeyGetter, IPrimaryKeyConverter primaryKeyConverter, IContentGetter contentGetter, IContentTypeCoreInterfaceProvider contentTypeCoreInterfaceProvider, IPropertyDefinitionProvider propertyDefinitionProvider, IContentUpdater contentUpdater, IContentCreator contentCreator, PolymorphicFormConverter polymorphicFormConverter, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter)
        {
            ContentTypeProvider = contentTypeProvider;
            PrimaryKeyGetter = primaryKeyGetter;
            PrimaryKeyConverter = primaryKeyConverter;
            ContentGetter = contentGetter;
            ContentTypeCoreInterfaceProvider = contentTypeCoreInterfaceProvider;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContentUpdater = contentUpdater;
            ContentCreator = contentCreator;
            PolymorphicFormConverter = polymorphicFormConverter;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
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

                var content = await ContentGetter.GetAsync(contentType.Id, change.KeyValues).ConfigureAwait(false);

                var propertyDefinitions = PropertyDefinitionProvider.GetFor(contentType.Id).ToDictionary(p => CamelCaseNamingStrategy.GetPropertyName(p.Name, false), p => p);
                var idProperties = PrimaryKeyPropertyGetter.GetFor(content.GetType());

                foreach (var changedField in change.ChangedFields)
                {
                    var propertyDefinition = propertyDefinitions[changedField.Name];

                    if(idProperties.Any(p => p.Name == propertyDefinition.Name))
                    {
                        throw new Exception("Tried to change primary key!");
                    }

                    var display = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

                    if (display != null && display.GetAutoGenerateField() == false)
                    {
                        continue;
                    }

                    propertyDefinition.Setter(content, changedField.Value);
                }

                if (!TryValidateModel(content))
                {
                    return ContentResponseMessage.CreateFrom(ModelState);
                }

                await ContentUpdater.UpdateAsync(content).ConfigureAwait(false);
            }

            return new ContentResponseMessage(true, "Updated");
        }

        public class SaveContentRequestBody
        {
            public IEnumerable<SaveContentRequestBodyChange> Changes { get; set; }
        }

        public class SaveContentRequestBodyChange
        {
            [Required]
            public string[] KeyValues { get; set; }
            [Required]
            public string ContentTypeId { get; set; }
            [Required]
            public SaveContentMappingChange[] ChangedFields { get; set; }
        }

        public class SaveContentMappingChange
        {
            public string Name { get; set; }
            public string Value { get; set; }
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
