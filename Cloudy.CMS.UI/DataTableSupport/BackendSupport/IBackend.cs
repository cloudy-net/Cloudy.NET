using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.DataTableSupport.BackendSupport
{
    public interface IBackend
    {
        Result Load(Query query);
    }
}
