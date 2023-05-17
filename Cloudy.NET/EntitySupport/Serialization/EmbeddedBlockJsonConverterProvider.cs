using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public class EmbeddedBlockJsonConverterProvider : IEmbeddedBlockJsonConverterProvider
    {
        public static IEmbeddedBlockJsonConverterProvider UglyInstance { get; set; }

        IEnumerable<JsonConverter> ContentJsonConverters { get; }

        public EmbeddedBlockJsonConverterProvider(IEmbeddedBlockJsonConverterCreator contentJsonConverterCreator)
        {
            ContentJsonConverters = contentJsonConverterCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<JsonConverter> GetAll()
        {
            return ContentJsonConverters;
        }
    }
}
