using Cloudy.CMS.ComponentSupport;
using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public class BackendProvider : IBackendProvider
    {
        IDictionary<string, IBackend> Backends { get; }

        public BackendProvider(IComposableProvider composableProvider, IBackendCreator backendCreator)
        {
            Backends = composableProvider.GetAll<IBackendCreator>().SelectMany(c => c.CreateAll()).ToDictionary(p => p.Key, p => p.Value);
        }

        public IBackend GetFor(string id)
        {
            if (!Backends.ContainsKey(id))
            {
                return null;
            }

            return Backends[id];
        }
    }
}
