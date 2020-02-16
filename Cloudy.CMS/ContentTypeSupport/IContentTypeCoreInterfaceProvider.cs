using System.Collections.Generic;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface IContentTypeCoreInterfaceProvider
    {
        IEnumerable<CoreInterfaceDescriptor> GetFor(string contentTypeId);
    }
}