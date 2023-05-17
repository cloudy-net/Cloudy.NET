using Cloudy.CMS.Licensing;
using Cloudy.CMS.Naming;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Cloudy.CMS.UI.List
{
    [Authorize("adminarea")]
    [ResponseCache(NoStore = true)]
    public class LayoutFooterController : Controller
    {
        private readonly IHumanizer humanizer;
        private readonly ILicenseProvider licenseProvider;

        public LayoutFooterController(IHumanizer humanizer, ILicenseProvider licenseProvider)
        {
            this.humanizer = humanizer;
            this.licenseProvider = licenseProvider;
        }

        [HttpGet]
        [Area("Admin")]
        [Route("/{area}/api/layout/footer")]
        public async Task<Footer> GetFooter()
        {
            return new Footer
            {
                BrandName = $"{humanizer.Humanize(Assembly.GetEntryAssembly().GetName().Name)} - {DateTime.Now.Year}",
                IsValidLicense = await licenseProvider.IsValidLicenseAsync()
            };
        }

        public class Footer
        {
            public string BrandName { get; set; }
            public bool IsValidLicense { get; set; }
        }
    }
}
