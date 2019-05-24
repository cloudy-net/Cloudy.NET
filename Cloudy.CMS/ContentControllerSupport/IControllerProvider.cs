using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentControllerSupport
{
    public interface IControllerProvider
    {
        IEnumerable<Type> GetAll();
    }
}
