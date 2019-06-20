using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Poetry.UI.FormSupport.ControlSupport
{
    public class ControlProvider : IControlProvider
    {
        IEnumerable<ControlDescriptor> Controls { get; }
        
        public ControlProvider(IControlCreator controlCreator)
        {
            Controls = controlCreator.Create().ToList().AsReadOnly();
        }

        public IEnumerable<ControlDescriptor> GetAll()
        {
            return Controls;
        }
    }
}
