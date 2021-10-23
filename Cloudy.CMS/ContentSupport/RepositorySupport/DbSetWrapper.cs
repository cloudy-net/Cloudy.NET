using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class DbSetWrapper : IDbSetWrapper
    {
        public object DbSet { get; }
        public Type Type { get; }

        public DbSetWrapper(object dbSet, Type type)
        {
            DbSet = dbSet;
            Type = type;
        }

        public async Task<object> FindAsync(params object[] keyValues)
        {
            var valueTask = DbSet.GetType().GetMethod(nameof(DbSet<object>.FindAsync), new[] { typeof(object[]) }).Invoke(DbSet, new object[] { keyValues });

            var task = (Task)valueTask.GetType().GetMethod(nameof(ValueTask<object>.AsTask)).Invoke(valueTask, new object[] { });

            await task.ConfigureAwait(false);

            return task.GetType().GetProperty("Result").GetValue(task);
        }

        public async Task AddAsync(object entity)
        {
            var genericValueTask = DbSet.GetType().GetMethod(nameof(DbSet<object>.AddAsync)).Invoke(DbSet, new object[] { entity, null });

            var task = (Task)genericValueTask.GetType().GetMethod(nameof(ValueTask<object>.AsTask)).Invoke(genericValueTask, new object[] { });

            await task.ConfigureAwait(false);
        }
    }
}