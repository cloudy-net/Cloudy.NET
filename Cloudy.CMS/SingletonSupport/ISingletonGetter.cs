using Cloudy.CMS.ContentSupport;
using System;
using System.Threading.Tasks;

namespace Cloudy.CMS.SingletonSupport
{
    public interface ISingletonGetter
    {
        Task<IContent> GetAsync(string contentTypeId, string language);
        T Get<T>(string language) where T : class;
    }
}