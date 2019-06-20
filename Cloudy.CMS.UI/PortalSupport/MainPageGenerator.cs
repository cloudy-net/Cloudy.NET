using Newtonsoft.Json;
using Poetry.UI.AppSupport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Poetry.ComponentSupport;
using Poetry.ComponentSupport.DependencySupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;

namespace Poetry.UI.PortalSupport
{
    public class MainPageGenerator : IMainPageGenerator
    {
        IComponentProvider ComponentProvider { get; }
        IComponentDependencySorter ComponentDependencySorter { get; }
        IBasePathProvider BasePathProvider { get; }
        IScriptProvider ScriptProvider { get; }
        IStyleProvider StyleProvider { get; }
        IAppProvider AppProvider { get; }
        IFaviconProvider FaviconProvider { get; }
        ITitleProvider TitleProvider { get; }

        public MainPageGenerator(IComponentProvider componentProvider, IComponentDependencySorter componentDependencySorter, IBasePathProvider basePathProvider, IScriptProvider scriptProvider, IStyleProvider styleProvider, IAppProvider appProvider, IFaviconProvider faviconProvider, ITitleProvider titleProvider)
        {
            ComponentProvider = componentProvider;
            ComponentDependencySorter = componentDependencySorter;
            BasePathProvider = basePathProvider;
            ScriptProvider = scriptProvider;
            StyleProvider = styleProvider;
            AppProvider = appProvider;
            FaviconProvider = faviconProvider;
            TitleProvider = titleProvider;
        }

        public void Generate(Stream write)
        {
            using (var output = new StreamWriter(write, Encoding.Unicode))
            {
                output.WriteLine($"<!DOCTYPE html>");
                output.WriteLine($"<html>");
                output.WriteLine($"  <head>");
                output.WriteLine($"    <meta charset=\"utf-8\">");
                output.WriteLine($"    <title>{TitleProvider.Title}</title>");
                output.WriteLine($"    <base href=\"/{BasePathProvider.BasePath}/\">");
                output.WriteLine($"    <meta name=\"viewport\" content=\"width=device-width, initial-scale=1\">");
                output.WriteLine($"    <link rel=\"icon\" href=\"{FaviconProvider.Favicon}\">");

                foreach (var component in ComponentDependencySorter.Sort(ComponentProvider.GetAll()))
                {
                    foreach (var script in ScriptProvider.GetAllFor(component))
                    {
                        output.WriteLine($"    <script type=\"module\" src=\"{component.Id}/{script.Path}\" crossorigin=\"use-credentials\"></script>");
                    }
                    foreach (var style in StyleProvider.GetAllFor(component))
                    {
                        output.WriteLine($"    <link rel=\"stylesheet\" type=\"text/css\" href=\"{component.Id}/{style.Path}\" />");
                    }
                }

                output.WriteLine($"  </head>");
                output.WriteLine($"  <body>");
                output.WriteLine($"    <script type=\"module\">");
                output.WriteLine($"      import Portal from './Poetry.UI/Scripts/portal.js';");
                output.WriteLine($"      ");
                output.WriteLine($"      new Portal();");
                output.WriteLine($"    </script>");
                output.WriteLine($"  </body>");
                output.WriteLine($"</html>");
            }
        }
    }
}
