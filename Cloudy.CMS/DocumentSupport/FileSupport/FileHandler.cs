using System.Collections.Generic;
using System.IO;

namespace Cloudy.CMS.DocumentSupport.FileSupport
{
    public class FileHandler : IFileHandler
    {
        public void Create(string path, string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, contents);
        }

        public void Delete(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.Delete(path);
        }

        public bool Exists(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            return File.Exists(path);
        }

        public string Get(string path)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            return File.ReadAllText(path);
        }

        public IEnumerable<string> List(string path)
        {
            Directory.CreateDirectory(path);

            var result = new List<string>();

            foreach(var file in Directory.EnumerateFiles(path))
            {
                result.Add(File.ReadAllText(file));
            }

            return result;
        }

        public void Update(string path, string contents)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            File.WriteAllText(path, contents);
        }
    }
}