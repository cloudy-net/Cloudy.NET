using System.Threading.Tasks;

namespace Cloudy.CMS.Licensing
{
    public interface ILicenseProvider
    {
        Task<bool> IsValidLicenseAsync();
    }
}
