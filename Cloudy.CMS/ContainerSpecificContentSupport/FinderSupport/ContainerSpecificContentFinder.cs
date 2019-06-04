using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport
{
    public class ContainerSpecificContentFinder : IContainerSpecificContentFinder
    {
        IExpressionParser ExpressionParser { get; }

        public ContainerSpecificContentFinder(IExpressionParser expressionParser)
        {
            ExpressionParser = expressionParser;
        }

        public QueryBuilder<T> Find<T>(string container) where T : class
        {
            return new QueryBuilder<T>(ExpressionParser);
        }
    }
}
