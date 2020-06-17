using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.CacheSupport
{
    public interface IDocumentLister
    {
        Task<IEnumerable<Document>> ListAsync(string container);
    }
}