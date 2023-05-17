using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cloudy.NET.EntitySupport.Serialization
{
    public interface IEmbeddedBlockJsonConverterProvider
    {
        IEnumerable<JsonConverter> GetAll();
    }
}