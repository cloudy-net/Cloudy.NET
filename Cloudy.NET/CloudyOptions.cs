using Cloudy.NET.EntityTypeSupport;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.NET
{
    public class CloudyOptions
    {
        public ISet<Assembly> Assemblies { get; } = new HashSet<Assembly>();
        public ISet<Type> ContextTypes { get; } = new HashSet<Type>();
        public string LicenseKey { get; set; }
    }
}