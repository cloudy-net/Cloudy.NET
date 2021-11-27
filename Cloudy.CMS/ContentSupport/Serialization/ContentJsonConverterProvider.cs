using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public class ContentJsonConverterProvider : IContentJsonConverterProvider
    {
        public static IContentJsonConverterProvider UglyInstance { get; set; }

        IEnumerable<JsonConverter> ContentJsonConverters { get; }

        public ContentJsonConverterProvider(IContentJsonConverterCreator contentJsonConverterCreator)
        {
            ContentJsonConverters = contentJsonConverterCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<JsonConverter> GetAll()
        {
            return ContentJsonConverters;
        }
    }
}
