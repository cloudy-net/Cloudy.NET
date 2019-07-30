namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class FileBasedDocumentOptions : IFileBasedDocumentOptions
    {
        public string Path { get; }

        public FileBasedDocumentOptions(string path)
        {
            Path = path;
        }
    }
}