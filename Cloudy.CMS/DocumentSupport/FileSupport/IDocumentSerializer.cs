using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public interface IDocumentSerializer
    {
        string Serialize(Document document);
    }
}
