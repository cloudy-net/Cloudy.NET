using System.Linq.Expressions;

namespace Cloudy.CMS.DocumentSupport
{
    public interface IDocumentPropertyPathProvider
    {
        string GetFor(Expression expression);
    }
}