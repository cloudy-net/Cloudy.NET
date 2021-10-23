using Cloudy.CMS.ContentSupport.RepositorySupport;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport
{
    public class ContentSaver : IContentSaver
    {
        IContextProvider ContextProvider { get; }

        public ContentSaver(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public async Task SaveAsync(object content)
        {
            var context = ContextProvider.GetFor(content.GetType());

            var dbSet = context.GetDbSet(content.GetType());

            await dbSet.AddAsync(content).ConfigureAwait(false);

            await context.Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}