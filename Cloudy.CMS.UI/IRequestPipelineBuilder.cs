using Cloudy.CMS.UI;
using Microsoft.AspNetCore.Builder;

namespace Cloudy.CMS
{
    public interface IRequestPipelineBuilder
    {
        void Build(IApplicationBuilder app, CloudyAdminOptions options);
    }
}