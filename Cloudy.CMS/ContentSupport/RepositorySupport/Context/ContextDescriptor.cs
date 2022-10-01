using Cloudy.CMS.ContentSupport.RepositorySupport.DbSet;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
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
