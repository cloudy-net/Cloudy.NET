using System.Collections.Generic;

namespace Cloudy.CMS.UI.ContentAppSupport.ListActionSupport
{
    public interface IListActionModuleCreator
    {
        IEnumerable<ListActionModuleDescriptor> Create();
    }
}