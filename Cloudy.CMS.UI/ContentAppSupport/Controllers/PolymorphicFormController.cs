
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.FormSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class PolymorphicFormController
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IHumanizer Humanizer { get; }

        public PolymorphicFormController(IContentTypeProvider contentTypeProvider, IHumanizer humanizer)
        {
            ContentTypeProvider = contentTypeProvider;
            Humanizer = humanizer;
        }

        public IEnumerable<FormResponseItem> GetOptions(IEnumerable<string> types)
        {
            var result = new List<FormResponseItem>();

            foreach (var contentType in ContentTypeProvider.GetAll())
            {
                if (!types.Contains(contentType.Id))
                {
                    continue;
                }

                result.Add(new FormResponseItem
                {
                    Type = contentType.Id,
                    Name = contentType.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? Humanizer.Humanize(contentType.Type.Name),
                });
            }
                
            return result.OrderBy(f => f.Name).ToList().AsReadOnly();
        }

        public class FormResponseItem
        {
            public string Type { get; set; }
            public string Name { get; set; }
        }
    }
}
