using Cloudy.CMS.UI.FormSupport.Controls.DropdownControlSupport;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentSupport.Serialization;
using Cloudy.CMS.ContentSupport.RepositorySupport;

namespace Cloudy.CMS.UI.ContentAppSupport
{
    [OptionProvider("parent")]
    public class ParentOptionProvider : IOptionProvider
    {
        IContentFinder ContentFinder { get; }
        IContentTypeProvider ContentTypeProvider { get; }
        IPrimaryKeyGetter PrimaryKeyGetter { get; }

        public ParentOptionProvider(IContentFinder contentFinder, IContentTypeProvider contentTypeProvider, IPrimaryKeyGetter primaryKeyGetter)
        {
            ContentFinder = contentFinder;
            ContentTypeProvider = contentTypeProvider;
            PrimaryKeyGetter = primaryKeyGetter;
        }

        public IEnumerable<Option> GetAll()
        {
            var contentTypes = ContentTypeProvider.GetAll().Where(t => typeof(IHierarchical).IsAssignableFrom(t.Type));

            var result = new List<Option>();

            foreach (var contentType in contentTypes)
            {
                var documents = ContentFinder.FindInContainer(contentType.Container).WithContentType(contentType.Id).GetResultAsync().Result.ToList();

                foreach (var content in documents)
                {
                    var id = "{" + string.Join(",", PrimaryKeyGetter.Get(content)) + "}";
                    result.Add(new Option((content as INameable).Name ?? id, id));
                }
            }

            result = result.OrderBy(i => i.Value).ToList();

            result.Insert(0, new Option("(root)", null));

            return result.AsReadOnly();
        }
    }
}
