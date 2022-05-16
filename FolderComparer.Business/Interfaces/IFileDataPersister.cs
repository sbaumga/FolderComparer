using FolderComparer.Business.Data;

namespace FolderComparer.Business.Interfaces
{
    public interface IFileDataPersister
    {
        FilePersistanceData Load(string filePath);

        void Save(string filePath, FilePersistanceData data);
    }
}