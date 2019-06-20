using Poetry.ComponentSupport;
using System;
using Poetry.ComponentSupport.DependencySupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;

namespace Poetry.UI.DataTableSupport
{
    [Component("Poetry.UI.DataTableSupport")]
    [Script("Scripts/data-table.js")]
    [Script("Scripts/backend.js")]
    [Script("Scripts/data-table-button.js")]
    [Script("Scripts/copy-as-tab-separated.js")]
    [Style("Styles/data-table.css")]
    [Dependency("Poetry.UI")]
    public class DataTableSupportComponent
    {
    }
}
