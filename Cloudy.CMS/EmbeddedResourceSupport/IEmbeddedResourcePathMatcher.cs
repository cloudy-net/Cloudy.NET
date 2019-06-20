namespace Poetry.EmbeddedResourceSupport
{
    public interface IEmbeddedResourcePathMatcher
    {
        bool Matches(EmbeddedResource embeddedResource, string path);
    }
}