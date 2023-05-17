using System;

namespace Cloudy.NET.ContextSupport
{
    public interface IContextCreator
    {
        IContextWrapper CreateFor(Type type);
    }
}