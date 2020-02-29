using System.Collections;
using System.Collections.Generic;

namespace Cloudy.CMS.InitializerSupport
{
    public interface IInitializerProvider
    {
        IEnumerable<IInitializer> GetAll();
    }
}