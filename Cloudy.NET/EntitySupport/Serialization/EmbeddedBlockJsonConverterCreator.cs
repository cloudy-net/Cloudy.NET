using Cloudy.NET.EntityTypeSupport;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public class EmbeddedBlockJsonConverterCreator : IEmbeddedBlockJsonConverterCreator
    {
        IContentJsonConverterTypeProvider ContentJsonConverterTypeProvider { get; }
        IServiceProvider ServiceProvider { get; }

        public EmbeddedBlockJsonConverterCreator(IContentJsonConverterTypeProvider contentJsonConverterTypeProvider, IServiceProvider serviceProvider)
        {
            ContentJsonConverterTypeProvider = contentJsonConverterTypeProvider;
            ServiceProvider = serviceProvider;
        }

        public IEnumerable<JsonConverter> Create()
        {
            return ContentJsonConverterTypeProvider
                .GetAll()
                .Select(t => typeof(EmbeddedBlockJsonConverter<>)
                .MakeGenericType(t))
                .Select(t => ActivatorUtilities.CreateInstance(ServiceProvider, t))
                .Cast<JsonConverter>();
        }
    }
}
