using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public interface IBackendCreator : IComposable
    {
        IDictionary<string, IBackend> CreateAll();
    }
}