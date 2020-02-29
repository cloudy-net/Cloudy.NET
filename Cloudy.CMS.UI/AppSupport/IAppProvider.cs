using System.Collections.Generic;
using Cloudy.CMS.ComponentSupport;

namespace Cloudy.CMS.UI.AppSupport
{
    public interface IAppProvider
    {
        IEnumerable<AppDescriptor> GetAll();
        IEnumerable<AppDescriptor> GetFor(string componentId);
    }
}