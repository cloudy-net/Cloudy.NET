using Poetry.ComponentSupport;
using System.Collections.Generic;

namespace Poetry.UI.AppSupport
{
    public interface IAppCreator
    {
        IEnumerable<AppDescriptor> Create();
    }
}