using System;

namespace Cloudy.CMS.ContentTypeSupport.Name
{
    public interface IContentTypeNameProvider
    {
        ContentTypeNameDescriptor Get(Type type);
    }
}