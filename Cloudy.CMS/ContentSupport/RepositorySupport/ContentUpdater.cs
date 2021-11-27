using Cloudy.CMS.ContentSupport.RepositorySupport;
using Microsoft.EntityFrameworkCore;
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
            
            context.Context.Entry(content).State = EntityState.Modified;
            
            await context.Context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}