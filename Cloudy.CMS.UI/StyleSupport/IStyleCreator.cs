using Poetry.ComponentSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.StyleSupport
{
    public interface IStyleCreator
    {
        IEnumerable<StyleDescriptor> Create(ComponentDescriptor componentDescriptor);
    }
}
