namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public interface IDocumentPropertyProvider
    {
        object GetFor(Document document, string path);
        bool Exists(Document document, string path);
    }
}