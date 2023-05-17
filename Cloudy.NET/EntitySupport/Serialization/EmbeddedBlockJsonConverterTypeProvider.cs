using Cloudy.NET.EntityTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public class EmbeddedBlockJsonConverterTypeProvider : IContentJsonConverterTypeProvider
    {
        IEntityTypeProvider EntityTypeProvider { get; }

        public EmbeddedBlockJsonConverterTypeProvider(IEntityTypeProvider entityTypeProvider)
        {
            EntityTypeProvider = entityTypeProvider;
        }

        public IEnumerable<Type> GetAll()
        {
            var types = new HashSet<Type>();

            foreach (var entityType in EntityTypeProvider.GetAll())
            {
                types.Add(entityType.Type);

                for (var baseType = entityType.Type; baseType != typeof(object); baseType = baseType.BaseType)
                {
                    types.Add(baseType);
                }

                foreach (var interfaceType in entityType.Type.GetInterfaces())
                {
                    types.Add(interfaceType);
                }
            }

            return types.ToList().AsReadOnly();
        }
    }
}
