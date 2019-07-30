using System.Collections.Generic;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public interface IFileHandler
    {
        bool Exists(string path);
        void Create(string path, string contents);
        void Delete(string id);
        string Get(string id);
        void Update(string id, string contents);
        IEnumerable<string> List(string container);
    }
}