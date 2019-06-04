using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport
{
    public class ExpressionParser : IExpressionParser
    {
        public string Parse(Expression expression)
        {
            var lambdaExpression = expression as LambdaExpression;

            if (lambdaExpression == null)
            {
                throw new ArgumentException("Expression was not of type LambdaExpression (should be something like: c => c.Property)", nameof(expression));
            }

            var memberExpression = lambdaExpression.Body as MemberExpression;

            if (memberExpression == null)
            {
                var body = lambdaExpression.Body as UnaryExpression;
                memberExpression = body?.Operand as MemberExpression;
            }

            if(memberExpression == null)
            {
                throw new ArgumentException("Expression not supported", nameof(expression));
            }

            return memberExpression.Member.Name;
        }
    }
}
