using System.Collections.Generic;

namespace Cloudy.CMS.UI.FormSupport.ControlSupport
{
    public interface IControlCreator
    {
        IEnumerable<ControlDescriptor> Create();
    }
}