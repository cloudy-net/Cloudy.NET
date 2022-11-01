using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Builder
{
    public static class StaticFileOptionsExtensions
    {
        /// <summary>
        /// Adds Cache-Control: no-cache to the static file HTTP response. Removes the need for manually clearing browser cache when Cloudy updates its frontend assets.
        /// 
        /// The no-cache response directive indicates that the response can be stored in caches, but the response must be validated with the origin server before each reuse, even when the cache is disconnected from the origin server.
        /// 
        /// Note that if you have the files cached in your browser, and add this directive after the fact, you still need to clear your browser cache in order to detect the new directive.
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public static StaticFileOptions MustValidate(this StaticFileOptions options)
        {
            if(options.OnPrepareResponse == null)
            {
                options.OnPrepareResponse = ctx => ctx.Context.Response.Headers.Append("Cache-Control", $"no-cache");

                return options;
            }
            
            var oldOnPrepareResponse = options.OnPrepareResponse;

            options.OnPrepareResponse = ctx =>
            {
                oldOnPrepareResponse(ctx);
                ctx.Context.Response.Headers.Append("Cache-Control", $"no-cache");
            };

            return options;
        }
    }
}
