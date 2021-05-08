using System;

namespace Cloudy.CMS.ContentSupport.EntityFrameworkSupport
{
    public interface IContextProvider
    {
        IContextWrapper GetFor(Type type);
    }
}