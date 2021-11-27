using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IContentJsonConverterProvider
    {
        IEnumerable<JsonConverter> GetAll();
    }
}