using System.Threading.Tasks;

namespace Cloudy.NET.Licensing
{
    internal interface ILicenseValidator
    {
        Task<bool> IsValidAsync(string licenseKey);
    }
}
