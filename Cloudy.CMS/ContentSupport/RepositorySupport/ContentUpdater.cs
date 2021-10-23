using Cloudy.CMS.ContentSupport.RepositorySupport;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentUpdater : IContentUpdater
    {
        IContextProvider ContextProvider { get; }

        public ContentUpdater(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public async Task UpdateAsync(object content)
        {
            var context = ContextProvider.GetFor(content.GetType());

            await context.Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}