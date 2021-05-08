using System;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContextWrapper : IContextWrapper
    {
        object Context { get; }

        public ContextWrapper(object context)
        {
            Context = context;
        }

        public IDbSetWrapper GetDbSet(Type type)
        {
            throw new NotImplementedException();
        }
    }
}