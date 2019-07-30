namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public interface IFilePathProvider
    {
        string GetPathFor(string container);
        string GetPathFor(string container, string id);
    }
}