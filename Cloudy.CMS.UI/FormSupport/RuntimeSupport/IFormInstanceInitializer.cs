namespace Cloudy.CMS.UI.FormSupport.RuntimeSupport
{
    public interface IFormInstanceInitializer
    {
        void Initialize(object instance, FormDescriptor form);
    }
}