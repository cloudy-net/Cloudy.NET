using Cloudy.CMS.ComposableSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.FieldSupport
{
    public interface IUIHintReplacer : IComposable
    {
        string Replace(string value);
    }
}
