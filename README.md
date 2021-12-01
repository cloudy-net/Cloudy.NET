![Screenshot of the Cloudy CMS admin UI.](/screenshot.png?raw=true)

# Installation

Create a new, empty ASP.NET Core web application.

Install Cloudy.CMS and Cloudy.CMS.UI from NuGet.

```
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMvc();
builder.Services.AddCloudy(cloudy => cloudy
    .AddAdmin(admin => admin.Unprotect())   // NOTE: Admin UI will be publicly available!
    .AddContext<MyContext>()                // Adds EF Core context with your content types
);

var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.UseCloudyAdminStaticFiles();

app.UseEndpoints(endpoints => {
    endpoints.MapCloudyAdminRoutes();
});

app.Run();
```

Then visit `/Admin` for the royal tour.

To route INavigatable content (will work on /pages/MyUrlSegment etc), do:

    app.UseEndpoints(endpoints => {
        endpoints.MapGet("/pages/{route:contentroute}", async c => await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute().Id}"));
        endpoints.MapControllerRoute(null, "/controllertest/{route:contentroute}", new { controller = "Page", action = "Blog" });
    });

In the controller, do:

    public ActionResult Index([FromContentRoute] IContent page)

# Authentication

The UI works well with external login providers. Just follow the guides eg. [Google authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/social-without-identity?view=aspnetcore-3.0) and don't forget to use `Authorize()` in UseCloudyAdmin!

# Database

Cloudy.CMS is made to align completely with Entity Framework Core.

# Usage for large corporations

This CMS is under active development, and is not guaranteed to keep API compatibility between releases.

If you are a large corporation or otherwise require something like a LTS, the best practice is to *clone the repo* and setup your own build pipeline with your own NuGet packages.

When you wish to upgrade (either for security reasons or if necessary functionality has been added or changed), you merge from this repo at the tag you planned, and only deploy to production after the upgrade has been fully regression tested by your QA team.