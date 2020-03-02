using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cloudy.CMS.UI.ContentAppSupport.Controllers
{
    [Area("Cloudy.CMS")]
    [Route("Content")]
    public class GetSettingsController
    {
        CloudyAdminOptions CloudyAdminOptions { get; }

        public GetSettingsController(CloudyAdminOptions cloudyAdminOptions)
        {
            CloudyAdminOptions = cloudyAdminOptions;
        }

        [HttpGet]
        [Route("GetSettings")]
        public ContentAppSettings GetSettings()
        {
            return new ContentAppSettings(CloudyAdminOptions.HelpSectionBaseUri);
        }

        public class ContentAppSettings
        {
            public string HelpSectionBaseUri { get; }

            public ContentAppSettings(string helpSectionBaseUri)
            {
                HelpSectionBaseUri = helpSectionBaseUri;
            }
        }
    }
}
