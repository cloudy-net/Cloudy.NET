using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public interface IUIHintParser
    {
        UIHint Parse(string value);
    }
}
