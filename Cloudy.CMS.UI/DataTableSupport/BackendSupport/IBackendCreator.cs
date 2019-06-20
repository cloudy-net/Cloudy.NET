using Poetry.ComponentSupport;
using Poetry.ComposableSupport;
using System.Collections.Generic;

namespace Poetry.UI.DataTableSupport.BackendSupport
{
    public interface IBackendCreator : IComposable
    {
        IDictionary<string, IBackend> CreateAll();
    }
}