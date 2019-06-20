using Poetry.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.Controls.DropdownControlSupport
{
    public interface IOptionProvider : IComposable
    {
        IEnumerable<Option> GetAll();
    }
}
