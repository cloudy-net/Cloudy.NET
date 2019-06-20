using System;
using System.Collections.Generic;

namespace Poetry.InitializerSupport
{
    public interface IInitializerCreator
    {
        IEnumerable<IInitializer> Create();
    }
}