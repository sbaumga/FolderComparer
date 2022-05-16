using FolderComparer.Business.Data;
using System.Collections.Generic;

namespace FolderComparer.Business.Interfaces
{
    public interface ISavedFileListSyncChecker
    {
        IEnumerable<FileSynchronizationStatusData> GetSynchronizationStatusForFiles(string folderPath, string expectedFolderContentsFilePath);
    }
}