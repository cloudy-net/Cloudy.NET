
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public record FieldController(IFieldProvider FieldProvider)
    {

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/form/fields")]
        public object GetFields(string contentType)
        {
            return FieldProvider.Get(contentType);
        }
    }
}
