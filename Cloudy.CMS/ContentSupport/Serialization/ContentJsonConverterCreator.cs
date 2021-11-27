using Cloudy.CMS.ContentTypeSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentJsonConverterCreator : IContentJsonConverterCreator
    {
        IContentJsonConverterTypeProvider ContentJsonConverterTypeProvider { get; }
        IServiceProvider ServiceProvider { get; }

        public ContentJsonConverterCreator(IContentJsonConverterTypeProvider contentJsonConverterTypeProvider, IServiceProvider serviceProvider)
        {
            ContentJsonConverterTypeProvider = contentJsonConverterTypeProvider;
            ServiceProvider = serviceProvider;
        }

        public IEnumerable<JsonConverter> Create()
        {
            return ContentJsonConverterTypeProvider
                .GetAll()
                .Select(t => typeof(ContentJsonConverter<>)
                .MakeGenericType(t))
                .Select(t => ActivatorUtilities.CreateInstance(ServiceProvider, t))
                .Cast<JsonConverter>();
        }
    }
}
