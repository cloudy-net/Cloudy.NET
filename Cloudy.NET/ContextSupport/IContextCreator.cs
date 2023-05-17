using System;

namespace Cloudy.CMS.ContextSupport
{
    public interface IContextCreator
    {
        IContextWrapper CreateFor(Type type);
    }
}