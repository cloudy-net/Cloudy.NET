using Cloudy.CMS.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Cloudy.CMS
{
    public class CloudyOptions
    {
        public List<Type> Components { get; } = new List<Type>();
        public List<Assembly> ComponentAssemblies { get; } = new List<Assembly>();
        public bool HasDocumentProvider { get; set; }
    }
}