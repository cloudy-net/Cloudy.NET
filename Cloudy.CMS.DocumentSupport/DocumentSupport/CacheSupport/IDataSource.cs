using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.CacheSupport
{
    public interface IDataSource
    {
        Task CreateDocumentAsync(string container, Document document);
        Task UpdateAsync(string container, string id, Document document);
        Task DeleteAsync(string container, string id);
        Task<IEnumerable<Document>> ListAsync(string container);
    }
}