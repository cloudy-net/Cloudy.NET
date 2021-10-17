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
using Microsoft.AspNetCore.Authorization;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport.PolymorphicControlMappingSupport;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class FieldController
    {
        ILogger Logger { get; }
        IFormProvider FormProvider { get; }
        IFieldProvider FieldProvider { get; }
        IControlMatcher ControlMatcher { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();
        IHumanizer Humanizer { get; }
        IPluralizer Pluralizer { get; }
        ISingularizer Singularizer { get; }
        IPolymorphicFormFinder PolymorphicFormFinder { get; }

        public FieldController(ILogger<FieldController> logger, IFormProvider formProvider, IFieldProvider fieldProvider, IControlMatcher controlMatcher, IHumanizer humanizer, IPluralizer pluralizer, ISingularizer singularizer, IPolymorphicFormFinder polymorphicFormFinder)
        {
            Logger = logger;
            FormProvider = formProvider;
            FieldProvider = fieldProvider;
            ControlMatcher = controlMatcher;
            Humanizer = humanizer;
            Pluralizer = pluralizer;
            Singularizer = singularizer;
            PolymorphicFormFinder = polymorphicFormFinder;
        }

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
                var polymorphicCandidates = field.Type.IsInterface ? PolymorphicFormFinder.FindFor(field.Type).ToList().AsReadOnly() : new List<string>().AsReadOnly();

                if (control == null && embeddedFormId == null && !polymorphicCandidates.Any())
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
                    IsPolymorphic = true,
                    PolymorphicCandidates = polymorphicCandidates,
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
            public bool IsPolymorphic { get; set; }
            public IEnumerable<string> PolymorphicCandidates { get; set; }
        }
    }
}
