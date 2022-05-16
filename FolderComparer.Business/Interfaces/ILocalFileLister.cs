using FolderComparer.Business.Data;
using System.Collections.Generic;

namespace FolderComparer.Business.Interfaces
{
    public interface ILocalFileLister
    {
        IEnumerable<FileData> GetFileDataForFolder(string folderPath);

        IEnumerable<string> GetFilePathsForFolder(string folderPath);
    }
}