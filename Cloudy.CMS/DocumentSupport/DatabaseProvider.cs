using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using MongoDB.Integrations.JsonDotNet;

namespace Cloudy.CMS.DocumentSupport
{
    public class DatabaseProvider : IDatabaseProvider
    {
        IMongoClient MongoClient { get; }

        public DatabaseProvider(IDatabaseConnectionStringNameProvider databaseConnectionStringNameProvider, IConfiguration configuration)
        {
            BsonSerializer.RegisterSerializer(typeof(Document), new JsonSerializerAdapter<Document>());

            var url = new MongoUrl(configuration.GetConnectionString(databaseConnectionStringNameProvider.DatabaseConnectionStringName));

            MongoClient = new MongoClient(url);

            Database = MongoClient.GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase Database { get; }
    }
}
