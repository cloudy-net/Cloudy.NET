using System;
using System.Runtime.Serialization;

namespace Cloudy.CMS.DocumentSupport
{
    [Serializable]
    internal class NoDatabaseSpecifiedException : Exception
    {
        public NoDatabaseSpecifiedException() : base($"No database specified. Please do app.AddCloudy(cloudy => cloudy.{nameof(CloudyConfigurator.WithDatabaseConnectionString)}(...))") { }
    }
}