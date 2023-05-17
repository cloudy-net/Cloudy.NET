using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS
{
    public class AlreadyInitializedException : Exception
    {
        public AlreadyInitializedException() : base("Cloudy.CMS is already initialized") { }
    }
}
