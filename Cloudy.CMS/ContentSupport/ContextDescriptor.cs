using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentSupport
{
    public class ContextDescriptor
    {
        public Type Type { get; }
        public IEnumerable<PropertyInfo> DbSets { get; }

        public ContextDescriptor(Type type, IEnumerable<PropertyInfo> dbSets)
        {
            Type = type;
            DbSets = dbSets.ToList().AsReadOnly();
        }

        public static IEnumerable<ContextDescriptor> CreateFrom(IDictionary<Type, IEnumerable<PropertyInfo>> entries)
        {
            var result = new List<ContextDescriptor>();

            foreach (var entry in entries)
            {
                result.Add(new ContextDescriptor(entry.Key, entry.Value));
            }

            return result.AsReadOnly();
        }
    }
}
