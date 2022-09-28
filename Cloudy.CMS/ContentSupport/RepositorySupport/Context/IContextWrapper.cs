using Cloudy.CMS.ContentSupport.RepositorySupport.DbSet;
using Microsoft.EntityFrameworkCore;
using System;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
{
    public interface IContextWrapper
    {
        DbContext Context { get; }
        IDbSetWrapper GetDbSet(Type type);
    }
}