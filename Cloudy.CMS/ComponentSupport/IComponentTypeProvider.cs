using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.ComponentSupport
{
    public interface IComponentTypeProvider
    {
        IEnumerable<Type> GetAll();
    }
}
