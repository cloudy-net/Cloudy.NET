using System.Linq.Expressions;

namespace Cloudy.CMS.DocumentSupport
{
    public interface IPropertyPathProvider
    {
        string GetFor(Expression expression);
    }
}