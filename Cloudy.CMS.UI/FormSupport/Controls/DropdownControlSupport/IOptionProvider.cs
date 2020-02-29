using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.Controls.DropdownControlSupport
{
    public interface IOptionProvider : IComposable
    {
        IEnumerable<Option> GetAll();
    }
}
