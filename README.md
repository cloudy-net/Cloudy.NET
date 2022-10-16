# Cloudy creates a CMS out of your EF Core context.

Just create your DbContext, Models, and hook up Cloudy. Configure behavior with UI hints.

```C#
[Display(Description = "This is a sample class to show off most bells and whistles of the CMS toolkit.")]
public class Page : INameable, IRoutable
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string UrlSegment { get; set; }
    [UIHint("textarea")]
    public string Description { get; set; }
    [Select(typeof(Page))]
    public string RelatedPageId { get; set; }
}
```

![Screenshot of how Cloudy scaffolds previously mentioned model, showing URL segment that will be used for routing](readme-images/create-new.png?raw=true)

```C#
endpoints.MapGet("/pages/{route:contentroute}", async c => 
    await c.Response.WriteAsync($"Hello {c.GetContentFromContentRoute<Page>().Name}")
);
```

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
    endpoints.MapControllers();
    endpoints.MapGet("/", async c => c.Response.Redirect("/Admin"));
});

app.Run();
```

# Authentication

The UI works well with external login providers. Just follow the official guides and don't forget to remove `Unprotect()` in UseCloudyAdmin!

# Database

Cloudy.CMS supports any database supported by EF Core: Inmemory, SQLite, SQL Server, CosmosDB ... even `/dev/null` (for legal reasons, that last one was a joke)

# Building the repository

Clone the repo and run. To get the frontend code of the Admin section running, run `npm ci` and `npm run build` from the `wwwroot-src/` folder. Alternatively, you can run `npm ci` and then `npm run dev` and set the Configuration value (right click the sample project in Visual Studio and choose "Edit user secrets") `ViteBaseUri` to the running Vite URL, something like `http://localhost:5173/`.

# Usage for large corporations

This CMS is under active development, and is not guaranteed to keep API compatibility between releases.