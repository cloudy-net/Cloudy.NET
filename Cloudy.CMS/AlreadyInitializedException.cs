using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry
{
    public class AlreadyInitializedException : Exception
    {
        public AlreadyInitializedException() : base("Poetry is already initialized") { }
    }
}
