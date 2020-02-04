namespace Cloudy.CMS.DocumentSupport
{
    public interface IDocumentPropertyFinder
    {
        object GetFor(Document document, string path);
        bool Exists(Document document, string path);
    }
}