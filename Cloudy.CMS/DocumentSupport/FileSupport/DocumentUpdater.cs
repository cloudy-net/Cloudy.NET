using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentUpdater : IDocumentUpdater
    {
        IFilePathProvider FilePathProvider { get; }
        IFileHandler FileHandler { get; }
        IDocumentSerializer DocumentSerializer { get; }

        public DocumentUpdater(IFilePathProvider filePathProvider, IFileHandler fileHandler, IDocumentSerializer documentSerializer)
        {
            FilePathProvider = filePathProvider;
            FileHandler = fileHandler;
            DocumentSerializer = documentSerializer;
        }

        public Task UpdateAsync(string container, string id, Document document)
        {
            var path = FilePathProvider.GetPathFor(container, id);

            if (!FileHandler.Exists(path))
            {
                throw new DocumentDoesNotExistException(container, id);
            }

            FileHandler.Update(path, DocumentSerializer.Serialize(document));

            return Task.CompletedTask;
        }
    }
}