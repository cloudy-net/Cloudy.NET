namespace Cloudy.CMS.Licensing
{
    public interface ILicenseProvider
    {
        public bool IsValidLicense { get; }
    }
}
