using System;
using System.Collections.Generic;
using System.Linq;

namespace Cloudy.CMS.ContextSupport
{
    public class ContextDescriptor
    {
        public Type Type { get; }
        public IEnumerable<DbSetDescriptor> DbSets { get; }

        public ContextDescriptor(Type type, IEnumerable<DbSetDescriptor> dbSets)
        {
            Type = type;
            DbSets = dbSets.ToList().AsReadOnly();
        }
    }
}
