using System.Collections.Generic;

namespace Cloudy.CMS.UI.AppSupport
{
    public interface IAppProvider
    {
        IEnumerable<AppDescriptor> GetAll();
    }
}