using Cloudy.CMS.ContentTypeSupport;
using System;

namespace Cloudy.CMS.ContentControllerSupport
{
    public interface IContentControllerFinder
    {
        IContentControllerMatch FindController(ContentTypeDescriptor contentType);
    }
}