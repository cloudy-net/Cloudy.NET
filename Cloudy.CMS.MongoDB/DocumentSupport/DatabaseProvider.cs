using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Integrations.JsonDotNet;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class DatabaseProvider : IDatabaseProvider
    {
        IMongoClient MongoClient { get; }

        static DatabaseProvider()
        {
            BsonSerializer.RegisterSerializer(typeof(Document), new JsonSerializerAdapter<Document>());
        }

        public DatabaseProvider(IDatabaseConnectionStringNameProvider databaseConnectionStringNameProvider, IConfiguration configuration)
        {
            var url = new MongoUrl(configuration.GetConnectionString(databaseConnectionStringNameProvider.DatabaseConnectionStringName));

            MongoClient = new MongoClient(url);

            Database = MongoClient.GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase Database { get; }
    }
}
