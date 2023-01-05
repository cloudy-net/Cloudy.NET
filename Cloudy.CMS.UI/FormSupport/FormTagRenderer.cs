using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Cloudy.CMS.UI.FormSupport
{
    public static class FormTagRenderer
    {
        public static IHtmlContent RenderCloudyForm(this IHtmlHelper html, ContentTypeDescriptor contentType, IEnumerable<string> keyValues = null)
        {
            var settings = new
            {
                contentType = contentType.Name,
                keyValues
            };
            return new HtmlString($"<div class=\"cloudy-form\" settings=\"{HttpUtility.HtmlAttributeEncode(JsonSerializer.Serialize(settings))}\" ></div>");
        }
    }
}
