using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.EntitySupport.PrimaryKey
{
    public interface IPrimaryKeyGetter
    {
        object[] Get(object content);
    }
}
