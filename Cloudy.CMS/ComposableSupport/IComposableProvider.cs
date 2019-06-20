using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComposableSupport
{
    public interface IComposableProvider
    {
        IEnumerable<T> GetAll<T>() where T : IComposable;
    }
}
