using System;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IContextWrapper
    {
        IDbSetWrapper GetDbSet(Type type);
    }
}