using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentTypeSupport.Name
{
    public interface IContentTypeNameCreator
    {
        IEnumerable<ContentTypeNameDescriptor> Create();
    }
}