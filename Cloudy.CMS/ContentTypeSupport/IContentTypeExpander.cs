using System.Collections;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface IContentTypeExpander
    {
        IEnumerable<ContentTypeDescriptor> Expand(string typeOrGroupIdOrTypeName);
    }
}