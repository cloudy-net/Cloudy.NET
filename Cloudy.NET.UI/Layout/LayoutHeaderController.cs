﻿using Cloudy.NET.EntityTypeSupport;
using Cloudy.NET.EntityTypeSupport.Naming;
using Cloudy.NET.Naming;
using Cloudy.NET.UI.Layout;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cloudy.NET.UI.List
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class LayoutHeaderController : Controller
    {
        private readonly IHumanizer humanizer;
        private readonly IEntityTypeProvider entityTypeProvider;
        private readonly IEntityTypeNameProvider entityTypeNameProvider;

        public LayoutHeaderController(
            IHumanizer humanizer,
            IEntityTypeProvider entityTypeProvider,
            IEntityTypeNameProvider entityTypeNameProvider)
        {
            this.humanizer = humanizer;
            this.entityTypeProvider = entityTypeProvider;
            this.entityTypeNameProvider = entityTypeNameProvider;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/layout/header")]
        public Header GetHeader()
        {
            return new Header
            {
                BrandName = GetBrandName(),
                EntityTypeLinks = GetEntityTypeLinks()
            };
        }


        private string GetBrandName() => humanizer.Humanize(Assembly.GetEntryAssembly().GetName().Name);

        private IEnumerable<EntityTypeLink> GetEntityTypeLinks()
        {
            foreach (var entityType in entityTypeProvider.GetAll().Where(t => t.IsIndependent).OrderBy(c => c.Name))
            {
                var name = entityTypeNameProvider.Get(entityType.Type);

                yield return new EntityTypeLink
                {
                    Text = entityType.IsSingleton ? name.Name : name.PluralName,
                    Url = UrlBuilder.Build(keys: null, "List", entityType.Name)
                };
            }
        }

        public class EntityTypeLink
        {
            public string Text { get; set; }
            public string Url { get; set; }
        }

        public class Header
        {
            public string BrandName { get; set; }
            public IEnumerable<EntityTypeLink> EntityTypeLinks { get; set; }
        }
    }
}
