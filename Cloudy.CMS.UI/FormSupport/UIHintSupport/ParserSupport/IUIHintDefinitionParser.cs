using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport.UIHintSupport.ParserSupport
{
    public interface IUIHintDefinitionParser
    {
        UIHintDefinition Parse(string value);
    }
}
