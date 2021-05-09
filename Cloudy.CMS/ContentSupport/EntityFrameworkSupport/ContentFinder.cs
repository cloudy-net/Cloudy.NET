using Cloudy.CMS.ContentSupport.RepositorySupport;
using System;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContentFinder : IContentFinder
    {
        IContextProvider ContextProvider { get; }

        public ContentFinder(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public IContentFinderQuery Find(Type type)
        {
            var context = ContextProvider.GetFor(type);

            return new ContentFinderQuery(context.GetDbSet(type), type);
        }

        public IContentFinderQuery Find<T>() where T : class
        {
            return Find(typeof(T));
        }
    }
}