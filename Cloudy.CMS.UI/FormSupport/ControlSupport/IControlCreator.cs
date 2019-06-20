using System.Collections.Generic;

namespace Poetry.UI.FormSupport.ControlSupport
{
    public interface IControlCreator
    {
        IEnumerable<ControlDescriptor> Create();
    }
}