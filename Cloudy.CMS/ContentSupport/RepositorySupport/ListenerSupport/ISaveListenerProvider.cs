using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public interface ISaveListenerProvider
    {
        IEnumerable<ISaveListener<object>> GetFor(object content);
    }
}
