using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS
{
    public class CloudyOptions
    {
        public ISet<Assembly> Assemblies { get; } = new HashSet<Assembly>();
    }
}