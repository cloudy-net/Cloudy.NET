using Cloudy.CMS.EntityTypeSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.FieldSupport
{
    [ResponseCache(NoStore = true)]
    public record FieldComponentController(IEntityTypeProvider EntityTypeProvider, IFieldProvider FieldProvider)
    {
        IEnumerable<string> Values { get; } = EntityTypeProvider.GetAll()
            .SelectMany(c => FieldProvider.Get(c.Name))
            .SelectMany(f => new List<string> { f.Partial, f.ListPartial })
            .Where(p => p != null)
            .Distinct();

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/form/fields/components")]
        public object Get()
        {
            return Values;
        }
    }
}
