using Cloudy.CMS.ContentSupport.RepositorySupport.Context;
using Cloudy.CMS.ContentTypeSupport;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    public class ListResultController
    {
        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/list/result")]
        public ListResultResponse ListResult(string contentType, string query, [FromServices] IContentTypeProvider contentTypeProvider, [FromServices] IContextCreator contextCreator)
        {
            var type = contentTypeProvider.Get(contentType);

            using var context = contextCreator.CreateFor(type.Type);

            var dbSet = (IQueryable)context.GetDbSet(type.Type).DbSet;

            return new ListResultResponse
            {
                Items = dbSet.Where($"Name.Contains(@0)", query).ToDynamicList(),
            };
        }

        public class ListResultPayload
        {
            public List<string> Columns { get; set; }
        }

        public class ListResultResponse
        {
            public IEnumerable<object> Items { get; set; }
        }
    }
}
