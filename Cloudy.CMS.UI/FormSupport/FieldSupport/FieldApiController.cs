using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Poetry.UI.FormSupport.ControlSupport;
using Poetry.UI.FormSupport.ControlSupport.MatchingSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.FieldSupport
{
    [Area("Cloudy.CMS")]
    [Route("Field")]
    public class FieldApiController
    {
        IFormProvider FormProvider { get; }
        IFieldProvider FieldProvider { get; }
        IControlMatcher ControlMatcher { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();
        IHumanizer Humanizer { get; }

        public FieldApiController(IFormProvider formProvider, IFieldProvider fieldProvider, IControlMatcher controlMatcher, IHumanizer humanizer)
        {
            FormProvider = formProvider;
            FieldProvider = fieldProvider;
            ControlMatcher = controlMatcher;
            Humanizer = humanizer;
        }

        [Route("GetAllForForm")]
        public IEnumerable<FieldResponse> GetAllForForm(string id)
        {
            var result = new List<FieldResponse>();

            foreach(var field in FieldProvider.GetAllFor(id))
            {
                if (!field.AutoGenerate)
                {
                    continue;
                }

                var control = ControlMatcher.GetFor(field.Type, field.UIHints);
                var embeddedFormId = FormProvider.GetAll().FirstOrDefault(f => f.Type == field.Type);

                if(control == null && embeddedFormId == null)
                {
                    continue;
                }

                result.Add(new FieldResponse
                {
                    Id = field.Id,
                    Label = field.Label ?? Humanizer.Humanize(field.Id),
                    CamelCaseId = CamelCaseNamingStrategy.GetPropertyName(field.Id, false),
                    Control = control,
                    EmbeddedFormId = embeddedFormId?.Id,
                    IsSortable = field.IsSortable,
                    Group = field.Group,
                });
            }

            return result;
        }

        public class FieldResponse
        {
            public string Id { get; set; }
            public string Label { get; set; }
            public string CamelCaseId { get; set; }
            public object Control { get; set; }
            public string EmbeddedFormId { get; set; }
            public bool IsSortable { get; set; }
            public string Group { get; set; }
        }
    }
}
