using Cloudy.CMS.ContentSupport.RepositorySupport;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContentCreator : IContentCreator
    {
        IContextProvider ContextProvider { get; }

        public ContentCreator(IContextProvider contextProvider)
        {
            ContextProvider = contextProvider;
        }

        public async Task CreateAsync(object content)
        {
            var context = ContextProvider.GetFor(content.GetType());

            var dbSet = context.GetDbSet(content.GetType());

            await dbSet.AddAsync(content).ConfigureAwait(false);

            await context.Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}