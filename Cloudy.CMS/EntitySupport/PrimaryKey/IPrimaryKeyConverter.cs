using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cloudy.CMS.EntitySupport.PrimaryKey
{
    public interface IPrimaryKeyConverter
    {
        object[] Convert(IEnumerable<string> keyValues, Type contentType);
    }
}
