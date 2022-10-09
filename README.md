# Cloudy creates a CMS out of your EF Core context.

Just create your DbContext, Models, and hook up Cloudy. Configure behavior with UI hints.

![Example of a standard EF Core model in C#](readme-images/model.png?raw=true)
![Screenshot of how Cloudy scaffolds previously mentioned model, showing URL segment that will be used for routing](readme-images/create-new.png?raw=true)
![Code demonstrating how to setup a route with Cloudy](readme-images/setup.png?raw=true)
![Screenshot of a browser routing a request to previously mentioned route](readme-images/routing.png?raw=true)


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