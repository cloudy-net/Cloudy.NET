
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
        public IDictionary<string, IEnumerable<FieldDescriptor>> GetFields()
        {
            var result = new Dictionary<string, IEnumerable<FieldDescriptor>>();

            foreach(var entityType in EntityTypeProvider.GetAll())
            {
                if(!entityType.IsIndependent)
                {
                    result[entityType.Name] = FieldProvider.Get(entityType.Name).Where(f => f.AutoGenerate ?? true);
                    continue;
                }

                var primaryKeyProperties = PrimaryKeyPropertyGetter.GetFor(entityType.Type);
                result[entityType.Name] = FieldProvider.Get(entityType.Name).Where(f => f.AutoGenerate ?? !primaryKeyProperties.Any(p => p.Name == f.Name));
            }

            return result;
        }
    }
}
