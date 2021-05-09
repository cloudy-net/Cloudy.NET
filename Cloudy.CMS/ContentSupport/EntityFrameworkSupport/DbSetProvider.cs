using Cloudy.CMS.ContentTypeSupport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class DbSetProvider : IDbSetProvider
    {
        IContextProvider ContextProvider { get; }

        public DbSetProvider(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public IDbSetWrapper Get(Type type)
        {
            var context = ContextProvider.GetFor(type);

            return context.GetDbSet(type);
        }
    }
}
