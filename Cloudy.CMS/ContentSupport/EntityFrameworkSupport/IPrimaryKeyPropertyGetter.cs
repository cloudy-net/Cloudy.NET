using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IPrimaryKeyPropertyGetter
    {
        IEnumerable<PropertyInfo> GetFor(Type type);
    }
}