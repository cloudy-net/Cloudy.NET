using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentTypeSupport.GroupSupport
{
    public interface IContentTypeGroupProvider
    {
        IEnumerable<ContentTypeGroupDescriptor> GetAll();
        ContentTypeGroupDescriptor Get(string contentTypeGroupId);
    }
}
