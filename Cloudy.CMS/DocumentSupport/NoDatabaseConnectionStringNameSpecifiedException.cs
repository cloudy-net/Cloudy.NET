using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.DocumentSupport
{
    public class NoDatabaseConnectionStringNameSpecifiedException : Exception
    {
        public NoDatabaseConnectionStringNameSpecifiedException() : base($"No database connection string name specified. Please do services.AddCloudy(cloudy => cloudy.{nameof(CloudyConfigurator.WithDatabaseConnectionStringName)}(...))") { }
    }
}