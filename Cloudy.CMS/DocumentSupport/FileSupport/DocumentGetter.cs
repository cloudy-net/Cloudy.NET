using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentGetter : IDocumentGetter
    {
        IFilePathProvider FilePathProvider { get; }
        IFileHandler FileHandler { get; }
        IDocumentDeserializer DocumentDeserializer { get; }

        public DocumentGetter(IFilePathProvider filePathProvider, IFileHandler fileHandler, IDocumentDeserializer documentDeserializer)
        {
            FilePathProvider = filePathProvider;
            FileHandler = fileHandler;
            DocumentDeserializer = documentDeserializer;
        }

        public Task<Document> GetAsync(string container, string id)
        {
            var path = FilePathProvider.GetPathFor(container, id);

            if (!FileHandler.Exists(path))
            {
                throw new DocumentDoesNotExistException(container, id);
            }

            string contents = FileHandler.Get(id);

            return Task.FromResult(DocumentDeserializer.Deserialize(contents));
        }
    }
}