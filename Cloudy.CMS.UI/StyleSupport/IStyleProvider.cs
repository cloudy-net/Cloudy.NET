using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.StyleSupport
{
    public interface IStyleProvider
    {
        IEnumerable<StyleDescriptor> GetAllFor(ComponentDescriptor componentDescriptor);
    }
}
