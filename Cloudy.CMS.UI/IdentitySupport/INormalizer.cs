namespace Cloudy.CMS.UI.IdentitySupport
{
    public interface INormalizer
    {
        string NormalizeName(string name);
        string NormalizeEmail(string email);
    }
}