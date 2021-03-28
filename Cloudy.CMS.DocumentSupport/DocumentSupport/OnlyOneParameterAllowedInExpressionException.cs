using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public class OnlyOneParameterAllowedInExpressionException : Exception
    {
        public OnlyOneParameterAllowedInExpressionException(IEnumerable<ParameterExpression> parameters) : base($"Only one parameter is allowed in expression (like d => ...). You had: " + (parameters.Any() ? string.Join(", ", parameters.Select(p => $"({p.Type}) {p.Name}")) : "(none)")) { }
    }
}
