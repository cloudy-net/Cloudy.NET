using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IContentDeserializer
    {
        IContent Deserialize(Document document, ContentTypeDescriptor contentType, string language);
    }
}