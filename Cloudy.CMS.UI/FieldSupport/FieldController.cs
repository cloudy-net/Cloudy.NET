
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.EntitySupport.PrimaryKey;
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
    public record FieldController(IFieldProvider FieldProvider, IContentTypeProvider ContentTypeProvider, IPrimaryKeyPropertyGetter PrimaryKeyPropertyGetter)
    {

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/form/fields")]
        public object GetFields([FromQuery(Name = "contentType")] string contentTypeName)
        {
            var contentType = ContentTypeProvider.Get(contentTypeName);
            var primaryKeyProperties = PrimaryKeyPropertyGetter.GetFor(contentType.Type);
            return FieldProvider.Get(contentTypeName).Where(f => !primaryKeyProperties.Any(p => p.Name == f.Name));
        }
    }
}
