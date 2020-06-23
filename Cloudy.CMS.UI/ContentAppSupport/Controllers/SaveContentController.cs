using Cloudy.CMS.ContainerSpecificContentSupport.RepositorySupport;
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

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize]
    [Area("Cloudy.CMS")]
    public class SaveContentController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IContainerSpecificContentGetter ContainerSpecificContentGetter { get; }
        IContentTypeCoreInterfaceProvider ContentTypeCoreInterfaceProvider { get; }
        IPropertyDefinitionProvider PropertyDefinitionProvider { get; }
        IContainerSpecificContentUpdater ContainerSpecificContentUpdater { get; }
        IContainerSpecificContentCreator ContainerSpecificContentCreator { get; }
        PolymorphicFormConverter PolymorphicFormConverter { get; }

        public SaveContentController(IContentTypeProvider contentTypeProvider, IContainerSpecificContentGetter containerSpecificContentGetter, IContentTypeCoreInterfaceProvider contentTypeCoreInterfaceProvider, IPropertyDefinitionProvider propertyDefinitionProvider, IContainerSpecificContentUpdater containerSpecificContentUpdater, IContainerSpecificContentCreator containerSpecificContentCreator, PolymorphicFormConverter polymorphicFormConverter)
        {
            ContentTypeProvider = contentTypeProvider;
            ContainerSpecificContentGetter = containerSpecificContentGetter;
            ContentTypeCoreInterfaceProvider = contentTypeCoreInterfaceProvider;
            PropertyDefinitionProvider = propertyDefinitionProvider;
            ContainerSpecificContentUpdater = containerSpecificContentUpdater;
            ContainerSpecificContentCreator = containerSpecificContentCreator;
            PolymorphicFormConverter = polymorphicFormConverter;
        }

        [HttpPost]
        public ContentResponseMessage SaveContent([FromBody] SaveContentRequestBody data)
        {
            if (!ModelState.IsValid)
            {
                return ContentResponseMessage.CreateFrom(ModelState);
            }

            var contentType = ContentTypeProvider.Get(data.ContentTypeId);

            var b = (IContent)JsonConvert.DeserializeObject(data.Content, contentType.Type, PolymorphicFormConverter);

            if (b.Id != null)
            {
                var a = (IContent)typeof(IContainerSpecificContentGetter).GetMethod(nameof(ContainerSpecificContentGetter.Get)).MakeGenericMethod(contentType.Type).Invoke(ContainerSpecificContentGetter, new[] { data.Id, null, contentType.Container });

                foreach(var coreInterface in ContentTypeCoreInterfaceProvider.GetFor(contentType.Id))
                {
                    foreach(var propertyDefinition in coreInterface.PropertyDefinitions)
                    {
                        var display = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

                        if (display != null && display.GetAutoGenerateField() == false)
                        {
                            continue;
                        }

                        propertyDefinition.Setter(a, propertyDefinition.Getter(b));
                    }
                }

                foreach (var propertyDefinition in PropertyDefinitionProvider.GetFor(contentType.Id))
                {
                    var display = propertyDefinition.Attributes.OfType<DisplayAttribute>().FirstOrDefault();

                    if (display != null && display.GetAutoGenerateField() == false)
                    {
                        continue;
                    }

                    propertyDefinition.Setter(a, propertyDefinition.Getter(b));
                }

                ContainerSpecificContentUpdater.Update(a, contentType.Container);

                return new ContentResponseMessage(true, "Updated");
            }
            else
            {
                ContainerSpecificContentCreator.Create(b, contentType.Container);

                return new ContentResponseMessage(true, "Created");
            }
        }

        public class SaveContentRequestBody
        {
            public string Id { get; set; }
            [Required]
            public string ContentTypeId { get; set; }
            [Required]
            public string Content { get; set; }
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
