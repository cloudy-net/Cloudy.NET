using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public interface IDocumentDeserializer
    {
        Document Deserialize(string contents);
    }
}
