using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport
{
    public interface IPrimaryKeyGetter
    {
        object[] Get(object content);
    }
}
