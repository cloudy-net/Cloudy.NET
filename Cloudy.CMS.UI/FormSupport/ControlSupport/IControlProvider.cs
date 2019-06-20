using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport
{
    public interface IControlProvider
    {
        IEnumerable<ControlDescriptor> GetAll();
    }
}
