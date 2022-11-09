using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.EntitySupport.PrimaryKey
{
    public interface IPrimaryKeySetter
    {
        void Set(IEnumerable<object> keyValues, object content);
    }
}
