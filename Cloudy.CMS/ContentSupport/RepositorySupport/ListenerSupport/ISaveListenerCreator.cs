using System.Collections;
using System.Collections.Generic;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public interface ISaveListenerCreator
    {
        IEnumerable<ISaveListener<IContent>> Create();
    }
}