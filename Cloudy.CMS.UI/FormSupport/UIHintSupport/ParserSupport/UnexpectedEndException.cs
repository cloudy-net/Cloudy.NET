using System;
using System.Runtime.Serialization;

namespace Poetry.UI.FormSupport.UIHintSupport.ParserSupport
{
    public class UnexpectedEndException : Exception
    {
        public UnexpectedEndException(string data, int position) : base($"UIHint {data} was not expected to end at position {position}: {data.PadRight(position, ' ').Insert(position, "↓")}") { }
    }
}