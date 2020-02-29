using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ComponentSupport
{
    public interface IComponentTypeProvider
    {
        IEnumerable<Type> GetAll();
    }
}
