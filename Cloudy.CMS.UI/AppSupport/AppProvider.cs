using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.AppSupport
{
    public class AppProvider : IAppProvider
    {
        IEnumerable<AppDescriptor> Apps { get; }

        public AppProvider(IAppCreator appCreator)
        {
            Apps = appCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<AppDescriptor> GetAll()
        {
            return Apps;
        }
    }
}
