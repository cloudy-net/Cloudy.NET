using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.DocumentSupport
{
    public interface IDocumentRepository
    {
        IMongoCollection<Document> Documents { get; }
    }
}
