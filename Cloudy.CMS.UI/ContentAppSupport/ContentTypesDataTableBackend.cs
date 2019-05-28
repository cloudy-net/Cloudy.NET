using FastMember;
using Poetry.UI.DataTableSupport.BackendSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Cloudy.CMS.SingletonSupport;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [DataTableBackend("Cloudy.CMS.ContentTypeList")]
    public class ContentTypesDataTableBackend : IBackend
    {
        int PageSize { get; } = 15;

        IContentTypeProvider ContentTypeRepository { get; }
        ISingletonProvider SingletonProvider { get; }
        IPluralizer Pluralizer { get; }
        IHumanizer Humanizer { get; }
        TypeAccessor TypeAccessor { get; } = TypeAccessor.Create(typeof(Row));

        public ContentTypesDataTableBackend(IContentTypeProvider contentTypeRepository, ISingletonProvider singletonProvider, IPluralizer pluralizer, IHumanizer humanizer)
        {
            ContentTypeRepository = contentTypeRepository;
            SingletonProvider = singletonProvider;
            Pluralizer = pluralizer;
            Humanizer = humanizer;
        }

        public Result Load(Query query)
        {
            var items = ContentTypeRepository.GetAll().Select(CreateRow);
            var sortBy = (Func<Row, object>)(c => TypeAccessor[c, query.SortBy ?? "Name"]);

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

        Row CreateRow(ContentTypeDescriptor contentType)
        {
            var name = contentType.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? contentType.Type.Name;
            string pluralName;

            if (name.Contains(':') && !contentType.Id.Contains(':'))
            {
                var nameSplit = name.Split(':');

                name = nameSplit.First();
                pluralName = nameSplit.Last();
            }
            else
            {
                name = Humanizer.Humanize(name);
                pluralName = Pluralizer.Pluralize(name);
            }

            var singleton = SingletonProvider.Get(contentType.Id);

            return new Row
            {
                Id = contentType.Id,
                Name = name,
                PluralName = pluralName,
                IsNameable = typeof(INameable).IsAssignableFrom(contentType.Type),
                IsSingleton = singleton != null,
                SingletonId = singleton?.Id,
                Count = -1,
            };
        }

        public class Row
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PluralName { get; set; }
            public bool IsNameable { get; set; }
            public bool IsSingleton { get; set; }
            public string SingletonId { get; set; }
            public int Count { get; set; }
        }
    }
}
