using System;
using System.Collections.Generic;

namespace Cloudy.CMS.InitializerSupport
{
    public interface IInitializerCreator
    {
        IEnumerable<IInitializer> Create();
    }
}