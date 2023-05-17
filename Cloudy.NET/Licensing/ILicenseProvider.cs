using System.Threading.Tasks;

namespace Cloudy.NET.Licensing
{
    public interface ILicenseProvider
    {
        Task<bool> IsValidLicenseAsync();
    }
}
