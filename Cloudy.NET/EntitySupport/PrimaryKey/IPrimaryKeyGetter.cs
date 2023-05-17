using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET.EntitySupport.PrimaryKey
{
    public interface IPrimaryKeyGetter
    {
        object[] Get(object content);
    }
}
