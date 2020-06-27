using Cloudy.CMS.UI.PortalSupport;
using Cloudy.CMS.UI.ScriptSupport;
using Cloudy.CMS.UI.StyleSupport;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.IdentitySupport
{
    [Area("Cloudy.CMS")]
    public class LoginController : Controller
    {
        ITitleProvider TitleProvider { get; }
        IFaviconProvider FaviconProvider { get; }
        IStyleProvider StyleProvider { get; }
        IScriptProvider ScriptProvider { get; }

        public LoginController(ITitleProvider titleProvider, IFaviconProvider faviconProvider, IStyleProvider styleProvider, IScriptProvider scriptProvider)
        {
            TitleProvider = titleProvider;
            FaviconProvider = faviconProvider;
            StyleProvider = styleProvider;
            ScriptProvider = scriptProvider;
        }

        public async Task Index()
        {
            if (!PathEndsInSlash(Request.Path))
            {
                RedirectToPathWithSlash(HttpContext);
                return;
            }

            var basePath = "./files";

            await Response.WriteAsync($"<!DOCTYPE html>\n");
            await Response.WriteAsync($"<html>\n");
            await Response.WriteAsync($"<head>\n");
            await Response.WriteAsync($"    <meta charset=\"utf-8\">\n");
            await Response.WriteAsync($"    <link rel=\"stylesheet\" href=\"https://fonts.googleapis.com/css?family=Roboto:300,400,500,700&amp;display=swap\"/>\n");
            await Response.WriteAsync($"    <base href=\"../\">\n");
            await Response.WriteAsync($"    <title>{TitleProvider.Title}</title>\n");
            await Response.WriteAsync($"\n");
            await Response.WriteAsync($"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">\n");
            await Response.WriteAsync($"\n");
            await Response.WriteAsync($"    <link rel=\"icon\" href=\"{FaviconProvider.Favicon}\">\n");
            await Response.WriteAsync($"\n");
            foreach (var style in StyleProvider.GetAll())
            {
                var path = style.Path.StartsWith("http") || style.Path.StartsWith("/") ? style.Path : Path.Combine(basePath, style.Path).Replace('\\', '/');
                await Response.WriteAsync($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{path}\" />\n");
            }
            foreach (var script in ScriptProvider.GetAll())
            {
                var path = script.Path.StartsWith("http") || script.Path.StartsWith("/") ? script.Path : Path.Combine(basePath, script.Path).Replace('\\', '/');
                await Response.WriteAsync($"    <script src=\"{path}\"></script>\n");
            }
            await Response.WriteAsync($"</head>\n");
            await Response.WriteAsync($"<body>\n");
            await Response.WriteAsync($"    <script type=\"module\">\n");
            await Response.WriteAsync($"        import Login from '{Path.Combine(basePath, "login.js").Replace('\\', '/')}';\n");
            await Response.WriteAsync($"        new Login('Login to {TitleProvider.Title}').appendTo(document.body);\n");
            await Response.WriteAsync($"    </script>\n");
            await Response.WriteAsync($"</body>\n");
            await Response.WriteAsync($"</html>\n");
        }

        bool PathEndsInSlash(PathString path)
        {
            return path.Value.EndsWith("/", StringComparison.Ordinal);
        }

        void RedirectToPathWithSlash(HttpContext context)
        {
            Response.StatusCode = StatusCodes.Status301MovedPermanently;
            var request = context.Request;
            var redirect = UriHelper.BuildAbsolute(request.Scheme, request.Host, request.PathBase, request.Path + "/", request.QueryString);
            Response.Headers[HeaderNames.Location] = redirect;
        }

        public async Task<LoginResult> Authorize([FromBody] LoginInput input)
        {
            var userManager = HttpContext.RequestServices.GetService<UserManager<User>>();
            var user = await userManager.FindByEmailAsync(input.Email);

            if (user == null)
            {
                return new LoginResult { Success = false, Message = $"No such user ({input.Email})" };
            }

            var signinManager = HttpContext.RequestServices.GetService<SignInManager<User>>();
            var result = await signinManager.PasswordSignInAsync(user.Username, input.Password, false, false);

            return new LoginResult { Success = result.Succeeded, Message = result.IsLockedOut ? "Locked out" : result.IsNotAllowed ? "Not allowed" : result.RequiresTwoFactor ? "Requires two factor" : null };
        }

        public class LoginInput
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class LoginResult
        {
            public bool Success { get; set; }
            public string Message { get; set; }
        }
    }
}
