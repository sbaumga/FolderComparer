using FolderComparer.Business.Data;
using FolderComparer.Business.Interfaces;
using System.Collections.Generic;

namespace FolderComparer.Business.Implementations
{
    public class SyncStatusCompilerImp : ISyncStatusCompiler
    {
        public IList<string> CompileOutput(IEnumerable<FileSynchronizationStatusData> data)
        {
            var syncOutputStrings = new List<string>();
            foreach (var file in data)
            {
                if (file.SourceData == null)
                {
                    if (file.DestinationData != null)
                    {
                        syncOutputStrings.Add($"NEW: \"{file.DestinationData.Path}\" exists in destination folder, but not source file.");
                    }
                }
                else
                {
                    if (file.DestinationData == null)
                    {
                        syncOutputStrings.Add($"MISSING: \"{file.SourceData.Path}\" exists in source file folder, but not in the destination folder.");
                    }
                    else
                    {
                        if (file.SourceData.LastModifiedDate > file.DestinationData.LastModifiedDate)
                        {
                            syncOutputStrings.Add($"OUT OF DATE: \"{file.SourceData.Path}\" has a more recent last modified date in the source file than in the destination folder.");
                        }
                    }
                }
            }

            return syncOutputStrings;
        }
    }
}