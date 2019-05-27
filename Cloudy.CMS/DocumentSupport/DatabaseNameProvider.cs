using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public class DatabaseNameProvider : IDatabaseNameProvider
    {
        public static string DatabaseName { get; set; } = "content";
        string IDatabaseNameProvider.DatabaseName => DatabaseName;
    }
}
