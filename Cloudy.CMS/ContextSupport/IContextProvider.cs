using System;

namespace Cloudy.CMS.ContextSupport
{
    public interface IContextProvider
    {
        IContextWrapper GetFor(Type instanceType);
    }
}