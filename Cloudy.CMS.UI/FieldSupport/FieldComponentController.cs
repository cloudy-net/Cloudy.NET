using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport
{
    public record FieldComponentController(IContentTypeProvider ContentTypeProvider, IFieldProvider FieldProvider)
    {
        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/form/fields/components")]
        public object Get()
        {
            return ContentTypeProvider.GetAll().SelectMany(c => FieldProvider.Get(c.Name)).Select(f => f.Partial).Distinct();
        }
    }
}
