using Newtonsoft.Json;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class DocumentDeserializer : IDocumentDeserializer
    {
        public Document Deserialize(string contents)
        {
            return JsonConvert.DeserializeObject<Document>(contents);
        }
    }
}