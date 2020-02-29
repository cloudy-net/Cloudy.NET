![Screenshot of the Cloudy CMS admin UI.](/screenshot.png?raw=true)

# Installation

Create a new, empty ASP.NET Core web application.

Install Cloudy.CMS and Cloudy.CMS.UI from NuGet.

In Startup.cs, under ConfigureServices, do:

    services.AddMvc();
    services.AddCloudy(cloudy => cloudy.AddAdmin());

And in the Configure method, do:

    app.UseCloudyAdmin(cloudy => cloudy.Unprotect()); // NOTE: Admin UI will be publicly available!

Then visit `/Admin` for the royal tour.

To route INavigatable content (will work on /pages/MyUrlSegment etc), do:

    app.UseRouting();
    app.UseEndpoints(endpoints => {
        endpoints.MapGet("/pages/{route:contentroute}", async c => await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute().Id}"));
        endpoints.MapControllerRoute(null, "/controllertest/{route:contentroute}", new { controller = "Page", action = "Blog" });
    });

In the controller, do:

    public ActionResult Index([FromContentRoute] IContent page)

To use IHierarchical content (nested pages), you need to use a `**` wildcard like `{**route:....`

# Authentication

To use ASP.NET Identity (UI) with Users managed by Cloudy, create an example project with individual user accounts, and uninstall the EF stuff. Instead uf IdentityUser and IdentityUserStore, use `User` and `UserStore` and don't forget to remove `Unprotect()` in UseCloudyAdmin!

The UI works well with OAuth aka external login providers. Just follow the guides eg. [Google authentication](https://docs.microsoft.com/en-us/aspnet/core/security/authentication/social/social-without-identity?view=aspnetcore-3.0) and don't forget to use `Authorize()` in UseCloudyAdmin!

# Database

Uses inmemory database by default.

To use a physical folder with JSON documents, do: `.WithStaticFiles()` under AddCloudy.

To use MongoDB, do: `.WithMongoDatabaseConnectionStringNamed("mongo")` under AddCloudy.

# Usage for large corporations

This CMS is under active development, and is not guaranteed to keep API compatibility between releases.

If you are a large corporation or otherwise require something like a LTS, the best practice is to *clone the repo* and setup your own build pipeline with your own NuGet packages.

When you wish to upgrade (either for security reasons or if necessary functionality has been added or changed), you merge from this repo at the tag you planned, and only deploy to production after the upgrade has been fully regression tested by your QA team.