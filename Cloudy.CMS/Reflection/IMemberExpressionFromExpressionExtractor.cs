using System.Linq.Expressions;

namespace Cloudy.CMS.Reflection
{
    public interface IMemberExpressionFromExpressionExtractor
    {
        MemberExpression ExtractMemberExpressionFromExpression(Expression expression);
    }
}