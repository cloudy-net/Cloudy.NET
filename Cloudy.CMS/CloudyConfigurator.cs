using System;

namespace Cloudy.CMS
{
    public class CloudyConfigurator
    {
        CloudyOptions Options { get; }

        public CloudyConfigurator(CloudyOptions options)
        {
            Options = options;
        }

        public CloudyConfigurator WithDatabaseConnectionStringName(string name)
        {
            if (name.Contains(":") || name.Contains("/"))
            {
                throw new ArgumentException("Connection strings have to be referenced by name from your appsettings.json. No direct URLs here. You'll thank me later!");
            }

            Options.DatabaseConnectionString = name;

            return this;
        }

        public CloudyConfigurator AddComponent<T>() where T : class
        {
            Options.Components.Add(typeof(T));

            return this;
        }
    }
}