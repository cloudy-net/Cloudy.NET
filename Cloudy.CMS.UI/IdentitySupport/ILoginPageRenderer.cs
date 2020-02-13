using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public interface ILoginPageRenderer
    {
        Task RenderAsync(HttpContext context);
    }
}