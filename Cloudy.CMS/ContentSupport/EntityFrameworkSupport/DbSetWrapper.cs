using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public class DbSetWrapper : IDbSetWrapper
    {
        public object DbSet { get; }

        public DbSetWrapper(object dbSet)
        {
            DbSet = dbSet;
        }

        public async Task<object> FindAsync(params object[] keyValues)
        {
            var task = (Task)DbSet.GetType().GetMethod(nameof(DbSet<object>.FindAsync)).Invoke(DbSet, new object[] { keyValues });

            await task.ConfigureAwait(false);

            return task.GetType().GetProperty("Result").GetValue(task);
        }

        public async Task AddAsync(object entity)
        {
            var task = (Task)DbSet.GetType().GetMethod(nameof(DbSet<object>.AddAsync)).Invoke(DbSet, new object[] { entity });

            await task.ConfigureAwait(false);
        }
    }
}