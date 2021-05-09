using Cloudy.CMS.ContentSupport.RepositorySupport;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class ContentFinderQuery : IContentFinderQuery
    {
        IDbSetWrapper DbSet { get; }
        Type Type { get; }
        IList<LambdaExpression> Filters { get; } = new List<LambdaExpression>();

        public ContentFinderQuery(IDbSetWrapper dbSet, Type type)
        {
            DbSet = dbSet;
            Type = type;
        }

        public Task<IEnumerable<object>> GetResultAsync()
        {
            throw new NotImplementedException();
        }

        public IContentFinderQuery WhereEquals<T1, T2>(Expression<Func<T1, T2>> property, T2 value) where T1 : class
        {
            //var parameter = Expression.Parameter(typeof(TData));
            //var expressionParameter = Expression.Property(parameter, GetParameterName(selector));

            //var body = Expression.GreaterThan(expressionParameter, Expression.Constant(valueToCompare, typeof(TKey)));
            // Expression.Lambda(body, parameter);

            return this;
        }

        public void WhereHasNoParent()
        {
            throw new NotImplementedException();
        }

        public void WhereParent(string parent)
        {
            throw new NotImplementedException();
        }

        public IContentFinderQuery WithContentType(params string[] contentTypeIds)
        {
            throw new NotImplementedException();
        }
    }
}