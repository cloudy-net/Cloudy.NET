using MongoDB.Driver;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public interface IDatabaseProvider
    {
        IMongoDatabase Database { get; }
    }
}