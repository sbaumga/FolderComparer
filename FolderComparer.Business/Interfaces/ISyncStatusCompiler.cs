using FolderComparer.Business.Data;
using System.Collections.Generic;

namespace FolderComparer.Business.Interfaces
{
    public interface ISyncStatusCompiler
    {
        IList<string> CompileOutput(IEnumerable<FileSynchronizationStatusData> data);
    }
}