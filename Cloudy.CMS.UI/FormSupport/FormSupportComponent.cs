using Poetry.ComponentSupport;
using Poetry.ComponentSupport.DependencySupport;
using Poetry.UI.ScriptSupport;
using Poetry.UI.StyleSupport;
using System;
using System.Collections.Generic;
using System.Text;

namespace Poetry.UI.FormSupport
{
    [Component("Poetry.UI.FormSupport")]
    [Script("Scripts/field.js")]
    [Script("Scripts/field-model.js")]
    [Script("Scripts/field-control.js")]
    [Script("Scripts/field-control-provider.js")]
    [Script("Scripts/field-descriptor-provider.js")]
    [Script("Scripts/form-builder.js")]
    [Script("Scripts/form.js")]
    [Script("Scripts/sortable.js")]
    [Script("Scripts/sortable-item.js")]
    [Style("Styles/form-elements.css")]
    [Dependency("Poetry.UI")]
    public class FormSupportComponent
    {
    }
}
