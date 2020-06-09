using System.Collections.Generic;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public interface IContentTypeGroupCreator
    {
        IEnumerable<ContentTypeGroupDescriptor> Create();
    }
}