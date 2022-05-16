using FolderComparer.Business.Data;
using FolderComparer.Business.Interfaces;
using System.IO;

namespace FolderComparer.Business.Implementations
{
    public class FileDataCreatorImp : IFileDataCreator
    {
        public FileData MakeFileDataFromLocalPath(string path)
        {
            if (!File.Exists(path))
            {
                throw new FileNotFoundException(path);
            }

            var data = new FileData
            {
                Path = path,
                LastModifiedDate = File.GetLastWriteTimeUtc(path)
            };

            return data;
        }
    }
}