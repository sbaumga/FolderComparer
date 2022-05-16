using FolderComparer.Business.Data;
using FolderComparer.Business.Interfaces;
using System;
using System.IO;

namespace FolderComparer.Business.Implementations
{
    public class FileDataPersisterImp : IFileDataPersister
    {
        private ISerializer Serializer { get; }

        public FileDataPersisterImp(ISerializer serializer)
        {
            Serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
        }

        public void Save(string filePath, FilePersistanceData data)
        {
            var serialized = Serializer.Serialize(data);
            File.WriteAllText(filePath, serialized);
        }

        public FilePersistanceData Load(string filePath)
        {
            var fileText = ReadFromFile(filePath);
            if (string.IsNullOrEmpty(fileText))
            {
                return null;
            }

            var data = Serializer.Deserialize<FilePersistanceData>(fileText);
            return data;
        }

        private string ReadFromFile(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }

            var fileText = File.ReadAllText(filePath);
            return fileText;
        }
    }
}