using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey
{
    public interface IPrimaryKeyPropertyGetter
    {
        IEnumerable<PropertyInfo> GetFor(Type type);
    }
}