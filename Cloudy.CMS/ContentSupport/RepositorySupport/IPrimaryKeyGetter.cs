using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IPrimaryKeyGetter
    {
        object[] Get(object content);
    }
}
