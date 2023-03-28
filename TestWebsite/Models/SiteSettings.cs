using Cloudy.CMS.SingletonSupport;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using TestWebsite.Constants;

namespace TestWebsite.Models
{
    [Display(GroupName = General.GroupNames.Settings)]
    public class SiteSettings : ISingleton
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SiteName { get; set; }
    }
}
