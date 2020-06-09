using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public interface IContentTypeGroupMatcher
    {
        IEnumerable<ContentTypeGroupDescriptor> GetContentTypeGroupsFor(string contentTypeId);
        IEnumerable<ContentTypeDescriptor> GetContentTypesFor(string contentTypeGroupId);
    }
}
