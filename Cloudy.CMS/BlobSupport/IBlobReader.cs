using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cloudy.CMS.BlobSupport
{
    public interface IBlobReader
    {
        void Read(string id, Action<Stream> readAction);
    }
}
