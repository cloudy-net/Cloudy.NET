namespace Cloudy.CMS
{
    public class CloudyConfigurator
    {
        CloudyOptions Options { get; }

        public CloudyConfigurator(CloudyOptions options)
        {
            Options = options;
        }

        public CloudyConfigurator WithDatabaseConnectionString(string databaseConnectionString)
        {
            Options.DatabaseConnectionString = databaseConnectionString;

            return this;
        }

        public CloudyConfigurator AddComponent<T>() where T : class
        {
            Options.Components.Add(typeof(T));

            return this;
        }
    }
}