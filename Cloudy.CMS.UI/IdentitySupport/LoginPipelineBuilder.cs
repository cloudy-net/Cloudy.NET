using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Claims;
using System.Text;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public class LoginPipelineBuilder : ILoginPipelineBuilder
    {
        ILoginPageRenderer LoginPageRenderer { get; }

        public LoginPipelineBuilder(ILoginPageRenderer loginPageRenderer)
        {
            LoginPageRenderer = loginPageRenderer;
        }

        public void Build(IApplicationBuilder app, CloudyAdminOptions options)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGet("/", async context =>
                {
                    if (!PathEndsInSlash(context.Request.Path))
                    {
                        RedirectToPathWithSlash(context);
                        return;
                    }

                    await LoginPageRenderer.RenderAsync(context);
                });

                endpoints.MapPost("/", async context =>
                {
                    var inputString = await new StreamReader(context.Request.Body).ReadToEndAsync();
                    var input = JsonConvert.DeserializeObject<LoginInput>(inputString);
                    var userManager = context.RequestServices.GetService<UserManager<User>>();
                    var user = await userManager.FindByEmailAsync(input.Email);

                    if (user == null)
                    {
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new { success = false, message = $"No such user ({input.Email})" }));
                    }

                    var signinManager = context.RequestServices.GetService<SignInManager<User>>();
                    var result = await signinManager.PasswordSignInAsync(user.Username, input.Password, false, false);

                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new { success = result.Succeeded }));
                });
            });
        }

        public class LoginInput
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        bool PathEndsInSlash(PathString path)
        {
            return path.Value.EndsWith("/", StringComparison.Ordinal);
        }

        void RedirectToPathWithSlash(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status301MovedPermanently;
            var request = context.Request;
            var redirect = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path + "/", request.QueryString);
            context.Response.Headers[HeaderNames.Location] = redirect;
        }
    }
}
