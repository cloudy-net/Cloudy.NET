using System;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContextProvider
    {
        IContextWrapper GetFor(Type instanceType);
    }
}