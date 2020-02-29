using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public interface IUIHintDefinitionParser
    {
        UIHintDefinition Parse(string value);
    }
}
