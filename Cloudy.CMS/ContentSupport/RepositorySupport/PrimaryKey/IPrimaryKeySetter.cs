using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey
{
    public interface IPrimaryKeySetter
    {
        void Set(IEnumerable<object> keyValues, object content);
    }
}
