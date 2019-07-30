using System.Threading.Tasks;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentDeleter : IDocumentDeleter
    {
        IFilePathProvider FilePathProvider { get; }
        IFileHandler FileHandler { get; }

        public DocumentDeleter(IFilePathProvider filePathProvider, IFileHandler fileHandler)
        {
            FilePathProvider = filePathProvider;
            FileHandler = fileHandler;
        }

        public Task DeleteAsync(string container, string id)
        {
            var path = FilePathProvider.GetPathFor(container, id);

            if (!FileHandler.Exists(path))
            {
                throw new DocumentDoesNotExistException(container, id);
            }

            FileHandler.Delete(id);

            return Task.CompletedTask;
        }
    }
}