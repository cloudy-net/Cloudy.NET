using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Driver;

namespace Cloudy.CMS.DocumentSupport
{
    public class NoDatabaseProvider : IDatabaseProvider
    {
        public IMongoDatabase Database => throw new NoDatabaseSpecifiedException();
    }
}
