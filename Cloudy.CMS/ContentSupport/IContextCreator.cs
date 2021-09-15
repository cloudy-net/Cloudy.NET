using System;

namespace Cloudy.CMS.ContentSupport
{
    public interface IContextCreator
    {
        IContextWrapper CreateFor(Type type);
    }
}