using System;
using System.Collections.Generic;
using System.Text;
using Cloudy.CMS.ContentTypeSupport;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public class ContainerProvider : IContainerProvider
    {
        IMongoDatabase Database { get; }

        public ContainerProvider(IDatabaseProvider databaseProvider)
        {
            Database = databaseProvider.Database;
        }

        public IMongoCollection<Document> Get(string container)
        {
            return Database.GetCollection<Document>(container);
        }
    }
}
