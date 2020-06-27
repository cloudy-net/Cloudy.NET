using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.ContentSupport.RepositorySupport.ListenerSupport
{
    public interface ISaveListener<out T> where T : class, IContent
    {
        void BeforeSave(IContent content);
    }
}
