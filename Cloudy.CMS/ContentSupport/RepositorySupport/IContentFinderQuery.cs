using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentFinderQuery
    {
        IContentFinderQuery WithContentType(params string[] contentTypeIds);
        void WhereParent(string parent);
        void WhereHasNoParent();
        Task<IEnumerable<IContent>> GetResultAsync();
        IContentFinderQuery WhereEquals<T1, T2>(Expression<Func<T1, T2>> property, T2 value) where T1 : class;
    }
}