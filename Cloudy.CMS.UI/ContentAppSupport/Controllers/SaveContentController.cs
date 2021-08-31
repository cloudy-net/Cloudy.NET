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

        public SaveContentController(IContentTypeProvider contentTypeProvider, IPrimaryKeyGetter primaryKeyGetter, IPrimaryKeyConverter primaryKeyConverter, IContentGetter contentGetter, IContentTypeCoreInterfaceProvider contentTypeCoreInterfaceProvider, IPropertyDefinitionProvider propertyDefinitionProvider, IContentUpdater contentUpdater, IContentCreator contentCreator, PolymorphicFormConverter polymorphicFormConverter)
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
        }

        [HttpPost]
        public async Task<ContentResponseMessage> SaveContent([FromBody] SaveContentRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                return ContentResponseMessage.CreateFrom(ModelState);
            }

            return new ContentResponseMessage(true, "This functionality is not done yet, but this is the data sent in: \n\n" + JsonConvert.SerializeObject(data));

            //var contentType = ContentTypeProvider.Get(data.ContentTypeId);

            //var b = JsonConvert.DeserializeObject(data.Content, contentType.Type, PolymorphicFormConverter);

            //if (!TryValidateModel(b))
            //{
            //    return ContentResponseMessage.CreateFrom(ModelState);
            //}

            //if (PrimaryKeyGetter.Get(b).All(id => id != null))
            //{
            //    var a = await ContentGetter.GetAsync(contentType.Id, PrimaryKeyConverter.Convert(data.KeyValues, contentType.Id)).ConfigureAwait(false);
            //}

            //    foreach(var coreInterface in ContentTypeCoreInterfaceProvider.GetFor(contentType.Id))
            //    {
            //        foreach(var propertyDefinition in coreInterface.PropertyDefinitions)
            //        {
            //            var display = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

            //            if (display != null && display.GetAutoGenerateField() == false)
            //            {
            //                continue;
            //            }
            //            var display = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

            //            propertyDefinition.Setter(a, propertyDefinition.Getter(b));
            //        }
            //    }
            //            }

            //    foreach (var propertyDefinition in PropertyDefinitionProvider.GetFor(contentType.Id))
            //    {
            //        var display = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

            //        if (display != null && display.GetAutoGenerateField() == false)
            //        {
            //            continue;
            //        }

            //        propertyDefinition.Setter(a, propertyDefinition.Getter(b));
            //    }
            //            continue;
            //        }

            //    if (!TryValidateModel(b))
            //    {
            //        throw new Exception("a was valid but b was not.");
            //    }

            //    await ContentUpdater.UpdateAsync(a).ConfigureAwait(false);
            //    {
            //        throw new Exception("a was valid but b was not.");
            //    }

            //    return new ContentResponseMessage(true, "Updated");
            //}
            //else
            //{
            //    await ContentCreator.CreateAsync(b).ConfigureAwait(false);

            //    return new ContentResponseMessage(true, "Created");
            //}
            //else
            //{
            //    await ContentCreator.CreateAsync(b).ConfigureAwait(false);

            //    return new ContentResponseMessage(true, "Created");
            //}
        }

        public class SaveContentRequestBody
        {
            public IEnumerable<SaveContentRequestBodyChange> Changes { get; set; }
        }

        public class SaveContentRequestBodyChange
        {
            public string[] KeyValues { get; set; }
            [Required]
            public string ContentId { get; set; }
            [Required]
            public string ContentTypeId { get; set; }
            [Required]
            public SaveContentMappingChange[] ChangedFields { get; set; }
        }

        public class SaveContentMappingChange
        {
            public string Path { get; set; }
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
