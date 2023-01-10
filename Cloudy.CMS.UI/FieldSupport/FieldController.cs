
using Cloudy.CMS.EntityTypeSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
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
    public record FieldController(IFieldProvider FieldProvider, IEntityTypeProvider EntityTypeProvider, IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter)
    {

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/form/fields")]
        public IEnumerable<FieldDescriptor> GetFields([FromQuery(Name = "type")] string entityTypeName)
        {
            var entityType = EntityTypeProvider.Get(entityTypeName);
            var primaryKeyProperties = PrimaryKeyPropertyGetter.GetFor(entityType.Type);
            return FieldProvider.Get(entityTypeName).Where(f => f.AutoGenerate ?? !primaryKeyProperties.Any(p => p.Name == f.Name));
        }
    }
}
