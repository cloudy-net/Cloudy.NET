using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.DbSet
{
    public interface IDbSetProvider
    {
        IDbSetWrapper Get(Type type);
    }
}
