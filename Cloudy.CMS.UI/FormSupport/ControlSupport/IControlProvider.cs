using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public interface IControlProvider
    {
        IEnumerable<ControlDescriptor> GetAll();
    }
}
