using System;

namespace Cloudy.CMS.ContentSupport
{
    public interface IContextProvider
    {
        IContextWrapper GetFor(Type instanceType);
    }
}