using Cloudy.CMS.ContentSupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentMover
    {
        void Move(IContent content, string id);
    }
}