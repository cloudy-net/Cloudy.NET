namespace Cloudy.CMS.UI.ContentAppSupport.ListActionSupport
{
    public class ListActionModuleDescriptor
    {
        public string ContentTypeId { get; }
        public string ModulePath { get; }

        public ListActionModuleDescriptor(string contentTypeId, string modulePath)
        {
            ContentTypeId = contentTypeId;
            ModulePath = modulePath;
        }
    }
}