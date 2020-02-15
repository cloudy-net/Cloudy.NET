using System.Collections.Generic;

namespace Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport
{
    public interface IContentTypeActionModuleCreator
    {
        IEnumerable<ContentTypeActionModuleDescriptor> Create();
    }
}