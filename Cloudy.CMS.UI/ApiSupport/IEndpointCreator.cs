using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.ApiSupport
{
    public interface IEndpointCreator
    {
        IEnumerable<Endpoint> Create(Type apiType);
    }
}
