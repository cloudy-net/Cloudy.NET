using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class UnknownParameterTypeException : Exception
    {
        public UnknownParameterTypeException(string uiHint, string parameter, string type, IEnumerable<string> supportedTypes) : base($"Unknown type {type} for parameter {parameter} in UIHint {uiHint}. Supported types are: {string.Join(", ", supportedTypes)}") { }
    }
}
