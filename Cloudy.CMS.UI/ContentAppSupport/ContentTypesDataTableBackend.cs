using FastMember;
using Poetry.UI.DataTableSupport.BackendSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [DataTableBackend("Cloudy.CMS.ContentTypeList")]
    public class ContentTypesDataTableBackend : IBackend
    {
        int PageSize { get; } = 15;

        IContentTypeProvider ContentTypeRepository { get; }
        TypeAccessor TypeAccessor { get; } = TypeAccessor.Create(typeof(ContentTypeItem));

        public ContentTypesDataTableBackend(IContentTypeProvider contentTypeRepository)
        {
            ContentTypeRepository = contentTypeRepository;
        }

        public Result Load(Query query)
        {
            var items = ContentTypeRepository.GetAll().Select(ContentTypeItem.CreateFrom);
            var sortBy = (Func<ContentTypeItem, object>)(c => TypeAccessor[c, query.SortBy ?? "Name"]);

            if (query.SortDirection == SortDirection.Descending)
            {
                items = items.OrderByDescending(sortBy);
            }
            else
            {
                items = items.OrderBy(sortBy);
            }

            return new Result(
                PageSize,
                items.Skip(PageSize * (query.Page - 1)).Take(PageSize),
                items.Count()
            );
        }

        public class ContentTypeItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public bool IsNameable { get; set; }
            public int Count { get; set; }

            public static ContentTypeItem CreateFrom(ContentTypeDescriptor contentType)
            {
                return new ContentTypeItem
                {
                    Id = contentType.Id,
                    Name = contentType.Id,
                    IsNameable = typeof(INameable).IsAssignableFrom(contentType.Type),
                    Count = -1,
                };
            }
        }
    }
}
