using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FormSupport
{
    public class FormViewComponent : ViewComponent
    {
        IFieldProvider FieldProvider { get; }

        public FormViewComponent(IFieldProvider fieldProvider)
        {
            FieldProvider = fieldProvider;
        }

        public async Task<IViewComponentResult> InvokeAsync(string contentType)
        {
            return View(new FormViewModel
            {
                Fields = FieldProvider.Get(contentType),
            });
        }
    }
}
