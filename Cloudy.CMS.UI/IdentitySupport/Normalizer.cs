using Microsoft.AspNetCore.Identity;

namespace Cloudy.CMS.UI.IdentitySupport
{
    public class Normalizer : INormalizer
    {
        ILookupNormalizer KeyNormalizer { get; } = new UpperInvariantLookupNormalizer();

        public string NormalizeName(string name) => KeyNormalizer.NormalizeName(name);
        public string NormalizeEmail(string email) => KeyNormalizer.NormalizeEmail(email);
    }
}