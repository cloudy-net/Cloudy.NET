using MongoDB.Driver;

namespace Cloudy.CMS.DocumentSupport
{
    public interface IDatabaseProvider
    {
        IMongoDatabase Database { get; }
    }
}