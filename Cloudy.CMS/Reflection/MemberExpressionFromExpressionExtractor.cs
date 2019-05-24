using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.Reflection
{
    public class MemberExpressionFromExpressionExtractor : IMemberExpressionFromExpressionExtractor
    {
        public MemberExpression ExtractMemberExpressionFromExpression(Expression expression)
        {
            var lambdaExpression = expression as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new Exception("Expression was not of type LambdaExpression (should be something like: c => c.Property)");
            }

            var memberExpression = lambdaExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                var body = (UnaryExpression)lambdaExpression.Body;
                memberExpression = body.Operand as MemberExpression;
            }

            return memberExpression;
        }
    }
}
