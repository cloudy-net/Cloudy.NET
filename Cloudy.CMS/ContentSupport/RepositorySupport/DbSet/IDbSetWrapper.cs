using System;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.DbSet
{
    public interface IDbSetWrapper
    {
        object DbSet { get; }
        Type Type { get; }
        Task<object> FindAsync(object[] keyValues);
        Task AddAsync(object entity);
        void Remove(object entity);
    }
}