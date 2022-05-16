using System.Collections.Generic;

namespace FolderComparer.Business.Data
{
    public class FilePersistanceData
    {
        public string BasePath { get; set; }

        public IEnumerable<FileData> Files { get; set; }
    }
}