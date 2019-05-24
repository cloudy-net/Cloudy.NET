using System;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentTypeSupport
{
    public interface IContentTypeProvider
    {
        ContentTypeDescriptor Get(Type type);
        ContentTypeDescriptor Get(string id);
        IEnumerable<ContentTypeDescriptor> GetAll();
    }
}