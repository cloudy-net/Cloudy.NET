using System.Collections;
using System.Collections.Generic;

namespace Cloudy.NET.InitializerSupport
{
    public interface IInitializerProvider
    {
        IEnumerable<IInitializer> GetAll();
    }
}