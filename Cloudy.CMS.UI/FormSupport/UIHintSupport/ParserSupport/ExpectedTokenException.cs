using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class ExpectedTokenException : Exception
    {
        public ExpectedTokenException(string data, int position, char[] tokens) : base($"Expected UIHint {data} to contain token{(tokens.Length > 1 ? "s" : null)} {string.Join(" or ", tokens)} at position {position}: {data.Insert(position, "↓")}") { }
    }
}