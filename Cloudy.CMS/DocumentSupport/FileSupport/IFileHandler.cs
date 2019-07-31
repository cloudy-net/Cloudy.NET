using System.Collections.Generic;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public interface IFileHandler
    {
        bool Exists(string path);
        void Create(string path, string contents);
        void Delete(string path);
        string Get(string path);
        void Update(string path, string contents);
        IEnumerable<string> List(string path);
    }
}