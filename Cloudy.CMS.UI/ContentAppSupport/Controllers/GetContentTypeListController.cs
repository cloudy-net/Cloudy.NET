using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
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
    [Route("Content")]
    public class GetContentTypeListController : Controller
    {
        IContentTypeProvider ContentTypeProvider { get; }
        IHumanizer Humanizer { get; }
        IPluralizer Pluralizer { get; }
        ISingletonProvider SingletonProvider { get; }
        CamelCaseNamingStrategy CamelCaseNamingStrategy { get; } = new CamelCaseNamingStrategy();
        IContentTypeActionModuleProvider ContentTypeActionModuleProvider { get; }
        INameExpressionParser NameExpressionParser { get; }
        IListActionModuleProvider ListActionModuleProvider { get; }

        public GetContentTypeListController(IContentTypeProvider contentTypeProvider, IHumanizer humanizer, IPluralizer pluralizer, ISingletonProvider singletonProvider, IContentTypeActionModuleProvider contentTypeActionModuleProvider, INameExpressionParser nameExpressionParser, IListActionModuleProvider listActionModuleProvider)
        {
            ContentTypeProvider = contentTypeProvider;
            Humanizer = humanizer;
            Pluralizer = pluralizer;
            SingletonProvider = singletonProvider;
            ContentTypeActionModuleProvider = contentTypeActionModuleProvider;
            NameExpressionParser = nameExpressionParser;
            ListActionModuleProvider = listActionModuleProvider;
        }

        [HttpGet]
        [Route("GetContentTypeList")]
        public IEnumerable<ContentTypeResponseItem> GetContentTypeList()
        {
            var result = new List<ContentTypeResponseItem>();

            foreach (var contentType in ContentTypeProvider.GetAll())
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

                result.Add(new ContentTypeResponseItem
                {
                    Id = contentType.Id,
                    Name = name,
                    LowerCaseName = name.Substring(0, 1).ToLower() + name.Substring(1),
                    PluralName = pluralName,
                    LowerCasePluralName = pluralName.Substring(0, 1).ToLower() + pluralName.Substring(1),
                    IsNameable = typeof(INameable).IsAssignableFrom(contentType.Type),
                    NameablePropertyName = typeof(INameable).IsAssignableFrom(contentType.Type) ? CamelCaseNamingStrategy.GetPropertyName(NameExpressionParser.Parse(contentType.Type), false) : null,
                    IsRoutable = typeof(IRoutable).IsAssignableFrom(contentType.Type),
                    IsSingleton = singleton != null,
                    Count = -1,
                    ContentTypeActionModules = ContentTypeActionModuleProvider.GetContentTypeActionModulesFor(contentType.Id),
                    ListActionModules = ListActionModuleProvider.GetListActionModulesFor(contentType.Id),
                });
            }

            return result.AsReadOnly();
        }

        public class ContentTypeResponseItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string LowerCaseName { get; set; }
            public string PluralName { get; set; }
            public string LowerCasePluralName { get; set; }
            public bool IsNameable { get; set; }
            public string NameablePropertyName { get; set; }
            public bool IsRoutable { get; set; }
            public bool IsSingleton { get; set; }
            public string SingletonId { get; set; }
            public int Count { get; set; }
            public IEnumerable<string> ContentTypeActionModules { get; set; }
            public IEnumerable<string> ListActionModules { get; set; }
        }
    }
}
