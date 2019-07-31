using Newtonsoft.Json;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentSerializer : IDocumentSerializer
    {
        public string Serialize(Document document)
        {
            return JsonConvert.SerializeObject(document, Formatting.Indented);
        }
    }
}