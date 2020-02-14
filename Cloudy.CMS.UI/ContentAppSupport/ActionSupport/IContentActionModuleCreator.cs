using System.Collections.Generic;

namespace Cloudy.CMS.UI.ContentAppSupport.ActionSupport
{
    public interface IContentActionModuleCreator
    {
        IEnumerable<ContentActionModuleDescriptor> Create();
    }
}