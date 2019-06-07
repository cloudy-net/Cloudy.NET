using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cloudy.CMS.BlobSupport
{
    public interface IBlobWriter
    {
        Stream Write(string container, string id);
    }
}
