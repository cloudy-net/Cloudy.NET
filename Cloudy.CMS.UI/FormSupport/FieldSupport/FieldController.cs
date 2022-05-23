using Cloudy.CMS.UI.ContentAppSupport;
using Cloudy.CMS.UI.FormSupport.ControlSupport.MatchingSupport;
using Microsoft.AspNetCore.Mvc;
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
using Cloudy.CMS.ContentSupport.RepositorySupport;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    [Authorize("Cloudy.CMS.UI")]
    [Area("Cloudy.CMS")]
    public class FieldController
    {
        ILogger Logger { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IContextDescriptorProvider ContextDescriptorProvider { get; }
        IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter { get; }
        IFieldProvider FieldProvider { get; }
        IControlMatcher ControlMatcher { get; }
        IHumanizer Humanizer { get; }
        IPluralizer Pluralizer { get; }
        ISingularizer Singularizer { get; }
        IPolymorphicFormFinder PolymorphicFormFinder { get; }

        public FieldController(ILogger<FieldController> logger, IContentTypeProvider contentTypeProvider, IContextDescriptorProvider contextDescriptorProvider, IPrimaryKeyPropertyGetter primaryKeyPropertyGetter, IFieldProvider fieldProvider, IControlMatcher controlMatcher, IHumanizer humanizer, IPluralizer pluralizer, ISingularizer singularizer, IPolymorphicFormFinder polymorphicFormFinder)
        {
            Logger = logger;
            ContentTypeProvider = contentTypeProvider;
            ContextDescriptorProvider = contextDescriptorProvider;
            PrimaryKeyPropertyGetter = primaryKeyPropertyGetter;
            FieldProvider = fieldProvider;
            ControlMatcher = controlMatcher;
            Humanizer = humanizer;
            Pluralizer = pluralizer;
            Singularizer = singularizer;
            PolymorphicFormFinder = polymorphicFormFinder;
        }

        public IDictionary<string, IEnumerable<FieldResponse>> GetAll()
        {
            var result = new Dictionary<string, IEnumerable<FieldResponse>>();

            foreach (var contentType in ContentTypeProvider.GetAll())
            {

                ISet<string> primaryKeys = null;
                
                if (ContextDescriptorProvider.GetFor(contentType.Type) != null)
                {
                    primaryKeys = new HashSet<string>(PrimaryKeyPropertyGetter.GetFor(contentType.Type).Select(p => p.Name));
                }

                var fields = new List<FieldResponse>();

                foreach (var field in FieldProvider.GetAllFor(contentType.Id))
                {
                    if (primaryKeys?.Contains(field.Name) ?? false)
                    {
                        continue;
                    }

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
                        Logger.LogInformation($"Could not find control for {contentType.Id} {field.Name}");
                        continue;
                    }

                    var label = field.Label;

                    if (label == null)
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

                    fields.Add(new FieldResponse
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

                result[contentType.Id] = fields;
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
