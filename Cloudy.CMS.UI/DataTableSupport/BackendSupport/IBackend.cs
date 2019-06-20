using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.DataTableSupport.BackendSupport
{
    public interface IBackend
    {
        Result Load(Query query);
    }
}
