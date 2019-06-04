using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Cloudy.CMS.ContainerSpecificContentSupport.FinderSupport
{
    public class QueryBuilder<T> where T : class
    {
        IExpressionParser ExpressionParser { get; }

        List<IFilter> Filters { get; } = new List<IFilter>();

        public QueryBuilder(IExpressionParser expressionParser)
        {
            ExpressionParser = expressionParser;
        }

        public FilterPrelude<T, string> Where(Expression<Func<T, string>> expression)
        {
            var property = ExpressionParser.Parse(expression);

            return new FilterPrelude<T, string>(this, property);
        }

        public void AddFilter(IFilter filter)
        {
            Filters.Add(filter);
        }

        //public async T FirstOrDefaultAsync<T>()
        //{
        //    Client.Execute(new FindOperation(Filters));
        //}
    }
}