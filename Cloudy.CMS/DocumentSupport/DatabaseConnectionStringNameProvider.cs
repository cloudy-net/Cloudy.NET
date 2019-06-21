using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public class DatabaseConnectionStringNameProvider : IDatabaseConnectionStringNameProvider
    {
        public string DatabaseConnectionStringName { get; }

        public DatabaseConnectionStringNameProvider(string name)
        {
            DatabaseConnectionStringName = name;
        }
    }
}
