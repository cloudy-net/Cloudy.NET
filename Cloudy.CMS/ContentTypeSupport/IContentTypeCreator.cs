using System.Collections.Generic;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface IContentTypeCreator
    {
        IEnumerable<ContentTypeDescriptor> Create();
    }
}