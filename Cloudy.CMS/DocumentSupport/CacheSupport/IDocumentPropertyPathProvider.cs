using System.Linq.Expressions;

namespace Cloudy.CMS.DocumentSupport.CacheSupport
{
    public interface IDocumentPropertyPathProvider
    {
        string GetFor(Expression expression);
    }
}