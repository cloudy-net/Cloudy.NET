using Microsoft.EntityFrameworkCore;
using System;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContextWrapper
    {
        DbContext Context { get; }
        IDbSetWrapper GetDbSet(Type type);
    }
}