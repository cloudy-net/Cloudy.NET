using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.NET
{
    public class AlreadyInitializedException : Exception
    {
        public AlreadyInitializedException() : base("Cloudy.NET is already initialized") { }
    }
}
