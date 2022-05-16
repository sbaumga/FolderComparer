using FolderComparer.Business.Data;
using FolderComparer.Business.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FolderComparer.Business.Implementations
{
    public class SavedFileListSyncCheckerImp : ISavedFileListSyncChecker
    {
        private ILocalFileLister FileLister { get; }
        private IFileDataPersister FileDataListPersister { get; }

        public SavedFileListSyncCheckerImp(ILocalFileLister fileLister, IFileDataPersister fileDataListPersister)
        {
            FileLister = fileLister ?? throw new ArgumentNullException(nameof(fileLister));
            FileDataListPersister = fileDataListPersister ?? throw new ArgumentNullException(nameof(fileDataListPersister));
        }

        public IEnumerable<FileSynchronizationStatusData> GetSynchronizationStatusForFiles(string folderPath, string expectedFolderContentsFilePath)
        {
            var source = FileDataListPersister.Load(expectedFolderContentsFilePath);
            var sourceFiles = source.Files;
            var destinationFiles = FileLister.GetFileDataForFolder(folderPath).ToList();

            var syncData = new List<FileSynchronizationStatusData>();
            foreach (var localFile in sourceFiles)
            {
                syncData.Add(CreateFileSynchronizationStatusDataForLocalFile(source.BasePath, localFile, folderPath, destinationFiles));
            }

            syncData.AddRange(FindMissingDestinationFiles(syncData, destinationFiles));

            return syncData;
        }

        private FileSynchronizationStatusData CreateFileSynchronizationStatusDataForLocalFile(string sourceFileBasePath, FileData sourceFile, string destinationFileBasePath, IList<FileData> destinationFiles)
        {
            var expectedDestinationPathForSourcePath = sourceFile.Path.Replace(sourceFileBasePath, destinationFileBasePath);
            var matchingDestinationFile = destinationFiles.SingleOrDefault(file => file.Path == expectedDestinationPathForSourcePath);

            var data = new FileSynchronizationStatusData
            {
                SourceData = sourceFile,
                DestinationData = matchingDestinationFile
            };

            return data;
        }

        private IList<FileSynchronizationStatusData> FindMissingDestinationFiles(IList<FileSynchronizationStatusData> sourceFileSyncData, IList<FileData> destinationFiles)
        {
            var missingFiles = destinationFiles.Where(destinationFile => !sourceFileSyncData.Any(d => d.DestinationData == destinationFile));
            var syncData = missingFiles.Select(m => new FileSynchronizationStatusData
            {
                SourceData = null,
                DestinationData = m
            }).ToList();

            return syncData;
        }
    }
}