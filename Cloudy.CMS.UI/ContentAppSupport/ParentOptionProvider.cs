using MongoDB.Driver;
using Cloudy.CMS.DocumentSupport;
using Poetry.UI.FormSupport.Controls.DropdownControlSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [OptionProvider("parent")]
    public class ParentOptionProvider : IOptionProvider
    {
        IDocumentRepository DocumentRepository { get; }

        public ParentOptionProvider(IDocumentRepository documentRepository)
        {
            DocumentRepository = documentRepository;
        }

        public IEnumerable<Option> GetAll()
        {
            var documents = DocumentRepository.Documents.FindSync(Builders<Document>.Filter.Exists(d => d.GlobalFacet.Interfaces["IHierarchical"].Properties["ParentId"]), new FindOptions<Document, Document> { Projection = Builders<Document>.Projection.Include(d => d.GlobalFacet.Interfaces["INameable"].Properties["Name"]) });

            var result = new List<Option>();

            result.Add(new Option("(root)", null));

            result.AddRange(documents.ToList().Select(d => new Option(d.GlobalFacet.Interfaces["INameable"].Properties["Name"] as string ?? d.Id, d.Id)));

            return result.AsReadOnly();
        }
    }
}
