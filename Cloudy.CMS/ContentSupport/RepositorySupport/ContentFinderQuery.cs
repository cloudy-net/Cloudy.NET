using Cloudy.CMS.ContentSupport.RepositorySupport;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
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

        public async Task<IEnumerable<object>> GetResultAsync()
        {

            //typeof(Enumerable).GetMethod(nameof(Enumerable.Where)).Invoke(DbSet, Filters.)
            //IEnumerable<TSource> Where<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
            var task = (Task)typeof(EntityFrameworkQueryableExtensions).GetMethod(nameof(EntityFrameworkQueryableExtensions.ToListAsync)).MakeGenericMethod(DbSet.Type).Invoke(null, new object[] { DbSet.DbSet, null });

            await task.ConfigureAwait(false);

            return (IEnumerable<object>)task.GetType().GetProperty("Result").GetValue(task);
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
        }

        public void WhereParent(string parent)
        {
        }

        public IContentFinderQuery WithContentType(params string[] contentTypeIds)
        {
            throw new NotImplementedException();
        }
    }
}