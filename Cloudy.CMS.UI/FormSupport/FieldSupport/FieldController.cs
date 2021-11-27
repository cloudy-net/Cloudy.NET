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
using Cloudy.CMS.ContentTypeSupport;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class FieldController
    {
        ILogger Logger { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IFieldProvider FieldProvider { get; }
        IControlMatcher ControlMatcher { get; }
        IHumanizer Humanizer { get; }
        IPluralizer Pluralizer { get; }
        ISingularizer Singularizer { get; }
        IPolymorphicFormFinder PolymorphicFormFinder { get; }

        public FieldController(ILogger<FieldController> logger, IContentTypeProvider contentTypeProvider, IFieldProvider fieldProvider, IControlMatcher controlMatcher, IHumanizer humanizer, IPluralizer pluralizer, ISingularizer singularizer, IPolymorphicFormFinder polymorphicFormFinder)
        {
            Logger = logger;
            ContentTypeProvider = contentTypeProvider;
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
                var embeddedFormId = ContentTypeProvider.GetAll().FirstOrDefault(f => f.Type == field.Type);
                var isPolymorphic = field.Type.IsInterface;
                var polymorphicCandidates = isPolymorphic ? PolymorphicFormFinder.FindFor(field.Type).ToList().AsReadOnly() : new List<string>().AsReadOnly();

                if (control == null && embeddedFormId == null && !polymorphicCandidates.Any())
                {
                    Logger.LogInformation($"Could not find control for {id} {field.Name}");
                    continue;
                }

                var label = field.Label;

                if(label == null)
                {
                    label = field.Name;
                    label = Humanizer.Humanize(field.Name);

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
                    Id = field.Name,
                    Label = label,
                    SingularLabel = singularLabel,
                    Control = control,
                    EmbeddedFormId = embeddedFormId?.Id,
                    IsSortable = field.IsSortable,
                    Group = field.Group,
                    IsPolymorphic = isPolymorphic,
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
            public object Control { get; set; }
            public string EmbeddedFormId { get; set; }
            public bool IsSortable { get; set; }
            public string Group { get; set; }
            public bool IsPolymorphic { get; set; }
            public IEnumerable<string> PolymorphicCandidates { get; set; }
        }
    }
}
