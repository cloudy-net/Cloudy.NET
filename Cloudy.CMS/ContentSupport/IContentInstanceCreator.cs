using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport
{
    public interface IContentInstanceCreator
    {
        object Create(ContentTypeDescriptor contentType);
    }
}
