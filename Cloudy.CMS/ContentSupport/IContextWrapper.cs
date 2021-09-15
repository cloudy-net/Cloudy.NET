using Microsoft.EntityFrameworkCore;
using System;

namespace Cloudy.CMS.ContentSupport
{
    public interface IContextWrapper
    {
        DbContext Context { get; }
        IDbSetWrapper GetDbSet(Type type);
    }
}