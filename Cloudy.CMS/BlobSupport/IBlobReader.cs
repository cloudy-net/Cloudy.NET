using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cloudy.CMS.BlobSupport
{
    public interface IBlobReader
    {
        Stream Read(string container, string id);
    }
}
