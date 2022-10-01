using System;

namespace Cloudy.CMS.ContentTypeSupport.Name
{
    public interface IContentTypeNameProvider
    {
        ContentTypeName Get(Type type);
    }
}