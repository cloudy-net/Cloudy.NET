using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.UI.FormSupport.FieldSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport
{
    public record FieldComponentController(IEntityTypeProvider EntityTypeProvider, IFieldProvider FieldProvider)
    {
        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/form/fields/components")]
        public object Get()
        {
            return EntityTypeProvider.GetAll().SelectMany(c => FieldProvider.Get(c.Name)).Select(f => f.Partial).Where(p => p != null).Distinct();
        }
    }
}
