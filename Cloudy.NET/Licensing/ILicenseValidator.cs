using System.Threading.Tasks;

namespace Cloudy.CMS.Licensing
{
    internal interface ILicenseValidator
    {
        Task<bool> IsValidAsync(string licenseKey);
    }
}
