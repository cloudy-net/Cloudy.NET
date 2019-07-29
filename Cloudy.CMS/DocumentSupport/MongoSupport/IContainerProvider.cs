using Cloudy.CMS.ContentSupport;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport.MongoSupport
{
    public interface IContainerProvider
    {
        IMongoCollection<Document> Get(string container);
    }
}
