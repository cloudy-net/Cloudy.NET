using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentCreator : IDocumentCreator
    {
        IFilePathProvider FilePathProvider { get; }
        IFileHandler FileHandler { get; }
        IDocumentSerializer DocumentSerializer { get; }

        public DocumentCreator(IFilePathProvider filePathProvider, IFileHandler fileHandler, IDocumentSerializer documentSerializer)
        {
            FilePathProvider = filePathProvider;
            FileHandler = fileHandler;
            DocumentSerializer = documentSerializer;
        }

        public Task Create(string container, Document document)
        {
            var path = FilePathProvider.GetPathFor(container, document.Id);

            if (FileHandler.Exists(path))
            {
                throw new DocumentAlreadyExistsException(container, document.Id);
            }

            FileHandler.Create(path, DocumentSerializer.Serialize(document));

            return Task.CompletedTask;
        }
    }
}