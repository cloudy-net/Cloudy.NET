using Cloudy.CMS.ComponentSupport;
using System.Collections.Generic;

namespace Cloudy.CMS.UI.AppSupport
{
    public interface IAppCreator
    {
        IEnumerable<AppDescriptor> Create();
    }
}