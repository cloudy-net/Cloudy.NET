using Poetry.EmbeddedResourceSupport;

namespace Poetry.UI.EmbeddedResourceSupport
{
    public interface IEmbeddedResourceRouter
    {
        EmbeddedResource Route(string path);
    }
}