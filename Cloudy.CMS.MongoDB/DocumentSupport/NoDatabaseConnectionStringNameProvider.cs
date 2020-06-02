using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class NoDatabaseConnectionStringNameProvider : IDatabaseConnectionStringNameProvider
    {
        public string DatabaseConnectionStringName => throw new NoDatabaseConnectionStringNameSpecifiedException();
    }
}
