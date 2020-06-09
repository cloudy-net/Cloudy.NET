using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.ContentTypeSupport.GroupSupport;
using Cloudy.CMS.SingletonSupport;
using Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport;
using Cloudy.CMS.UI.ContentAppSupport.ListActionSupport;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Area("Cloudy.CMS")]
    [Route("ContentTypeGroupProvider/")]
    public class ContentTypeGroupProviderController : Controller
    {
        IContentTypeGroupProvider ContentTypeGroupProvider { get; }
        IHumanizer Humanizer { get; }
        IPluralizer Pluralizer { get; }
        ISingletonProvider SingletonProvider { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();

        public ContentTypeGroupProviderController(IContentTypeGroupProvider contentTypeGroupProvider, IHumanizer humanizer, IPluralizer pluralizer, ISingletonProvider singletonProvider)
        {
            ContentTypeGroupProvider = contentTypeGroupProvider;
            Humanizer = humanizer;
            Pluralizer = pluralizer;
            SingletonProvider = singletonProvider;
        }

        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<ContentTypeGroupResponseItem> GetContentTypeGroupList()
        {
            var result = new List<ContentTypeGroupResponseItem>();

            foreach (var contentType in ContentTypeGroupProvider.GetAll())
            {
                result.Add(GetItem(contentType));
            }

            return result.AsReadOnly();
        }

        private ContentTypeGroupResponseItem GetItem(ContentTypeGroupDescriptor contentTypeGroup)
        {
            var name = contentTypeGroup.Type.GetCustomAttribute<DisplayAttribute>()?.Name ?? contentTypeGroup.Type.Name;
            string pluralName;

            if (name.Contains(':') && !contentTypeGroup.Id.Contains(':'))
            {
                var nameSplit = name.Split(':');

                name = nameSplit.First();
                pluralName = nameSplit.Last();
            }
            else
            {
                if(name.Length >= 2 && name.StartsWith('I') && Char.IsUpper(name[1]))
                {
                    name = name.Substring(1);
                }

                name = Humanizer.Humanize(name);
                pluralName = Pluralizer.Pluralize(name);
            }

            var item = new ContentTypeGroupResponseItem
            {
                Id = contentTypeGroup.Id,
                Name = name,
                PluralName = pluralName,
            };
            return item;
        }

        public class ContentTypeGroupResponseItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string PluralName { get; set; }
        }
    }
}
