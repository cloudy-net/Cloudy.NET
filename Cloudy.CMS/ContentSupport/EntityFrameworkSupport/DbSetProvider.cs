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
        IContentTypeProvider ContentTypeProvider { get; }

        public DbSetProvider(IContextProvider contextProvider, IContentTypeProvider contentTypeProvider)
        {
            ContextProvider = contextProvider;
            ContentTypeProvider = contentTypeProvider;
        }

        public IDbSetWrapper Get(string contentTypeId)
        {
            var contentType = ContentTypeProvider.Get(contentTypeId);

            var context = ContextProvider.GetFor(contentType.Type);

            return context.GetDbSet(contentType.Type);
        }

        public IDbSetWrapper Get<T>() where T : class
        {
            var context = ContextProvider.GetFor(typeof(T));

            return context.GetDbSet(typeof(T));
        }
    }
}
