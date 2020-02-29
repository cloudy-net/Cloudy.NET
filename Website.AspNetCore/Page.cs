using Cloudy.CMS.ContentSupport;
using Cloudy.CMS.ContentTypeSupport;
using Cloudy.CMS.UI.FormSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Website.AspNetCore
{
    [ContentType("80ca82a2-b46e-4394-88e0-a77ae93a9366")]
    public class Page : IContent, INameable, IRoutable
    {
        public string Id { get; set; }
        public string ContentTypeId { get; set; }
        public string Name { get; set; }
        [UIHint("textarea")]
        public string Description { get; set; }
        [UIHint("password")]
        public string PinCode { get; set; }
        public bool PinnedToTop { get; set; }
        public Meta Metadata { get; set; }

        [Form("957966b6-c6e7-4497-8b8b-0a772f39e5f0")]
        public class Meta
        {
            public string Title { get; set; }
            [UIHint("image")]
            public string Image { get; set; }
            [UIHint("textarea({rows:3})")]
            public string Description { get; set; }
            public string Keywords { get; set; }
            public Item Item { get; set; }
        }

        [Display(GroupName = "Examples")]
        public IEnumerable<string> Ids { get; set; }
        [Display(GroupName = "Examples")]
        public IEnumerable<Item> Items { get; set; }

        [Display(GroupName = "Examples")]
        [UIHint("table({Name:name,Price:price})")]
        public IEnumerable<Item> ItemsTable { get; set; }
        public string UrlSegment { get; set; }

        [Form("d21d20a4-dc8a-415e-82c9-849037b972d0")]
        public class Item
        {
            public string Name { get; set; }
            public double Price { get; set; }
        }
    }
}
