using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Poetry.UI.AppSupport;
using Poetry.UI.AspNetCore.EmbeddedResourceSupport;
using Poetry.UI.ApiSupport;
using Poetry.DependencyInjectionSupport;
using Poetry.UI.PortalSupport;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using Poetry.UI.AspNetCore.ApiSupport;
using Poetry.UI.AspNetCore.PortalSupport;
using Poetry.UI.ControlMessageSupport;
using Poetry.UI.FormSupport;
using Poetry.UI.NotificationSupport;
using Poetry.UI.DataTableSupport;
using Poetry.UI.ContextMenuSupport;
using Poetry.UI.AspNetCore.AuthorizationSupport;

namespace Poetry.UI.AspNetCore
{
    public static class Startup
    {
        public static bool HasInitialized { get; private set; }

        /// <summary>
        /// Adds Poetry UI to your application.
        /// </summary>
        public static void AddUI(this PoetryConfigurator poetryConfigurator)
        {
            poetryConfigurator.AddUI(null);
        }

        /// <summary>
        /// Adds Poetry UI to your application.
        /// </summary>
        public static void AddUI(this PoetryConfigurator poetryConfigurator, Action<PoetryUIConfigurator> configuratorFunction)
        {
            if (HasInitialized)
            {
                throw new AlreadyInitializedException();
            }

            HasInitialized = true;

            var configurator = new PoetryUIConfigurator(poetryConfigurator);

            poetryConfigurator.AddComponent<PoetryUIComponent>();
            poetryConfigurator.AddComponent<ContextMenuSupportComponent>();
            poetryConfigurator.AddComponent<ControlMessageSupportComponent>();
            poetryConfigurator.AddComponent<DataTableSupportComponent>();
            poetryConfigurator.AddComponent<FormSupportComponent>();
            poetryConfigurator.AddComponent<NotificationSupportComponent>();

            poetryConfigurator.InjectSingleton<IUIAuthorizationPolicyProvider, UIAuthorizationPolicyProvider>();

            configuratorFunction?.Invoke(configurator);

            configurator.Done();
        }

        public static PoetryUIConfigurator SetAuthorizationPolicy(this PoetryUIConfigurator poetryUIConfigurator, UIAuthorizeOptions uiAuthorizeOptions)
        {
            UIAuthorizationPolicyProvider.Set(uiAuthorizeOptions);

            return poetryUIConfigurator;
        }

        public static void UsePoetryUI(this IApplicationBuilder app)
        {
            app.UseMiddleware<MainPageMiddleware>();
            app.UseMiddleware<EmbeddedResourcesMiddleware>();
            app.UseMiddleware<ApiMiddleware>();
        }
    }
}
