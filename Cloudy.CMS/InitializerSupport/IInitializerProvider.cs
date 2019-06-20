using System.Collections;
using System.Collections.Generic;

namespace Poetry.InitializerSupport
{
    public interface IInitializerProvider
    {
        IEnumerable<IInitializer> GetAll();
    }
}