using Newtonsoft.Json.Linq;
using Cloudy.CMS.DocumentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.ContentSupport.Serialization
{
    public interface IContentSerializer
    {
        Document Serialize(IContent content, ContentTypeDescriptor contentType);
    }
}
