using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IPrimaryKeySetter
    {
        void Set(IEnumerable<object> keyValues, object content);
    }
}
