using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.BlobSupport
{
    public interface IBlobDeleter
    {
        void Delete(string container, string id);
    }
}
