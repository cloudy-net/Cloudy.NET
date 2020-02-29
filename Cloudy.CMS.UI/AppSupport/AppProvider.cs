using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudy.CMS.ComponentSupport;

namespace Cloudy.CMS.UI.AppSupport
{
    public class AppProvider : IAppProvider
    {
        IEnumerable<AppDescriptor> Apps { get; }
        IDictionary<string, IEnumerable<AppDescriptor>> AppsGroupedByComponentId { get; }

        public AppProvider(IAppCreator appCreator)
        {
            Apps = appCreator.Create().ToList().AsReadOnly();
            AppsGroupedByComponentId = Apps.GroupBy(a => a.ComponentId).ToDictionary(g => g.Key, g => (IEnumerable<AppDescriptor>)g.ToList().AsReadOnly());
        }

        public IEnumerable<AppDescriptor> GetAll()
        {
            return Apps;
        }

        public IEnumerable<AppDescriptor> GetFor(string componentId)
        {
            if(!AppsGroupedByComponentId.ContainsKey(componentId))
            {
                return null;
            }

            return AppsGroupedByComponentId[componentId];
        }
    }
}
