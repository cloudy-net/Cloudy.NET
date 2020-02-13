using Microsoft.AspNetCore.Builder;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public interface ILoginPipelineBuilder
    {
        void Build(IApplicationBuilder app, CloudyAdminOptions options);
    }
}