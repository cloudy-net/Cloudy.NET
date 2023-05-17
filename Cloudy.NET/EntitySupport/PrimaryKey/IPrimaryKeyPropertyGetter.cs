using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.NET.EntitySupport.PrimaryKey
{
    public interface IPrimaryKeyPropertyGetter
    {
        IEnumerable<PropertyInfo> GetFor(Type type);
    }
}