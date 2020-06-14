
using Cloudy.CMS.UI.ContentAppSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.Controls
{
    [Area("Cloudy.CMS")]
    [Route("PolymorphicFormControl")]
    public class PolymorphicFormApiController
    {
        IFormProvider FormProvider { get; }
        IHumanizer Humanizer { get; }

        public PolymorphicFormApiController(IFormProvider formProvider, IHumanizer humanizer)
        {
            FormProvider = formProvider;
            Humanizer = humanizer;
        }

        [Route("GetOptions")]
        public IEnumerable<FormResponseItem> GetOptions(IEnumerable<string> types)
        {
            var result = new List<FormResponseItem>();

            foreach (var form in FormProvider.GetAll())
            {
                if (!types.Contains(form.Id))
                {
                    continue;
                }

                result.Add(new FormResponseItem
                {
                    FormId = form.Id,
                    Name = form.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? Humanizer.Humanize(form.Type.Name),
                });
            }
                
            return result.OrderBy(f => f.Name).ToList().AsReadOnly();
        }

        public class FormResponseItem
        {
            public string FormId { get; set; }
            public string Name { get; set; }
        }
    }
}
