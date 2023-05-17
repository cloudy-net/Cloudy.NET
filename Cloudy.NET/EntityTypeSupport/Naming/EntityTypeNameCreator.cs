using Cloudy.CMS.Naming;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Cloudy.CMS.EntityTypeSupport.Naming
{
    public record EntityTypeNameCreator(IEntityTypeProvider EntityTypeProvider, IHumanizer Humanizer, IPluralizer Pluralizer) : IEntityTypeNameCreator
    {
        public IEnumerable<EntityTypeName> Create()
        {
            var result = new List<EntityTypeName>();

            foreach (var type in EntityTypeProvider.GetAll())
            {
                var name = type.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? Humanizer.Humanize(type.Name);
                var pluralName = Pluralizer.Pluralize(name);

                result.Add(new EntityTypeName(
                    type.Type,
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