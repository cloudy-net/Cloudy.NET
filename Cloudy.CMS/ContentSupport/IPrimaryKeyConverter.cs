using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cloudy.CMS.ContentSupport
{
    public interface IPrimaryKeyConverter
    {
        object[] Convert(IEnumerable<JsonElement> keyValues, string contentTypeId);
    }
}
