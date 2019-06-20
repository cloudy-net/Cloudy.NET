using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    public interface IApiProvider
    {
        IEnumerable<Api> GetAllFor(ComponentDescriptor component);
    }
}
