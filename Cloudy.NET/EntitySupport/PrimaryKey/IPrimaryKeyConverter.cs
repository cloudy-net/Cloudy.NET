using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;

namespace Cloudy.NET.EntitySupport.PrimaryKey
{
    public interface IPrimaryKeyConverter
    {
        object[] Convert(IEnumerable<string> keyValues, Type entityType);
    }
}
