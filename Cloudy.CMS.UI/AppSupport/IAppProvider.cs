using System.Collections.Generic;
using Poetry.ComponentSupport;

namespace Poetry.UI.AppSupport
{
    public interface IAppProvider
    {
        IEnumerable<AppDescriptor> GetAll();
        IEnumerable<AppDescriptor> GetFor(string componentId);
    }
}