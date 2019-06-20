using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;

namespace Cloudy.CMS
{
    public class CloudyOptions
    {
        public List<Type> Components { get; } = new List<Type>();
        public string DatabaseConnectionString { get; set; }
    }
}