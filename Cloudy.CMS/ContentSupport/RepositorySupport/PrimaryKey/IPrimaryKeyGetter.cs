using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.PrimaryKey
{
    public interface IPrimaryKeyGetter
    {
        object[] Get(object content);
    }
}
