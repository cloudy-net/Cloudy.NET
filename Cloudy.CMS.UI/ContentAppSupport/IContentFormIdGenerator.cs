using Cloudy.CMS.ContentTypeSupport;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    public interface IContentFormIdGenerator
    {
        string Generate(ContentTypeDescriptor contentType);
    }
}