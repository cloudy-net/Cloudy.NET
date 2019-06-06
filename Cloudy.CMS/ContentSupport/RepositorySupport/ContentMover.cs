using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.RepositorySupport;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public class ContentMover : IContentMover
    {
        IContentUpdater ContentUpdater { get; }

        public ContentMover(IContentUpdater contentUpdater)
        {
            ContentUpdater = contentUpdater;
        }

        public void Move(IContent content, string id)
        {
            var hierarchical = content as IHierarchical;

            if(hierarchical == null)
            {
                throw new ArgumentException($"Content with Id {content.Id} ({content.ContentTypeId}) is not IHierarchical");
            }

            hierarchical.ParentId = id;

            ContentUpdater.Update(content);
        }
    }
}
