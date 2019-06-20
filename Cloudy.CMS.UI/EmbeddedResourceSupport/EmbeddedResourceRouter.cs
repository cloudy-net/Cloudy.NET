using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Poetry.ComponentSupport;
using Poetry.EmbeddedResourceSupport;
using Poetry.UI.AppSupport;
using Poetry.UI.RoutableResourceSupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;

namespace Poetry.UI.EmbeddedResourceSupport
{
    public class EmbeddedResourceRouter : IEmbeddedResourceRouter
    {
        IComponentProvider ComponentProvider { get; }
        IEmbeddedResourceProvider EmbeddedResourceProvider { get; }
        IScriptProvider ScriptProvider { get; }
        IStyleProvider StyleProvider { get; }
        IRoutableResourceProvider RoutableResourceProvider { get; }
        IAppProvider AppProvider { get; }

        public EmbeddedResourceRouter(IComponentProvider componentProvider, IEmbeddedResourceProvider embeddedResourceProvider, IScriptProvider scriptProvider, IStyleProvider styleProvider, IRoutableResourceProvider routableResourceProvider, IAppProvider appProvider)
        {
            ComponentProvider = componentProvider;
            EmbeddedResourceProvider = embeddedResourceProvider;
            ScriptProvider = scriptProvider;
            StyleProvider = styleProvider;
            RoutableResourceProvider = routableResourceProvider;
            AppProvider = appProvider;
        }

        public EmbeddedResource Route(string path)
        {
            if (!path.Contains('/'))
            {
                return null;
            }

            var slashIndex = path.IndexOf('/');
            var basePath = path.Substring(0, slashIndex);

            path = path.Substring(slashIndex + 1);

            foreach (var component in ComponentProvider.GetAll().Where(c => c.Id.Equals(basePath, StringComparison.InvariantCultureIgnoreCase)))
            {
                var resource = EmbeddedResourceProvider.Get(component.Id, path);

                if(resource == null)
                {
                    continue;
                }

                foreach (var script in ScriptProvider.GetAllFor(component))
                {
                    if (script.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return resource;
                    }
                }

                foreach (var style in StyleProvider.GetAllFor(component))
                {
                    if (style.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return resource;
                    }
                }

                foreach (var routableResource in RoutableResourceProvider.GetAllFor(component))
                {
                    if (routableResource.Path.Equals(path, StringComparison.InvariantCultureIgnoreCase))
                    {
                        return resource;
                    }
                }
            }

            return null;
        }
    }
}
