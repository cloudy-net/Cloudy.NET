using System;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.Context
{
    public interface IContextProvider
    {
        IContextWrapper GetFor(Type instanceType);
    }
}