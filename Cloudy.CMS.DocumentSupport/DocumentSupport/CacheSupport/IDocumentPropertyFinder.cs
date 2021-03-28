namespace Cloudy.CMS.DocumentSupport.CacheSupport
{
    public interface IDocumentPropertyFinder
    {
        object GetFor(Document document, string path);
        bool Exists(Document document, string path);
    }
}