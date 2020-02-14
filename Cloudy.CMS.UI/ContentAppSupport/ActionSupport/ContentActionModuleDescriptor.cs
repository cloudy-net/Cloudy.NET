namespace Cloudy.CMS.UI.ContentAppSupport.ActionSupport
{
    public class ContentActionModuleDescriptor
    {
        public string ContentTypeId { get; }
        public string ModulePath { get; }

        public ContentActionModuleDescriptor(string contentTypeId, string modulePath)
        {
            ContentTypeId = contentTypeId;
            ModulePath = modulePath;
        }
    }
}