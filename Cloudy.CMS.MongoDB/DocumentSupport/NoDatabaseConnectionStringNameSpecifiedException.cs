using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class NoDatabaseConnectionStringNameSpecifiedException : Exception
    {
        public NoDatabaseConnectionStringNameSpecifiedException() : base($"No database connection string name specified. Please do services.AddCloudy(cloudy => cloudy.{nameof(Cloudy.CMS.MongoDB.StartupExtensions.WithMongoDatabaseConnectionStringNamed)}(...))") { }
    }
}