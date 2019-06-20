using Poetry.ComponentSupport.DependencySupport;
using Poetry.ComponentSupport;
using System;
using Poetry.UI.StyleSupport;
using Poetry.UI.ScriptSupport;

namespace Poetry.UI.NotificationSupport
{
    [Component("Poetry.UI.NotificationSupport")]
    [Dependency("Poetry.UI")]
    [Style("Styles/notification-manager.css")]
    [Script("Scripts/notification-manager.js")]
    [Script("Scripts/notification.js")]
    public class NotificationSupportComponent
    {
    }
}
