using Cloudy.CMS.ComposableSupport;
using Cloudy.CMS.ContentTypeSupport;

namespace Cloudy.CMS.ContentSupport
{
    public interface IContentInstanceInitializer : IComposable
    {
        void Initialize(object content, ContentTypeDescriptor contentType);
    }
}