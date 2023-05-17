using System;
using System.Collections.Generic;

namespace Cloudy.NET.InitializerSupport
{
    public interface IInitializerCreator
    {
        IEnumerable<IInitializer> Create();
    }
}