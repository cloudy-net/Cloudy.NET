using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Methods
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