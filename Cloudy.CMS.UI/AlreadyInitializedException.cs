using System;
using System.Runtime.Serialization;

namespace Poetry.UI
{
    public class AlreadyInitializedException : Exception
    {
        public AlreadyInitializedException() : base("Poetry UI is already initialized") { }
    }
}