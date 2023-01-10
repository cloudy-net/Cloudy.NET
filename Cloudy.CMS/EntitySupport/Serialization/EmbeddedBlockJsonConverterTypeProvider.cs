using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.EntitySupport.Serialization
{
    public class EmbeddedBlockJsonConverterTypeProvider : IContentJsonConverterTypeProvider
    {
        IContentTypeProvider ContentTypeProvider { get; }

        public EmbeddedBlockJsonConverterTypeProvider(IContentTypeProvider contentTypeProvider)
        {
            ContentTypeProvider = contentTypeProvider;
        }

        public IEnumerable<Type> GetAll()
        {
            var types = new HashSet<Type>();

            foreach (var contentType in ContentTypeProvider.GetAll())
            {
                types.Add(contentType.Type);

                for (var baseType = contentType.Type; baseType != typeof(object); baseType = baseType.BaseType)
                {
                    types.Add(baseType);
                }

                foreach (var interfaceType in contentType.Type.GetInterfaces())
                {
                    types.Add(interfaceType);
                }
            }

            return types.ToList().AsReadOnly();
        }
    }
}
