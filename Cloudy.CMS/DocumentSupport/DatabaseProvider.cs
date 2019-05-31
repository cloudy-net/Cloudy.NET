using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Cloudy.CMS.DocumentSupport
{
    public class DatabaseProvider : IDatabaseProvider
    {
        static DatabaseProvider()
        {
            BsonClassMap.RegisterClassMap<Document>(cm => { cm.AutoMap(); new ImmutableTypeClassMapConvention().Apply(cm); });
            BsonClassMap.RegisterClassMap<DocumentFacet>(cm => { cm.AutoMap(); new ImmutableTypeClassMapConvention().Apply(cm); });
            BsonClassMap.RegisterClassMap<DocumentInterface>(cm => { cm.AutoMap(); new ImmutableTypeClassMapConvention().Apply(cm); });
        }

        IMongoClient MongoClient { get; }

        public DatabaseProvider(string connectionString)
        {
            var url = new MongoUrl(connectionString);

            MongoClient = new MongoClient(url);

            Database = MongoClient.GetDatabase(url.DatabaseName);
        }

        public IMongoDatabase Database { get; }
    }
}
