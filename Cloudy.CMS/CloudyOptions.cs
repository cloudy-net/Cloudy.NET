using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS
{
    public class CloudyOptions
    {
        public ISet<Assembly> Assemblies { get; } = new HashSet<Assembly>();
        public IDictionary<Type, IEnumerable<PropertyInfo>> Contexts { get; } = new Dictionary<Type, IEnumerable<PropertyInfo>>();
    }
}