using Cloudy.CMS.SingletonSupport;
using System.ComponentModel.DataAnnotations.Schema;

namespace TestWebsite.Models
{
    public class SiteSettings : ISingleton
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string SiteName { get; set; }
    }
}
