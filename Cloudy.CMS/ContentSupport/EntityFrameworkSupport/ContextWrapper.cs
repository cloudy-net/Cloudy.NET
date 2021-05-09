using Microsoft.EntityFrameworkCore;
using System;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContextWrapper : IContextWrapper
    {
        public DbContext Context { get; }

        public ContextWrapper(DbContext context)
        {
            Context = context;
        }

        public IDbSetWrapper GetDbSet(Type type)
        {
            throw new NotImplementedException();
        }
    }
}