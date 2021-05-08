using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport
{
    public interface IPrimaryKeyConverter
    {
        object[] Convert(IEnumerable<object> keyValues, string contentTypeId);
    }
}
