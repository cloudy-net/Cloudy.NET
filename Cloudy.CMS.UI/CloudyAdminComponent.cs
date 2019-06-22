using Poetry.ComponentSupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI
{
    [Component("Cloudy.CMS.Admin")]
    [Script("NaggingSupport/nagging.js")]
    [Script("Scripts/portal.js")]
    [Script("Scripts/nav.js")]
    [Script("Scripts/app.js")]
    [Script("Scripts/blade.js")]
    [Script("Scripts/close-button.js")]
    [Script("Scripts/button.js")]
    [Script("Scripts/link-button.js")]
    [Script("Scripts/remove-element-listener.js")]
    [Style("Styles/portal.css")]
    [Script("ContextMenuSupport/Scripts/context-menu.js")]
    [Style("ContextMenuSupport/Styles/context-menu.css")]
    [Script("ControlMessageSupport/Scripts/control-message.js")]
    [Style("ControlMessageSupport/Styles/control-message.css")]
    [Script("DataTableSupport/Scripts/data-table.js")]
    [Script("DataTableSupport/Scripts/backend.js")]
    [Script("DataTableSupport/Scripts/data-table-button.js")]
    [Script("DataTableSupport/Scripts/copy-as-tab-separated.js")]
    [Style("DataTableSupport/Styles/data-table.css")]
    [Script("FormSupport/Scripts/field.js")]
    [Script("FormSupport/Scripts/field-model.js")]
    [Script("FormSupport/Scripts/field-control.js")]
    [Script("FormSupport/Scripts/field-control-provider.js")]
    [Script("FormSupport/Scripts/field-descriptor-provider.js")]
    [Script("FormSupport/Scripts/form-builder.js")]
    [Script("FormSupport/Scripts/form.js")]
    [Script("FormSupport/Scripts/sortable.js")]
    [Script("FormSupport/Scripts/sortable-item.js")]
    [Style("FormSupport/Styles/form-elements.css")]
    [Style("NotificationSupport/Styles/notification-manager.css")]
    [Script("NotificationSupport/Scripts/notification-manager.js")]
    [Script("NotificationSupport/Scripts/notification.js")]
    public class CloudyAdminComponent
    {
    }
}
