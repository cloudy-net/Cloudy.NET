using Cloudy.CMS.SingletonSupport;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebsite.Models
{
    [Singleton]
    public class SiteSettings
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SiteName { get; set; }
    }
}
