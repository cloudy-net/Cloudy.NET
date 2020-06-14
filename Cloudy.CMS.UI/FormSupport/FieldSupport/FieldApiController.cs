using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using Cloudy.CMS.UI.FormSupport.ControlSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    [Area("Cloudy.CMS")]
    [Route("Field")]
    public class FieldApiController
    {
        ILogger Logger { get; }
        IFormProvider FormProvider { get; }
        IFieldProvider FieldProvider { get; }
        IControlMatcher ControlMatcher { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();
        IHumanizer Humanizer { get; }
        IPluralizer Pluralizer { get; }
        ISingularizer Singularizer { get; }

        public FieldApiController(ILogger<FieldApiController> logger, IFormProvider formProvider, IFieldProvider fieldProvider, IControlMatcher controlMatcher, IHumanizer humanizer, IPluralizer pluralizer, ISingularizer singularizer)
        {
            Logger = logger;
            FormProvider = formProvider;
            FieldProvider = fieldProvider;
            ControlMatcher = controlMatcher;
            Humanizer = humanizer;
            Pluralizer = pluralizer;
            Singularizer = singularizer;
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
                    Logger.LogInformation($"Could not find control for {id} {field.Id}");
                    continue;
                }

                var label = field.Label;

                if(label == null)
                {
                    label = field.Id;
                    label = Humanizer.Humanize(field.Id);

                    if (label.EndsWith(" ids"))
                    {
                        label = label.Substring(0, label.Length - " ids".Length);
                        label = Pluralizer.Pluralize(label);
                    }
                    else if (label.EndsWith(" id"))
                    {
                        label = label.Substring(0, label.Length - " id".Length);
                    }
                }

                var singularLabel = Singularizer.Singularize(label);

                result.Add(new FieldResponse
                {
                    Id = field.Id,
                    Label = label,
                    SingularLabel = singularLabel,
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
            public string SingularLabel { get; set; }
            public string CamelCaseId { get; set; }
            public object Control { get; set; }
            public string EmbeddedFormId { get; set; }
            public bool IsSortable { get; set; }
            public string Group { get; set; }
        }
    }
}
