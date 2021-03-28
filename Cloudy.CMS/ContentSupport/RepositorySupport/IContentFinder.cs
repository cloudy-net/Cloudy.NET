using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport
{
    public interface IContentFinder
    {
        IContentFinderQuery FindInContainer(string container);
    }
}
