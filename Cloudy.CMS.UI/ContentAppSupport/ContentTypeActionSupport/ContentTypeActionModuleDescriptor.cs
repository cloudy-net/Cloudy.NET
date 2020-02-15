namespace Cloudy.CMS.UI.ContentAppSupport.ContentTypeActionSupport
{
    public class ContentTypeActionModuleDescriptor
    {
        public string ContentTypeId { get; }
        public string ModulePath { get; }

        public ContentTypeActionModuleDescriptor(string contentTypeId, string modulePath)
        {
            ContentTypeId = contentTypeId;
            ModulePath = modulePath;
        }
    }
}