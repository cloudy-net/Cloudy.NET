
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
        IFormProvider FormProvider { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IHumanizer Humanizer { get; }

        public PolymorphicFormController(IFormProvider formProvider, IContentTypeProvider contentTypeProvider, IHumanizer humanizer)
        {
            FormProvider = formProvider;
            ContentTypeProvider = contentTypeProvider;
            Humanizer = humanizer;
        }

        public IEnumerable<FormResponseItem> GetOptions(IEnumerable<string> types)
        {
            var result = new List<FormResponseItem>();

            foreach (var form in FormProvider.GetAll())
            {
                if (ContentTypeProvider.Get(form.Type) != null)
                {
                    continue; // content types are not allowed as embedded forms (is this an unnecessary constraint?)
                }

                if (!types.Contains(form.Id))
                {
                    continue;
                }

                result.Add(new FormResponseItem
                {
                    Type = form.Id,
                    Name = form.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? Humanizer.Humanize(form.Type.Name),
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
