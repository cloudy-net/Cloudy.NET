using Cloudy.CMS.UI;
using Microsoft.AspNetCore.Builder;

namespace Cloudy.CMS
{
    public interface IPipelineBuilder
    {
        void Build(IApplicationBuilder app, CloudyAdminOptions options);
    }
}