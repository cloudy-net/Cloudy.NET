using System.Linq.Expressions;

namespace Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport
{
    public interface IExpressionParser
    {
        string Parse(Expression expression);
    }
}