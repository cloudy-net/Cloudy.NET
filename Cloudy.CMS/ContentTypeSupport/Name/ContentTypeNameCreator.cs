using Cloudy.CMS.Naming;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.ContentTypeSupport.Name
{
    public record ContentTypeNameCreator(IContentTypeProvider ContentTypeProvider, IHumanizer Humanizer, IPluralizer Pluralizer) : IContentTypeNameCreator
    {
        public IEnumerable<ContentTypeNameDescriptor> Create()
        {
            var result = new List<ContentTypeNameDescriptor>();

            foreach (var type in ContentTypeProvider.GetAll().Select(t => t.Type))
            {
                var name = type.GetCustomAttribute<DisplayAttribute>()?.Name ?? Humanizer.Humanize(type.Name);
                var pluralName = Pluralizer.Pluralize(name);

                result.Add(new ContentTypeNameDescriptor(
                    type,
                    name,
                    name.Substring(0, 1).ToLower() + name.Substring(1),
                    pluralName,
                    pluralName.Substring(0, 1).ToLower() + pluralName.Substring(1)
                ));
            }

            return result.AsReadOnly();
        }
    }
}