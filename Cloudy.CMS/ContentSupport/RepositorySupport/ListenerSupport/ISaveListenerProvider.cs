using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public interface ISaveListenerProvider
    {
        IEnumerable<ISaveListener<IContent>> GetFor(IContent content);
    }
}
