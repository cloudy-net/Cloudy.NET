# Cloudy is an automatic scaffolding solution for .NET using EF Core.

Just create your DbContext, Models, and hook up Cloudy. Override behavior with UI hints, just as the good old days.

![Screenshot of the Cloudy CMS admin UI.](/screenshot.png?raw=true)

# Installation

Create a new, empty ASP.NET Core web application.

Install Cloudy.CMS and Cloudy.CMS.UI from NuGet.

```
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages().AddApplicationPart(typeof(CloudyUIAssemblyHandle).Assembly);
builder.Services.AddMvc();
builder.Services.AddCloudy(cloudy => cloudy
    .AddAdmin(admin => admin.Unprotect())   // NOTE: Admin UI will be publicly available!
    .AddContext<MyContext>()                // Adds EF Core context with your content types
);

var app = builder.Build();

app.UseStaticFiles(new StaticFileOptions
{
    // Not strictly necessary, but good - browsers will cache but revalidate on ETag every time.
    OnPrepareResponse = ctx => ctx.Context.Response.Headers.Append("Cache-Control", $"no-cache")
});

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => {
    endpoints.MapRazorPages();
    endpoints.MapGet("/", async c => c.Response.Redirect("/Admin"));
});

app.Run();
```

# Authentication

The UI works well with external login providers. Just follow the official guides and don't forget to remove `Unprotect()` in UseCloudyAdmin!

# Database

Cloudy.CMS is made to align completely with Entity Framework Core.

# Usage for large corporations

This CMS is under active development, and is not guaranteed to keep API compatibility between releases.